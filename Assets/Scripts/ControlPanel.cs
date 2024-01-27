using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    public static ControlPanel instance;

    static bool gameIsPause = false;
    [Header("Text")]
    public Text countBeerKill;
    public Text countTreeKill;

    public int countBeerEnemy = 0;
    public int countTreeEnemy = 0;
    [Header("Audio")]
    public AudioSource finishGame;
    public AudioSource audioBGM;
    [Header("Panel")]
    public GameObject panelEsc;
    public GameObject panelMission;
    public GameObject panelMinimap;

    bool checkPanelMinimap = true;
    bool isOnPanelMission = true;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        countBeerKill.text = countBeerEnemy.ToString() + " / " + ControlScenes.instance.amountBeer;
        countTreeKill.text = countTreeEnemy.ToString() + " / " + ControlScenes.instance.amountTree;
    }
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

        ControlMiniMap.isMiniMapOn = true;
    }
    public void CloseMiniMap()
    {
        panelMinimap.SetActive(false);

        ControlMiniMap.isMiniMapOn = false;
    }
    void Pause()
    {
        panelEsc.SetActive(true);
        Time.timeScale = 0f;
        audioBGM.Pause();
        gameIsPause = true;
    }

    public void ClosePanelMission()
    {
        panelMission.SetActive(false);

        isOnPanelMission = false;
    }
    public void ShowPanelMission()
    {
        panelMission.SetActive(true);

        isOnPanelMission = true;
    }
    public bool IsOnPanelMission()
    {
        return isOnPanelMission;
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
                ShowMiniMap();
                checkPanelMinimap = true;
            }
            else
            {
                CloseMiniMap();
                checkPanelMinimap = false;
            }
        }
    }
}
