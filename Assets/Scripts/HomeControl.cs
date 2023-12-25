using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeControl : MonoBehaviour
{
    bool onOffVolume = true;
    public AudioSource bgMusic;
    public GameObject panelTutorial;
    public GameObject panelTutorial1;
    public GameObject panelTutorial2;

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void btnOnOffVolume()
    {
        if (onOffVolume)
        {
            bgMusic.Pause();
            onOffVolume = false;
        }
        else if (!onOffVolume)
        {
            bgMusic.Play();
            onOffVolume = true;
        }
    }
    public void btnShowPanelTutorial()
    {
        panelTutorial.SetActive(true);
    }
    public void btnOffPanelTutorial()
    {
        panelTutorial.SetActive(false);
    }
    public void btnNextTutorial()
    {
        panelTutorial1.SetActive(false);
        panelTutorial2.SetActive(true);
    }
    public void btnBackTutorial()
    {
        panelTutorial1.SetActive(true);
        panelTutorial2.SetActive(false);
    }
}
