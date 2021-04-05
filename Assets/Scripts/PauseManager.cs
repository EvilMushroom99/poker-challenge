using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objs;
    [SerializeField] private GameObject menuPanel;

    //Sounds
    [SerializeField] private AudioSource pauseSound;

    public void PauseGame()
    {
        pauseSound.Play();
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
        menuPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
