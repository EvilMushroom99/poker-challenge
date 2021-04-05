using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    //public
    public int cardsChangedCount = 0;
    public List<int> cardsToChangeNumbers;
    public string pokerHand = "Nothing";


    //private
    [SerializeField] private GameObject[] cards;
    [SerializeField] private GameObject table;
    [SerializeField] private float timeToReset;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Button[] buttons;
    private List<GameObject> dropedCards = new List<GameObject>();


    //Sounds
    [SerializeField] private AudioSource changeSound;
    [SerializeField] private AudioSource dropSound;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DropCards(0.1f));
    }

    //PokerHand check what cards and what pokerhand the player has
    IEnumerator PokerHand()
    {
        yield return new WaitForSeconds(0.5f);
        RepeatedCards();
        Straight();
        Flush();
        StartCoroutine(ResetDeck());
        scoreManager.SumPoints();
    }

    //ReapeatedCards count the cards by his numbers and set the "pokerhand" to: "Pair", "Two Pair","Three of a kind", "Full House" , or "Poker"
    private void RepeatedCards()
    {
        Dictionary<int, int> cardsCount = new Dictionary<int, int>();

        for (int i = 0; i < 5; i++)
        {
            int num = table.transform.GetChild(i).gameObject.GetComponentInChildren<Card>().number;
            if (cardsCount.ContainsKey(num))
            {
                cardsCount[num] += 1;
            }
            else {
                cardsCount.Add(num, 1);
            }
        }
        if(cardsCount.Count < 5)
        {
            int pairCount = 0;
            bool threeOAK = false;
            foreach (int value in cardsCount.Values)
            {
                if (value == 2)
                {
                    if (pairCount == 1)
                    {
                        pokerHand="Two Pair";
                        break;
                    }
                    else
                    {
                        pokerHand="Pair";
                        if (threeOAK) pokerHand = "Full House";
                        pairCount++;
                    }
                }
                else if (value == 3)
                {
                    pokerHand="Three of a kind";
                    threeOAK = true;
                    if (pairCount == 1) pokerHand = "Full House";
                }
                else if (value == 4)
                {
                    pokerHand="Poker";
                    break;
                }
            }
        }
    }

    //Straight sort the cards by his numbers and check if last + 1 == next, then, if true, set "pokerhand" to "Straight"
    private void Straight()
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            int num = table.transform.GetChild(i).gameObject.GetComponentInChildren<Card>().number;
            numbers.Add(num);
        }
        numbers.Sort();
        int lastNum = numbers[0];
        bool isStraigh = false;
        for (int i = 1; i < 5; i++)
        {
            if (numbers[i] == (lastNum + 1))
            {
                lastNum = numbers[i];
                isStraigh = true;
            }
            else
            {
                if (numbers[0] == 2 && numbers[i] == 14 && i == 4) isStraigh = true; //but if the last is 14(As) and start with 2(2,3,4,5,14(As))
                else isStraigh = false;
                break;
            }
        }
        if (isStraigh) pokerHand = "Straight";
    }


    //Flush check if the last suit is equal to the next suit, then, if true, set "pokerhand" to "Flush"
    private void Flush()
    {
        bool isFlush = false;
        string lastSuit = table.transform.GetChild(0).gameObject.GetComponentInChildren<Card>().suit;
        for (int i = 1; i < 5; i++)
        {
            if (table.transform.GetChild(i).gameObject.GetComponentInChildren<Card>().suit == lastSuit)
            {
                isFlush = true;
            }
            else
            {
                isFlush = false;
                break;
            }
        }
        if (isFlush) pokerHand = "Flush";
    }

    //DropCards drop random cards in the table, checking if the card isn't droped alredy
    IEnumerator DropCards(float timeForDrop)
    {
        yield return new WaitForSeconds(timeForDrop);
        if(dropedCards.Count == 0)
        {
            int i = 0;
            while (i < 5)
            {
                int randomNumber = Random.Range(0, 51);
                GameObject card = cards[randomNumber];
                if (!dropedCards.Contains(card))
                {
                    Transform position = table.transform.GetChild(i);
                    GameObject newCard = Instantiate(card, position.position, position.rotation);
                    newCard.transform.SetParent(position);
                    dropedCards.Add(card); 
                    i++;
                }
            }
            dropSound.Play();
        }
    }

    //ChangeCard change the selected cards by the player and drope a new one cheking that isn't alredy in the table or selected by the player
    private void ChangeCard(List<int> cardsToChangeNumbers)
    {
        for(int i = 0; i < cardsToChangeNumbers.Count; i++) 
        {
            int randomNumber = Random.Range(0, 51);
            GameObject card = cards[randomNumber];
            while (dropedCards.Contains(card))
            {
                randomNumber = Random.Range(0, 51);
                card = cards[randomNumber];
            }
            Transform cardToChangePosition = table.transform.GetChild(cardsToChangeNumbers[i]);
            DestroyImmediate(cardToChangePosition.GetChild(0).gameObject, false);
            table.transform.GetChild(cardsToChangeNumbers[i]).GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(SwapCards());

            IEnumerator SwapCards()
            {
                yield return new WaitForEndOfFrame();
                GameObject newCard = Instantiate(card, cardToChangePosition.position, cardToChangePosition.rotation);
                newCard.transform.SetParent(cardToChangePosition);
                dropedCards.Add(card);
                cardsChangedCount++;
            }
        }

    }

    //Prepares ChangeCard and PokerHand funtions, checking if they are cards selected to be changed, if not, just PokerHand be called
    public void SendCardsTobeChanged()
    {
        changeSound.Play();
        DisableButtons();
        if (cardsChangedCount > 0 && cardsChangedCount < 4)
        {
            ChangeCard(cardsToChangeNumbers);
        }
        StartCoroutine(PokerHand()); 
    }

    //ResetDeck is called when the hand is over and restart the game
     IEnumerator ResetDeck() 
    {
        yield return new WaitForSeconds(timeToReset);
        //Reset all the values and lists, destroy all the cards
        dropedCards = new List<GameObject>();
        cardsToChangeNumbers = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            Transform cardToChangePosition = table.transform.GetChild(i);
            Destroy(cardToChangePosition.GetChild(0).gameObject);
        }
        cardsChangedCount = 0;
        pokerHand = "Nothing";
        StartCoroutine(DropCards(0.2f));
        ActivateButtons();
    }

    private void DisableButtons()
    {
        foreach(Button button in buttons)
        {
            button.interactable = false;
        }
    }

    private void ActivateButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
}