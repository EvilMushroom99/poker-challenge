using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objsToActivate;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject highScore;
    [SerializeField] private GameObject exitButton;

    //Sounds
    [SerializeField] private AudioSource panelUpSound;
    [SerializeField] private AudioSource panelDownSound;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }

    public void ActivateMenuButtons()
    {
        panelUpSound.Play();
        playButton.SetActive(true);
        highScore.SetActive(true);
        exitButton.SetActive(true);
    }

    public void DesactivateMenuButton()
    {
        panelDownSound.Play();
        playButton.SetActive(false);
        highScore.SetActive(false);
        exitButton.SetActive(false);
    }

    public void DesactivateMenuPanel()
    {
        this.gameObject.SetActive(false);
    }

    public void StartCloseAnimation()
    {
        anim.SetBool("Close", true);
    }

    //Activate the buttons and deck
    public void GameStart()
    {
        foreach (GameObject obj in objsToActivate)
        {
            obj.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
