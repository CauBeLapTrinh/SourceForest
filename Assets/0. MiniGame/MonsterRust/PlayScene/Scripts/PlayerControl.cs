using DG.Tweening.Core.Easing;
using Minigame.Forest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class PlayerControl : MonoBehaviour
    {
        public GunControl gun;
        public ExpBar expBar;
        public PlayerMovement playerMovement;
        [Header("--------- Properties ---------")]
        [Header("--- Attack ---")]
        public float damage;
        public float rangeShoot;
        public float speedShoot;
        public MeleeRotate meleeRotate;

        float nextShoot = 0;
        [Header("--- Health ---")]
        public float maxHealth;
        float currentHealth;

        [Header("--------- Exp ---------")]
        public float rangeTakeExp;

        [Header("--------- HealthBar ---------")]
        public ProgressBar healthBar;
        [Header("--------- Hand ---------")]
        public SpriteRenderer leftHand;
        public SpriteRenderer rightHand;

        bool isDead = false;
        bool isNearEnemy = false;
        bool isFacingRight = true;
        // --------------------------------------
        int levelSpeed = 0;
        int[] speedLv = { 0, 2, 4, 6, 8, 11, 15 };

        // Start is called before the first frame update
        void Start()
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
            gun = GetComponentInChildren<GunControl>();
            currentHealth = maxHealth;
            healthBar.SetMaxValue(maxHealth);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextShoot)
            {
                Shoot();
                nextShoot = Time.time + 1 / speedShoot;
            }

            CheckRangeExp();
        }
        public void CheckRangeExp()
        {
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, rangeTakeExp, Controller.instance.expLayer);
            if (colls.Length > 0)
            {
                foreach (var item in colls)
                {
                    ExpControl expItem = item.GetComponent<ExpControl>();
                    expItem.SetTarget(transform);
                }
            }
        }
        public void TakeExp(float expTake)
        {
            expBar.TakeExperience(expTake);
        }

        public bool IsNearEnemy()
        {
            return isNearEnemy;
        }
        public Transform GetEnemyNearest()
        {
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, rangeShoot, Controller.instance.enemyLayer);

            if (colls.Length == 0)
            {
                return null;
            }

            float minDistance = 100f;
            int indexNearest = 0;
            for (int i = 0; i < colls.Length; i++)
            {
                float distanceCurrent = Vector2.Distance(colls[i].transform.position, transform.position);

                if (distanceCurrent < minDistance)
                {
                    minDistance = distanceCurrent;
                    indexNearest = i;
                }
            }
            return colls[indexNearest].transform;
        }
        public void Shoot()
        {
            Transform enemyNearest = GetEnemyNearest();
            if (enemyNearest != null)
            {
                isNearEnemy = true;
                if (enemyNearest.position.x < transform.position.x)
                {
                    SetFacingLeft();
                }
                else if (enemyNearest.position.x > transform.position.x) 
                {
                    SetFacingRight();
                }

                gun.Shoot(enemyNearest);
            }
            else
            {
                isNearEnemy = false;
            }
        }

        
        public void TakeDame(float damage)
        {
            if (isDead) return;

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                isDead = true;

                currentHealth = 0;
            }

            healthBar.SetValue(currentHealth);
            healthBar.SetText($"{currentHealth}/{maxHealth}");
        }

        public void SetMelee(GameObject weaponPrefab)
        {
            MeleeWeaponControl meleeWeaponControl = weaponPrefab.GetComponent<MeleeWeaponControl>();
            rightHand.sprite = meleeWeaponControl.spriteWeapon;

            meleeRotate.AddWeapon(weaponPrefab);
            meleeRotate.SetSpeed(meleeWeaponControl.speed);
        }
        public void SetGun(GameObject gunPrefab)
        {
            GunControl gunControl = gunPrefab.GetComponent<GunControl>();
            leftHand.sprite = gunControl.gunImg;

            gun.SetGun(gunControl);
        }
        public void AddMaxHeal()
        {
            maxHealth += 5;
            healthBar.SetMaxValue(maxHealth);
            healthBar.SetValue(currentHealth);
            healthBar.SetText($"{currentHealth}/{maxHealth}");
        }
        public void SpeedUp()
        {
            levelSpeed += 1;
            Debug.Log(levelSpeed);
            Debug.Log(speedLv[levelSpeed]);
            playerMovement.SetSpeedUp(speedLv[levelSpeed]);
        }
        public int GetLevelSpeed()
        {
            return levelSpeed;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rangeShoot);
            Gizmos.DrawWireSphere(transform.position, rangeTakeExp);
        }
        public bool IsFacingRight()
        {
            return isFacingRight;
        }

        public void SwapFace()
        {
            isFacingRight = !isFacingRight;

            Vector3 theScale = transform.localScale;
            theScale.x = -theScale.x;

            transform.localScale = theScale;
        }
        public void SetFacingRight()
        {
            isFacingRight = true;

            Vector3 theScale = transform.localScale;
            theScale.x = 1f;

            transform.localScale = theScale;
        }
        public void SetFacingLeft()
        {
            isFacingRight = false;

            Vector3 theScale = transform.localScale;
            theScale.x = -1f;

            transform.localScale = theScale;
        }
    }
}
