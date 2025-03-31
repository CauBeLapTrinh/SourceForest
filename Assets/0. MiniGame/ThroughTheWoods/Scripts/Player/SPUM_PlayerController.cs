using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
namespace ThroughTheWoods
{
    public class SPUM_PlayerController : MonoBehaviour, IHealth
    {
        public Transform posAttack;
        [Header("--- Properties ---")]
        [Header("Attack")]
        public int damageDefault;
        public int cristical;
        int attributeCount = 5;
        int curAttributeHeal = 0;
        int curAttributeDamage = 0;
        int curAttributeCristical = 0;
        [Header("Health")]
        public ProgressBar healBar;
        public float maxHp;
        float currentHp;
        float delayRecoveryHp = 1f;
        [Header("Mana")]
        public ProgressBar mpBar;
        public float maxMp;
        float currentMp;
        float delayRecoveryMp = 1f;
        [Header("Level")]
        public ProgressBar expBar;
        int curLevel = 1;
        int curExp = 0;
        [Header("SPUM")]
        SPUM_PlayerMovement playerMovement;
        public SPUM_Prefabs _prefabs;
        public Dictionary<PlayerState, int> IndexPair = new();

        void Start()
        {
            playerMovement = GetComponent<SPUM_PlayerMovement>();

            if (_prefabs == null)
            {
                _prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
            }

            foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
            {
                IndexPair[state] = 0;
            }

            LoadCharacterBar();
            LoadInfoUI();
            //SetStateAnimationIndex(PlayerState.ATTACK, 3);
        }

        public void SetStateAnimationIndex(PlayerState state, int index = 0)
        {
            IndexPair[state] = index;
        }

        public void PlayStateAnimation(PlayerState state)
        {
            _prefabs.PlayAnimation(state, IndexPair[state]);
        }

        // Update is called once per frame
        void Update()
        {
            // Kiểm tra nếu người chơi click chuột
            if (Input.GetMouseButtonDown(0)) // 0 là nút chuột trái
            {
                // Lấy vị trí click chuột trong thế giới game
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Đảm bảo z = 0 để không ảnh hưởng đến 2D

                // Tính vector hướng từ nhân vật đến vị trí click chuột
                Vector2 direction = (mousePosition - transform.position).normalized;

                if (!playerMovement.isAction)
                {
                    StartCoroutine(Attack(direction));
                }
            }

            RecoveryHp();
            RecoveryMp();
        }

