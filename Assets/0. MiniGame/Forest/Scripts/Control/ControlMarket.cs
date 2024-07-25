using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Minigame.Forest
{
    public class ControlMarket : MonoBehaviour
    {
        public static ControlMarket instance;
        public GameObject pnlNotify;

        public Text yourCoinText;
        public Text yourGemText;

        int yourCoin;
        int yourGem;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            yourCoin = Money.coin;
            yourGem = Money.gem;

            yourCoinText.text = yourCoin.ToString();
            yourGemText.text = yourGem.ToString();
        }

        public void BuyCherrie(int price)
        {
            if (price <= yourCoin)
            {
                yourCoin -= price;

                StaticPropertis.damage += 2;

                yourCoinText.text = yourCoin.ToString();
            }
            else
            {
                pnlNotify.SetActive(true);
            }
        }
        public void BuyOrange(int price)
        {
            if (price <= yourCoin)
            {
                yourCoin -= price;

                StaticPropertis.health += 10;

                yourCoinText.text = yourCoin.ToString();
            }
            else
            {
                pnlNotify.SetActive(true);
            }
        }
        public void BuySholve(int price)
        {
            if (price <= yourGem)
            {
                yourGem -= price;

                StaticPropertis.indexWeapon.Add(0);

                yourGemText.text = yourGem.ToString();
            }
            else
            {
                pnlNotify.SetActive(true);
            }
        }
        public void BuyTrident(int price)
        {
            if (price <= yourGem)
            {
                yourGem -= price;

                StaticPropertis.indexWeapon.Add(1);

                yourGemText.text = yourGem.ToString();
            }
            else
            {
                pnlNotify.SetActive(true);
            }
        }
        public void BuySickle(int price)
        {
            if (price <= yourGem)
            {
                yourGem -= price;

                StaticPropertis.indexWeapon.Add(2);

                yourGemText.text = yourGem.ToString();
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

}
