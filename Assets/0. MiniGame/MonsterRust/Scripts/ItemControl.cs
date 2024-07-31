using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class ItemControl : MonoBehaviour
    {
        [Header("--------- Weapon ---------")]
        public bool isWeapon;
        public GameObject weaponPrefab;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerControl playerControl = collision.GetComponent<PlayerControl>();

                if (isWeapon)
                {
                    playerControl.SetMelee(weaponPrefab);
                }

                Destroy(gameObject);
            }
        }
    }

}
