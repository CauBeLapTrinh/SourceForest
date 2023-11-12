using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollect : MonoBehaviour
{
    public static int countCoin = 0;
    public static int countGem = 0;
    
    public Text txtCountCoin;
    public Text txtCountGem;

    public AudioSource audioCollectFruit;
    public AudioSource audioCollectCoinOrGem;

    public GameObject collectEffect;

    private void Start()
    {
        countCoin = Money.coin;
        countGem = Money.gem;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag ==  "Item")
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Fruits")) 
            {
                audioCollectFruit.Play();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("CoinGem"))
            {
                if (collision.gameObject.name.IndexOf("Coin") == 0)
                {
                    countCoin++;
                    txtCountCoin.text = countCoin.ToString();
                }else if (collision.gameObject.name.IndexOf("Gem") == 0)
                {
                    countGem++;
                    txtCountGem.text = countGem.ToString();
                }
                audioCollectCoinOrGem.Play();
            }
            
            Instantiate(collectEffect, collision.transform.position, collision.transform.rotation);

            Destroy(collision.gameObject);
        }
    }
}
