using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMarket : MonoBehaviour
{
    public GameObject pnlNotify;

    public Text yourCoinText;
    public Text yourGemText;

    int priceCherrie = 3;
    int priceOrange = 5;

    int yourCoin;
    int yourGem;

    private void Start()
    {
        yourCoin = Money.coin;
        yourGem = Money.gem;

        yourCoinText.text = yourCoin.ToString();
        yourGemText.text = yourGem.ToString();
    }

    public void BuyCherrie()
    {
        if (priceCherrie <= yourGem)
        {
            yourGem -= priceCherrie;

            Player.damage += 5;

            yourGemText.text = yourGem.ToString();
        }
        else
        {
            pnlNotify.SetActive(true);
        }
    }
    public void BuyOrange()
    {
        if (priceOrange <= yourCoin)
        {
            yourCoin -= priceOrange;

            Player.health += 10;

            yourCoinText.text = yourCoin.ToString();
        }
        else
        {
            pnlNotify.SetActive(true);
        }
    }
    public void Continue()
    {
        Money.coin = yourCoin;
        Money.gem = yourGem;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ClosePanelNotify()
    {
        pnlNotify.SetActive(false);
    }
}
