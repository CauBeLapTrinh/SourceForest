using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.IO;
using System.IO;

public class ControlScenes : MonoBehaviour
{
    public Text yourCoinText;
    public Text yourGemText;

    public static int amountTree = 0;
    public static int amountBeer = 0;

    int loadCoin;
    int loadGem;

    static int getSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        getSceneIndex = scene.buildIndex;

        Debug.Log(getSceneIndex);
        if (getSceneIndex == 1)
        {
            amountBeer = 8;
            amountTree = 8;
        }else if (getSceneIndex == 3)
        {
            amountBeer = 14;
            amountTree = 10;
        }

        loadCoin = Money.coin;
        loadGem = Money.gem;

        yourCoinText.text = loadCoin.ToString();
        yourGemText.text = loadGem.ToString();

        Debug.Log("so luong enemttree: "+amountTree);
        Debug.Log("so luong enemtbeer: " + amountBeer);
    }

    public static void nextScene()
    {
        Money.coin = ItemCollect.countCoin;
        Money.gem = ItemCollect.countGem;

        SceneManager.LoadScene(getSceneIndex+1);
    }
}
