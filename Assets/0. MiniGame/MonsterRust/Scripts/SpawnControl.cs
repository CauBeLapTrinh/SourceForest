using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class SpawnControl : MonoBehaviour
    {
        Transform playerTransform;
        [Header("--------- Spawn Monster ---------")]
        public GameObject animSpawnMonsterPrefab;
        public GameObject monsterPrefab;
        public float distanceSpawnMonster;
        public float rateSpawnMonster;
        public int amountSpawnMonster;
        float nextSpawnMonster = 0;

        [Header("--------- Spawn BoxItem ---------")]
        public GameObject boxItemPrefab;
        public float distanceSpawnBox;
        public float rateSpawnBox;
        float nextSpawnBox = 0;
        // Start is called before the first frame update
        void Start()
        {
            playerTransform = Controller.instance.player;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextSpawnMonster)
            {
                StartCoroutine(SpawnMonsterAroundPlayer());

                nextSpawnMonster = Time.time + rateSpawnMonster;
            }

            if (Time.time > nextSpawnBox)
            {
                SpawnBoxItemAroundPlayer();

                nextSpawnBox = Time.time + rateSpawnBox;
            }
        }
        public void SetRush(bool setValue)
        {
            if (setValue)
            {
                rateSpawnMonster /= 2;
                amountSpawnMonster = 5;
            }
            else
            {
                rateSpawnMonster *= 2;
                amountSpawnMonster = 3;
            }
        }
        IEnumerator SpawnMonsterAroundPlayer()
        {
            int rdSpawn = Random.Range(1, amountSpawnMonster);

            for (int i = 0; i < rdSpawn; i++)
            {
                StartCoroutine(IESpawnMonster());
                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            }
        }
        IEnumerator IESpawnMonster()
        {
            Vector3 playerPosition = playerTransform.position;

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPosition = playerPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * distanceSpawnMonster;

            Instantiate(animSpawnMonsterPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(1f);

            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            monster.transform.parent = transform;
        }
        void SpawnBoxItemAroundPlayer()
        {
            Vector3 playerPosition = playerTransform.position;

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPosition = playerPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * distanceSpawnBox;

            GameObject boxItem = Instantiate(boxItemPrefab, spawnPosition, Quaternion.identity);
            boxItem.transform.parent = transform;
        }
    }
}
