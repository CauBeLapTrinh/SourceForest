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

            expBar.SetMaxValue(GetExpToNextLevel());
            expBar.SetValue(curExp);

            LoadCharacterBar();
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
            if (Input.GetMouseButtonDown(0) && !Controller.instance.controlCanvasUI.isOncanvas) // 0 là nút chuột trái
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
            LoadInfoUI();
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
                    if (enemyScript.IsDead()) break;
                    Health health = enemy.GetComponent<Health>();
                    int damage = UnityEngine.Random.Range(damageDefault - 5, damageDefault + 6);

                    bool isCritical = UnityEngine.Random.Range(0, 100) < cristical;
                    if (isCritical)
                    {
                        damage *= 2;
                        health.TakeDamage(damage, true);
                    }
                    else
                    {
                        health.TakeDamage(damage, false);
                    }

                    // Tính toán EXP dựa trên damage gây ra
                    int expGained = CalculateExp(damage);
                    GainExp(expGained);
                }
            }
        }

        private int CalculateExp(int damage)
        {
            int baseExp = damage / 2; // Lượng EXP cơ bản dựa trên damage
            return baseExp;
        }

        private void GainExp(int exp)
        {
            curExp += exp;
            expBar.SetValue(curExp);
            Vector2 posSpawn = transform.position + Vector3.up;
            GameObject textHit = Instantiate(Controller.instance.controlPrefabs.textHit, posSpawn, Quaternion.identity);
            TextHit scriptText = textHit.GetComponent<TextHit>();
            scriptText.SetText($"+{exp}", Color.green);

            // Kiểm tra nếu đủ EXP để lên cấp
            if (curExp >= GetExpToNextLevel())
            {
                LevelUp();
            }
        }

        private int GetExpToNextLevel()
        {
            return curLevel * 100; // Ví dụ: mỗi cấp độ cần 100 * cấp độ EXP
        }

        private void LevelUp()
        {
            curLevel += 1; // Tăng cấp độ
            curExp = 0; // Reset EXP sau khi lên cấp
            maxHp += 10; // Tăng máu tối đa
            maxMp += 5; // Tăng mana tối đa
            AttributeUpdate(1);

            // Hồi đầy máu và mana
            currentHp = maxHp;
            currentMp = maxMp;

            // Cập nhật giao diện
            healBar.SetMaxValue(maxHp);
            healBar.SetValue(currentHp);
            healBar.SetText($"{currentHp}/{maxHp}");

            mpBar.SetMaxValue(maxMp);
            mpBar.SetValue(currentMp);
            mpBar.SetText($"{currentMp}/{maxMp}");

            expBar.SetMaxValue(GetExpToNextLevel());
            expBar.SetValue(curExp);

            Controller.instance.controlCanvasUI.levelText.text = $"Level: {curLevel}";

            Debug.Log($"Level Up! Current Level: {curLevel}");
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
            AttributeUpdate(0);
            HealUpdate(0);
            DamageUpdate(0);
            CristicalUpdate(0);
        }
        public void AttributeUpdate(int count)
        {
            attributeCount += count;

            Controller.instance.controlCanvasUI.attributeCountText.text = $"{attributeCount}";
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