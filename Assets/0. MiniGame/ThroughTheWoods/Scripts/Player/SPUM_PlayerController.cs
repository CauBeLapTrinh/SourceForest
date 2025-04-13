using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ScriptBoy.DiggableTerrains2D_Demos;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
namespace ThroughTheWoods
{
    public class PlayerData
    {
        public int damageDefault;
        public int cristical;
        public int attributeCount = 0;
        public int curAttributeHeal = 0;
        public int curAttributeDamage = 0;
        public int curAttributeCristical = 0;
        public int curLevel;
        public int curExp;
        public float maxHp;
        public float currentHp;
        public float maxMp;
        public float currentMp;
        public Vector3 position;
    }
    public class SPUM_PlayerController : MonoBehaviour, IHealth
    {
        public Transform posAttack;
        PlayerData playerData = new();
        [Header("--- Properties ---")]
        [Header("Attack")]
        public int damageDefault;
        public int cristical;
        int attributeCount = 0;
        int curAttributeHeal = 0;
        int curAttributeDamage = 0;
        int curAttributeCristical = 0;
        public SpriteRenderer weaponSprite;
        SkillUI skillUI;
        int weaponIndex;
        [Header("Health")]
        public ProgressBar healBar;
        public float maxHp;
        float currentHp;
        float delayRecoveryHp = 1f;
        bool isDead = false;
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

            //SetStateAnimationIndex(PlayerState.ATTACK, 3);
            Controller.instance.SetSkillUI(0);
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
            if (Input.GetMouseButtonDown(0) && !Controller.instance.controlCanvasUI.IsOncanvas()) // 0 là nút chuột trái
            {
                // Lấy vị trí click chuột trong thế giới game
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Đảm bảo z = 0 để không ảnh hưởng đến 2D

                // Tính vector hướng từ nhân vật đến vị trí click chuột
                //Vector2 direction = (mousePosition - transform.position).normalized;

                if (!playerMovement.isAction)
                {
                    Attack(mousePosition);
                }
            }
            SaveCurrentData();
            RecoveryHp();
            RecoveryMp();
            LoadInfoUI();
        }
        public PlayerData GetPlayerData()
        {
            return playerData;
        }
        public void SaveCurrentData()
        {
            playerData.damageDefault = damageDefault;
            playerData.cristical = cristical;
            playerData.attributeCount = attributeCount;
            playerData.curAttributeHeal = curAttributeHeal;
            playerData.curAttributeDamage = curAttributeDamage;
            playerData.curAttributeCristical = curAttributeCristical;
            playerData.curLevel = curLevel;
            playerData.curExp = curExp;
            playerData.maxHp = maxHp;
            playerData.currentHp = currentHp;
            playerData.maxMp = maxMp;
            playerData.currentMp = currentMp;
            playerData.position = transform.position;
        }
        public void LoadData(PlayerData data)
        {
            damageDefault = data.damageDefault;
            cristical = data.cristical;
            attributeCount = data.attributeCount;
            curAttributeHeal = data.curAttributeHeal;
            curAttributeDamage = data.curAttributeDamage;
            curAttributeCristical = data.curAttributeCristical;
            curLevel = data.curLevel;
            curExp = data.curExp;
            maxHp = data.maxHp;
            currentHp = data.currentHp;
            maxMp = data.maxMp;
            currentMp = data.currentMp;
            transform.position = data.position;
            // Cập nhật giao diện
            LoadCharacterBar();
        }
        public void SetWeaponSprite(int indexWeapon, SkillUI skillUISet)
        {
            weaponIndex = indexWeapon;
            skillUI = skillUISet;
            SetStateAnimationIndex(PlayerState.ATTACK, indexWeapon);
            weaponSprite.sprite = skillUI.skillSprite;
        }

