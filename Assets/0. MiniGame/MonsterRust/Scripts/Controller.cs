using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class Controller : MonoBehaviour
    {
        public static Controller instance;

        [Header("--------- PlayerControl ---------")]
        public Transform player;
        public PlayerControl playerScript;

        [Header("--------- Layer ---------")]
        public LayerMask enemyLayer;
        public LayerMask expLayer;
        [Header("--------- Item prefabs ---------")]
        public GameObject enemyDeadAnim;
        public GameObject textHit;
        public GameObject boxItem;
        public List<GameObject> weapons;
        public List<GameObject> exps;
        private void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