        Collider2D[] enemys;
        public IEnumerator Attack(Vector2 dir)
        {
            if (currentMp < 10)
            {
                yield break;
            }
            currentMp -= 10;
            mpBar.SetValue(currentMp);
            mpBar.SetText($"{currentMp}/{maxMp}");

            playerMovement.FaceControl(dir.x);
            playerMovement.isAction = true;
            playerMovement.StopMovement();
            PlayStateAnimation(PlayerState.ATTACK);
            enemys = Physics2D.OverlapBoxAll(posAttack.position, Vector2.one, 0, Controller.instance.enemyLayer);
            yield return new WaitForSeconds(0.2f);
            HitEnemy();
            yield return new WaitForSeconds(0.2f);
            EndActionAttack();
        }
        public void EndActionAttack()
        {
            playerMovement.isAction = false;
        }
        public void HitEnemy()
        {
            if (enemys.Length > 0)
            {
                foreach (var enemy in enemys)
                {
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.SetTargetFollow(transform);
                    Health health = enemy.GetComponent<Health>();
                    int dame = UnityEngine.Random.Range(damageDefault - 5, damageDefault + 6);

                    if (UnityEngine.Random.Range(0, 100) < cristical)
                    {
                        dame *= 2;
                        health.TakeDamage(dame, true);
                    }
                    else
                    {
                        health.TakeDamage(dame, false);
                    }

                }
            }
        }
        public void TakeDamage(float damage, bool isCristical)
        {
            currentHp -= damage;
            Vector2 posSpawn = transform.position + Vector3.up;
            GameObject textHit = Instantiate(Controller.instance.controlPrefabs.textHit, posSpawn, Quaternion.identity);

            TextHit scriptText = textHit.GetComponent<TextHit>();
            if (isCristical)
            {
                scriptText.SetText($"-{damage}", Color.yellow);
            }
            else
            {
                scriptText.SetText($"-{damage}");
            }

            PlayStateAnimation(PlayerState.DAMAGED);

            if (currentHp <= 0)
            {
                currentHp = 0;

                Dead();
            }
            healBar.SetValue(currentHp);
            healBar.SetText($"{currentHp}/{maxHp}");
            Controller.instance.controlCanvasUI.healthText.text = $"{currentHp}/{maxHp}";
        }
        public void Dead()
        {

        }
        public void RecoveryHp()
        {
            delayRecoveryHp -= Time.deltaTime;

            if (delayRecoveryHp <= 0)
            {
                currentHp += 3;
                if (currentHp > maxHp)
                {
                    currentHp = maxHp;
                }
                healBar.SetValue(currentHp);
                healBar.SetText($"{currentHp}/{maxHp}");
                delayRecoveryHp = 1f;
            }
        }
        public void RecoveryMp()
        {
            delayRecoveryMp -= Time.deltaTime;

            if (delayRecoveryMp <= 0)
            {
                currentMp += 2;
                if (currentMp > maxMp)
                {
                    currentMp = maxMp;
                }
                mpBar.SetValue(currentMp);
                mpBar.SetText($"{currentMp}/{maxMp}");
                delayRecoveryMp = 1f;
            }
        }
        public void LoadCharacterBar()
        {
            currentHp = maxHp;
            healBar.SetMaxValue(maxHp);
            healBar.SetValue(currentHp);
            healBar.SetText($"{currentHp}/{maxHp}");

            currentMp = maxMp;
            mpBar.SetMaxValue(maxMp);
            mpBar.SetValue(currentMp);
            mpBar.SetText($"{currentMp}/{maxMp}");

            Controller.instance.controlCanvasUI.levelText.text = $"Level: {curLevel}";
        }
        public void LoadInfoUI()
        {
            Controller.instance.controlCanvasUI.attributeCountText.text = $"{attributeCount}";
            HealUpdate(0);
            DamageUpdate(0);
            CristicalUpdate(0);
        }
        public void HealUpdate(int heal)
        {
            maxHp += heal;

            Controller.instance.controlCanvasUI.healthText.text = $"{currentHp}/{maxHp}";
        }
        public void DamageUpdate(int damage)
        {
            damageDefault += damage;

            Controller.instance.controlCanvasUI.damageText.text = $"{damageDefault - 5}-{damageDefault + 5}";
        }
        public void CristicalUpdate(int cristicalPlus)
        {
            cristical += cristicalPlus;

            Controller.instance.controlCanvasUI.cristicalText.text = $"{cristical}%";
        }
        public void UpHealAttribute()
        {
            if (attributeCount == 0) return;

            attributeCount -= 1;
            curAttributeHeal += 1;
            Controller.instance.controlCanvasUI.PlusAttribute(EAttribute.Health, attributeCount, curAttributeHeal);
            HealUpdate(10);
        }
        public void UpDamageAttribute()
        {
            if (attributeCount == 0) return;

            attributeCount -= 1;
            curAttributeDamage += 1;
            Controller.instance.controlCanvasUI.PlusAttribute(EAttribute.Damage, attributeCount, curAttributeDamage);
            DamageUpdate(5);
        }
        public void UpCristicalAttribute()
        {
            if (attributeCount == 0) return;

            attributeCount -= 1;
            curAttributeCristical += 1;
            Controller.instance.controlCanvasUI.PlusAttribute(EAttribute.Defend, attributeCount, curAttributeCristical);
            CristicalUpdate(3);
        }
        void OnDrawGizmos()
        {
            if (posAttack == null)
                return;

            // Đặt màu cho Gizmos
            Gizmos.color = Color.red;

            // Vẽ hình hộp (box) bằng Gizmos
            Gizmos.DrawWireCube(posAttack.position, Vector3.one);
        }
    }
}