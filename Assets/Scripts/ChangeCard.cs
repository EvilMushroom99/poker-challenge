using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCard : MonoBehaviour
{
    //Selected cards to change it
    [SerializeField] private int cardNumber;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameObject table;

    //Sounds
    [SerializeField] private AudioSource selectSound;

    //Add selected cards by the player to the list of cards to be changed
    public void AddCardToArray()
    {
        if (deckManager.cardsChangedCount < 3 && !deckManager.cardsToChangeNumbers.Contains(cardNumber))
        {
            selectSound.Play();
            table.transform.GetChild(cardNumber).GetComponent<SpriteRenderer>().enabled = true; 
            deckManager.cardsToChangeNumbers.Add(cardNumber); 
            deckManager.cardsChangedCount++;
        }
        else if (deckManager.cardsToChangeNumbers.Contains(cardNumber)) 
        {
            selectSound.Play();
            table.transform.GetChild(cardNumber).GetComponent<SpriteRenderer>().enabled = false; 
            deckManager.cardsToChangeNumbers.Remove(cardNumber); 
            deckManager.cardsChangedCount--;
        }
    }
}

