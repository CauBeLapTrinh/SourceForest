using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountKillEnemy : MonoBehaviour
{
    public static int countBeerEnemy = 0;
    public static int countTreeEnemy = 0;

    public AudioSource finishGame;
    public GameObject panelMission;

    
    bool isNextScene = false;
    public void closePanelMission()
    {
        panelMission.SetActive(false);
    }
    public void showPanelMission()
    {
        panelMission.SetActive(true);
    }

    private void Update()
    {
        if (countBeerEnemy == ControlScenes.amountBeer && countTreeEnemy == ControlScenes.amountTree && isNextScene == false)
        {
            finishGame.Play();
            Invoke("nextScenes", 5);
            isNextScene = true;

            countBeerEnemy = 0;
            countTreeEnemy = 0;
        }
    }
    public void nextScenes()
    {
        ControlScenes.nextScene();
    }
}
