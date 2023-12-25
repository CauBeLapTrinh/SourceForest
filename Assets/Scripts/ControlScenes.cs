using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.IO;
using System.IO;

public class ControlScenes : MonoBehaviour
{
    public static ControlScenes instance;
    public static bool isNextScene;

    public Text yourCoinText;
    public Text yourGemText;

    public int amountTree;
    public int amountBeer;

    int loadCoin;
    int loadGem;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        loadCoin = Money.coin;
        loadGem = Money.gem;

        yourCoinText.text = "Coin: " + loadCoin.ToString();
        yourGemText.text = "Gem: " + loadGem.ToString();

        isNextScene = false;
    }

    public void CheckWin()
    {
        StartCoroutine(IECheckWin());
    }

    IEnumerator IECheckWin()
    {
        if (ControlPanel.instance.countBeerEnemy == amountBeer
            && ControlPanel.instance.countTreeEnemy == amountTree
            && !isNextScene)
        {
            isNextScene = true;

            yield return new WaitForSeconds(5f);

            instance.NextScene();
        }
    }

    public void NextScene()
    {
        Money.coin = ItemCollect.countCoin;
        Money.gem = ItemCollect.countGem;

        ControlPanel.instance.countBeerEnemy = 0;
        ControlPanel.instance.countTreeEnemy = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
