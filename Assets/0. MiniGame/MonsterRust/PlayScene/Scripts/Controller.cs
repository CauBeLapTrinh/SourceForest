using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.MonsterRush
{
    public class Controller : MonoBehaviour
    {
        public static Controller instance;

        int gemAmount;
        [Header("--------- ScriptsControl ---------")]
        public SpawnControl spawnControl;

        [Header("--------- PlayerControl ---------")]
        public Transform player;
        public PlayerControl playerScript;

        [Header("--------- Layer ---------")]
        public LayerMask enemyLayer;
        public LayerMask expLayer;
        [Header("--------- Item prefabs ---------")]
        public GameObject enemyDeadAnim;
        public GameObject textHit;
        public List<GameObject> weapons;
        public List<GameObject> exps;

        [Header("--------- TextGem ---------")]
        public Text txtGem;
        private void Awake()
        {
            instance = this;

            txtGem.text = $"{GetGemAmount()}";
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Quit()
        {
            Application.Quit();
        }
        public int GetGemAmount()
        {
            gemAmount = PlayerPrefs.GetInt("MonsterRush_GemAmount");

            return gemAmount;
        }
        public void TakeGem(int setGem)
        {
            StartCoroutine(IETakeGem(setGem));
        }
        public IEnumerator IETakeGem (int setGem)
        {
            int temp = gemAmount + setGem;

            while (gemAmount < temp)
            {
                gemAmount += 1;

                txtGem.text = $"{gemAmount}";

                yield return new WaitForSeconds(0.02f);
            }

            PlayerPrefs.SetInt("MonsterRush_GemAmount", gemAmount);
        }
    }
}
