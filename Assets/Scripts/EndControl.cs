using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndControl : MonoBehaviour
{
    bool onOffVolume = true;
    public AudioSource bgMusic;
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
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
}