        Collider2D[] enemys;
        public void Attack(Vector3 mousePosition)
        {
            int damage = UnityEngine.Random.Range(damageDefault - 5, damageDefault + 6);

            bool isCritical = UnityEngine.Random.Range(0, 100) < cristical;
            if (isCritical)
            {
                damage *= 2;
            }

            switch (weaponIndex)
            {
                case 0:
                    StartCoroutine(Attack1(damage, isCritical, mousePosition));
                    break;
                case 3:
                    StartCoroutine(Attack2(damage, isCritical, mousePosition));
                    break;
                default:
                    break;
            }
        }
        public IEnumerator Attack1(int damage, bool isCristical, Vector3 mousePosition)
        {
            if (currentMp < 10 || skillUI.IsDelayActive())
            {
                yield break;
            }

            skillUI.StartDelay(0.5f);
            currentMp -= 10;
            mpBar.SetValue(currentMp);
            mpBar.SetText($"{currentMp}/{maxMp}");
            Vector2 direction = (mousePosition - transform.position).normalized;
            playerMovement.FaceControl(direction.x);
            playerMovement.isAction = true;
            playerMovement.StopMovement();
            PlayStateAnimation(PlayerState.ATTACK);
            enemys = Physics2D.OverlapBoxAll(posAttack.position, Vector2.one, 0, Controller.instance.enemyLayer);
            yield return new WaitForSeconds(0.2f);
            HitEnemy(damage, isCristical);
            yield return new WaitForSeconds(0.2f);
            EndActionAttack();
        }
        public IEnumerator Attack2(int damage, bool isCristical, Vector3 mousePosition)
        {
            if (currentMp < 15 || skillUI.IsDelayActive())
            {
                yield break;
            }

            skillUI.StartDelay(0.8f);
            currentMp -= 15;
            mpBar.SetValue(currentMp);
            mpBar.SetText($"{currentMp}/{maxMp}");
            Vector2 direction = (mousePosition - transform.position).normalized;
            playerMovement.FaceControl(direction.x);
            playerMovement.isAction = true;
            playerMovement.StopMovement();
            PlayStateAnimation(PlayerState.ATTACK);
            yield return new WaitForSeconds(0.3f);
            CreateArrow(damage, isCristical, mousePosition);
            yield return new WaitForSeconds(0.2f);
            EndActionAttack();
        }
        public void CreateArrow(int damage, bool isCristical, Vector3 mousePosition)
        {
            Vector2 direction = (mousePosition - posAttack.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            GameObject arrow = Instantiate(Controller.instance.controlPrefabs.arrowPrefab, posAttack.position, Quaternion.Euler(0, 0, angle));
            Physics2D.IgnoreCollision(arrow.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            arrowScript.SetDamage(damage, isCristical);
            arrowScript.SetRootPlayer(this);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * 12f, ForceMode2D.Impulse);
        }

        public void EndActionAttack()
        {
            playerMovement.isAction = false;
        }
        public void HitEnemy(int damage, bool isCristical)
        {
            if (enemys.Length > 0)
            {
                foreach (var enemy in enemys)
                {
                    IEnemyTarget enemyI = enemy.GetComponent<IEnemyTarget>();
                    enemyI.SetTargetFollow(transform);
                    Health health = enemy.GetComponent<Health>();
                    if (health.IsDead()) break;
                    health.TakeDamage(damage, isCristical);

                    CalculateDamage(damage);
                }
            }
        }

        private int CalculateExp(int damage)
        {
            int baseExp = damage / 2; // Lượng EXP cơ bản dựa trên damage
            return baseExp;
        }
        public void CalculateDamage(int damage)
        {
            int expGained = CalculateExp(damage);
            GainExp(expGained);
        }

        public void GainExp(int expGained)
        {
            curExp += expGained;
            expBar.SetValue(curExp);
            Vector2 posSpawn = transform.position + Vector3.up;
            GameObject textHit = Instantiate(Controller.instance.controlPrefabs.textHit, posSpawn, Quaternion.identity);
            TextHit scriptText = textHit.GetComponent<TextHit>();
            scriptText.SetText($"+{expGained}", Color.green);

            // Kiểm tra nếu đủ EXP để lên cấp
            if (curExp >= GetExpToNextLevel())
            {
                LevelUp();
            }
        }
        public void Healing(int amount)
        {
            currentHp = Mathf.Min(currentHp + amount, maxHp);
            healBar.SetValue(currentHp);
            healBar.SetText($"{currentHp}/{maxHp}");
            Vector2 posSpawn = transform.position + Vector3.up;
            GameObject textHp = Instantiate(Controller.instance.controlPrefabs.textHit, posSpawn, Quaternion.identity);
            TextHit scriptText = textHp.GetComponent<TextHit>();
            scriptText.SetText($"+{amount}", Color.red);
        }

        public void RestoreMana(int amount)
        {
            currentMp = Mathf.Min(currentMp + amount, maxMp);
            mpBar.SetValue(currentMp);
            mpBar.SetText($"{currentMp}/{maxMp}");
            Vector2 posSpawn = transform.position + Vector3.up;
            GameObject textMp = Instantiate(Controller.instance.controlPrefabs.textHit, posSpawn, Quaternion.identity);
            TextHit scriptText = textMp.GetComponent<TextHit>();
            scriptText.SetText($"+{amount}", Color.blue);
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
            damageDefault += 3; // Tăng sát thương cơ bản
            cristical += 1; // Tăng tỉ lệ chí mạng
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
        public bool IsDead()
        {
            return isDead;
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
            if (currentHp == 0)
            {
                currentHp = maxHp;
            }
            healBar.SetMaxValue(maxHp);
            healBar.SetValue(currentHp);
            healBar.SetText($"{currentHp}/{maxHp}");

            if (currentMp == 0)
            {
                currentMp = maxMp;
            }
            mpBar.SetMaxValue(maxMp);
            mpBar.SetValue(currentMp);
            mpBar.SetText($"{currentMp}/{maxMp}");

            expBar.SetMaxValue(GetExpToNextLevel());
            expBar.SetValue(curExp);

            Controller.instance.controlCanvasUI.levelText.text = $"Level: {curLevel}";

            Controller.instance.controlCanvasUI.UpdateAttribute(EAttribute.Health, attributeCount, curAttributeHeal);
            Controller.instance.controlCanvasUI.UpdateAttribute(EAttribute.Damage, attributeCount, curAttributeDamage);
            Controller.instance.controlCanvasUI.UpdateAttribute(EAttribute.Defend, attributeCount, curAttributeCristical);
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
            healBar.SetMaxValue(maxHp);
            healBar.SetValue(currentHp);
            healBar.SetText($"{currentHp}/{maxHp}");
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
            Controller.instance.controlCanvasUI.UpdateAttribute(EAttribute.Health, attributeCount, curAttributeHeal);
            HealUpdate(10);
        }
        public void UpDamageAttribute()
        {
            if (attributeCount == 0) return;

            attributeCount -= 1;
            curAttributeDamage += 1;
            Controller.instance.controlCanvasUI.UpdateAttribute(EAttribute.Damage, attributeCount, curAttributeDamage);
            DamageUpdate(5);
        }
        public void UpCristicalAttribute()
        {
            if (attributeCount == 0) return;

            attributeCount -= 1;
            curAttributeCristical += 1;
            Controller.instance.controlCanvasUI.UpdateAttribute(EAttribute.Defend, attributeCount, curAttributeCristical);
            CristicalUpdate(3);
        }
        void OnDrawGizmosSelected()
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