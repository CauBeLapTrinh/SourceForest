using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheWoods
{
    public interface IEnemyTarget
    {
        void SetTargetFollow(Transform targetSet);
    }
    [Serializable]
    public class ItemDrop
    {
        public GameObject[] itemPrefab;
        public int dropRate;
    }
    public class SPUM_Enemy : MonoBehaviour, IHealth, IEnemyTarget
    {
        [Header("---- Item drop ----")]
        public ItemDrop[] itemDrops;

        [Header("---- Properties ----")]
        public MonsterType monsterType;
        public float maxHeal;
        public GameObject infoCanvas;
        public ProgressBar healthBar;
        float timeShowHealthBar = 0f;
        float currentHeal;
        //Animator animator;
        bool isDead = false;
        float timeRevive = 5f;
        [Header("Attack")]
        public int damageDefault;
        public float rangeAttack;
        public float rangeFollow = 3f;
        [HideInInspector] public bool isAttack = false;
        [Header("AI Movement")]
        public float movement;
        public float speed;
        public float movementRange = 3;
        float idleTime = 2f;
        bool isFacingRight = false;
        Vector2 limitRangeX;
        Vector2 limitRangeY;
        Vector3 targetPosition;
        [HideInInspector] public Transform targetFollow = null;
        [Header("SPUM")]
        public SPUM_Prefabs _prefabs;
        private PlayerState _currentState;
        public bool isAction = false;
        public Dictionary<PlayerState, int> IndexPair = new();
        public virtual void Start()
        {
            if (_prefabs == null)
            {
                _prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
                if (!_prefabs.allListsHaveItemsExist())
                {
                    _prefabs.PopulateAnimationLists();
                }
            }
            _prefabs.OverrideControllerInit();
            foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
            {
                IndexPair[state] = 0;
            }

            currentHeal = maxHeal;
            limitRangeX = new Vector2(transform.position.x - movementRange, transform.position.x + movementRange);
            limitRangeY = new Vector2(transform.position.y - movementRange, transform.position.y + movementRange);
            RandomTagetPos();

            healthBar.SetMaxValue(maxHeal);
            healthBar.SetValue(maxHeal);
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
        public virtual void Update()
        {
            if (healthBar.gameObject.activeSelf)
            {
                timeShowHealthBar -= Time.deltaTime;
                if (timeShowHealthBar <= 0)
                {
                    healthBar.gameObject.SetActive(false);
                }
            }
        }

        public virtual void FixedUpdate()
        {
            if (!isDead)
            {
                Movement();
            }
            else
            {
                timeRevive -= Time.fixedDeltaTime;
                if (timeRevive <= 0)
                {
                    Revive();
                }
            }

            if (isAction) return;

            if (movement < 0.1f)
            {
                _currentState = PlayerState.IDLE;
            }
            else
            {
                _currentState = PlayerState.MOVE;
            }

            switch (_currentState)
            {
                case PlayerState.IDLE:
                    //StopMovement();
                    break;
                case PlayerState.MOVE:
                    //Movement();
                    break;
            }
            PlayStateAnimation(_currentState);
        }
        public void Movement()
        {
            if (targetFollow != null)
            {
                float distanceTarget = Vector2.Distance(transform.position, targetFollow.position);

                if (distanceTarget < rangeFollow && RangeCheck())
                {
                    FollowTarget(distanceTarget);
                }
                else
                {
                    isAttack = false;
                    targetFollow = null;
                    idleTime = 0;
                    RandomTagetPos();
                }
            }
            else
            {
                idleTime -= Time.fixedDeltaTime;
                if (idleTime <= 0)
                {
                    AutoMovement();
                }
            }
        }
        public void FollowTarget(float distance)
        {
            FaceCheck(targetFollow.position);

            if (distance > rangeAttack)
            {
                movement = 1;
                transform.position = Vector3.MoveTowards(transform.position, targetFollow.position, speed * Time.fixedDeltaTime);
            }
            else
            {
                movement = 0;
                isAttack = true;
            }
        }
        public void AutoMovement()
        {
            FaceCheck(targetPosition);
            movement = 1;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                idleTime = UnityEngine.Random.Range(3f, 4f);
                movement = 0;
                RandomTagetPos();
            }
        }
        public void FaceCheck(Vector2 target)
        {
            if (target.x < transform.position.x && isFacingRight)
            {
                FaceSet(-1);
            }
            else if (target.x > transform.position.x && !isFacingRight)
            {
                FaceSet(1);
            }
        }
        public bool RangeCheck()
        {
            if (transform.position.x > limitRangeX.x && transform.position.x < limitRangeX.y)
            {
                if (transform.position.y > limitRangeY.x && transform.position.y < limitRangeY.y)
                {
                    return true;
                }
            }
            return false;
        }
        public void RandomTagetPos()
        {
            float rdX = UnityEngine.Random.Range(limitRangeX.x, limitRangeX.y);
            float rdY = UnityEngine.Random.Range(limitRangeY.x, limitRangeY.y);
            targetPosition = new Vector2(rdX, rdY);
            //Debug.Log(targetPosition);
        }
        public void FaceSet(float dir)
        {
            if (dir == -1)
            {
                isFacingRight = false;
            }
            else
            {
                isFacingRight = true;
            }
            Vector3 theScale = transform.localScale;
            theScale.x = dir;
            transform.localScale = theScale;

            Vector3 theScaleInfo = infoCanvas.transform.localScale;
            theScaleInfo.x = dir;
            infoCanvas.transform.localScale = theScaleInfo;
        }
        public void SetTargetFollow(Transform targetSet)
        {
            targetFollow = targetSet;
        }
        public void TakeDamage(float damage, bool isCristical)
        {
            if (isDead) return;

            currentHeal -= damage;
            healthBar.SetValue(currentHeal);
            timeShowHealthBar = 4f;
            if (!healthBar.gameObject.activeSelf)
            {
                healthBar.gameObject.SetActive(true);
            }
            Instantiate(Controller.instance.controlPrefabs.bloodVfx, transform.position + Vector3.up, Quaternion.identity);
            //animator.SetTrigger("Hit");
            PlayStateAnimation(PlayerState.DAMAGED);
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

            if (currentHeal <= 0)
            {
                Dead();
            }
        }
        public bool IsDead()
        {
            return isDead;
        }
        public void Dead()
        {
            isAttack = false;
            isDead = true;
            PlayStateAnimation(PlayerState.DEATH);
            DropItems();
            Controller.instance.missionManager.OnMonsterKilled(monsterType);
            timeRevive = 10f;
        }
        private void DropItems()
        {
            if (itemDrops.Length == 0) return; // Không có item nào để rơi
            int cumulativeRate = 0; // Tỷ lệ tích lũy
            List<(int min, int max, ItemDrop itemDrop)> dropRanges = new();

            // Tính toán phạm vi tỷ lệ cho từng ItemDrop
            foreach (var itemDrop in itemDrops)
            {
                int min = cumulativeRate + 1;
                int max = cumulativeRate + itemDrop.dropRate;
                cumulativeRate = max;

                dropRanges.Add((min, max, itemDrop));
            }

            // Random một số trong khoảng từ 1 đến tổng tỷ lệ
            int randomChance = UnityEngine.Random.Range(1, 101);

            // Tìm ItemDrop tương ứng với số random
            foreach (var range in dropRanges)
            {
                if (randomChance >= range.min && randomChance <= range.max)
                {
                    // Chọn ngẫu nhiên một itemPrefab từ ItemDrop đã chọn
                    if (range.itemDrop.itemPrefab.Length > 0)
                    {
                        GameObject itemToDrop = range.itemDrop.itemPrefab[UnityEngine.Random.Range(0, range.itemDrop.itemPrefab.Length)];

                        // Tạo item tại vị trí của quái vật
                        Instantiate(itemToDrop, transform.position, Quaternion.identity);
                    }
                    return; // Chỉ rơi một loại item, thoát khỏi hàm
                }
            }
        }
        public void Revive()
        {
            isDead = false;
            currentHeal = maxHeal;
            _prefabs._anim.SetTrigger("Revive");
            Instantiate(Controller.instance.controlPrefabs.collectPrefab, transform.position, Quaternion.identity);
            RandomTagetPos();
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if (!Application.isPlaying)
            {
                limitRangeX = new Vector2(transform.position.x - movementRange, transform.position.x + movementRange);
                limitRangeY = new Vector2(transform.position.y - movementRange, transform.position.y + movementRange);
            }

            Gizmos.DrawLine(new Vector3(limitRangeX.x, limitRangeY.y), new Vector3(limitRangeX.y, limitRangeY.y));
            Gizmos.DrawLine(new Vector3(limitRangeX.x, limitRangeY.x), new Vector3(limitRangeX.y, limitRangeY.x));
            Gizmos.DrawLine(new Vector3(limitRangeX.y, limitRangeY.x), new Vector3(limitRangeX.y, limitRangeY.y));
            Gizmos.DrawLine(new Vector3(limitRangeX.x, limitRangeY.x), new Vector3(limitRangeX.x, limitRangeY.y));
        }
    }
}