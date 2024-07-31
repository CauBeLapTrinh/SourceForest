using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class SpawnControl : MonoBehaviour
    {
        Transform playerTransform;
        [Header("--------- Spawn Monster ---------")]
        public GameObject monsterPrefab;
        public float spawnDistanceMonster;
        public float rateSpawnMonster;
        float nextSpawnMonster = 0;

        [Header("--------- Spawn BoxItem ---------")]
        public GameObject boxItemPrefab;
        public float spawnDistanceBox;
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
                SpawnMonsterAroundPlayer();

                nextSpawnMonster = Time.time + rateSpawnMonster;
            }
            
            if (Time.time > nextSpawnBox)
            {
                SpawnBoxItemAroundPlayer();

                nextSpawnBox = Time.time + rateSpawnBox;
            }
        }
        void SpawnMonsterAroundPlayer()
        {
            Vector3 playerPosition = playerTransform.position;

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPosition = playerPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * spawnDistanceMonster;

            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            monster.transform.parent = transform;
        }
        void SpawnBoxItemAroundPlayer()
        {
            Vector3 playerPosition = playerTransform.position;

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPosition = playerPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * spawnDistanceBox;

            GameObject boxItem = Instantiate(boxItemPrefab, spawnPosition, Quaternion.identity);
            boxItem.transform.parent = transform;
        }
    }
}
