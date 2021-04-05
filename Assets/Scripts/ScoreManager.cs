using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private List<GameObject> popUps = new List<GameObject>();
    [SerializeField] private Text textScore;
    [SerializeField] private Text textHighScore;

    private Dictionary<string, int> pokerHands = new Dictionary<string,int>();
    private Dictionary<string, GameObject> popUpsParticles = new Dictionary<string, GameObject>();
    private int score;

    //Sounds
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;

    private void Awake()
    {
        ConfigDictOfPokerHands();
        ConfigDictOfPOPUPS();
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        textScore.text = score.ToString();
        textHighScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    //Check what point to sum by the pokerhand and check is score is bigger than highscore
    public void SumPoints()
    {
        if (deckManager.pokerHand != "Nothing")
        {
            winSound.Play();
            score += pokerHands[deckManager.pokerHand];
            GameObject popUp = Instantiate(popUpsParticles[deckManager.pokerHand]);
        }
        else if (score >= 2)
        {
            loseSound.Play();
            score -= 2;
        }
        else 
        {
            loseSound.Play();
            score = 0;
        } 
            
        if(score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            textHighScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
        textScore.text = score.ToString();
    }

    private void ConfigDictOfPokerHands()
    {
        pokerHands.Add("Nothing", 0);
        pokerHands.Add("Pair", 1);
        pokerHands.Add("Two Pair", 3);
        pokerHands.Add("Three of a kind", 5);
        pokerHands.Add("Straight", 10);
        pokerHands.Add("Flush",15);
        pokerHands.Add("Full House",18);
        pokerHands.Add("Poker", 20);
    }

    private void ConfigDictOfPOPUPS()
    {
        popUpsParticles.Add("Pair",popUps[0]);
        popUpsParticles.Add("Two Pair", popUps[1]);
        popUpsParticles.Add("Three of a kind", popUps[2]);
        popUpsParticles.Add("Straight", popUps[3]);
        popUpsParticles.Add("Flush", popUps[4]);
        popUpsParticles.Add("Full House", popUps[5]);
        popUpsParticles.Add("Poker", popUps[6]);
    }
}
