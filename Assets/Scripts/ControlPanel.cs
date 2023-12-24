using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPanel : MonoBehaviour
{
    static bool gameIsPause = false;

    public GameObject panelEsc;
    public GameObject panelMinimap;

    public AudioSource audioBGM;

    bool checkPanelMinimap = true;

    public void gotoHomeScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        panelEsc.SetActive(false);
        Time.timeScale = 1f;
        audioBGM.Play();
        gameIsPause = false;
    }

    public void ShowMiniMap()
    {
        panelMinimap.SetActive(true);

        ControlMiniMap.instance.isMiniMapOn = true;
    }
    public void CloseMiniMap()
    {
        panelMinimap.SetActive(false);

        ControlMiniMap.instance.isMiniMapOn = false;
    }
    void Pause()
    {
        panelEsc.SetActive(true);
        Time.timeScale = 0f;
        audioBGM.Pause();
        gameIsPause = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPause) Pause();
            else Resume();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!checkPanelMinimap)
            {
                panelMinimap.SetActive(true);
                checkPanelMinimap = true;
            }
            else
            {
                panelMinimap.SetActive(false);
                checkPanelMinimap = false;
            }
        }
    }
}
