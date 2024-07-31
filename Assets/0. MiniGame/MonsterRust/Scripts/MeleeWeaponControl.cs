using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class MeleeWeaponControl : MonoBehaviour
    {
        [Header("--------- Properties ---------")]
        public int indexWeapon;
        public float damage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                MonsterControl monsterControl = collision.GetComponent<MonsterControl>();
                monsterControl.TakeDame(damage);
            }
        }
    }

}
