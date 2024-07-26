using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class SpawnMonster : MonoBehaviour
    {
        public GameObject monsterPrefab;
        public float spawnDistance;

        Transform playerTransform;
        public float rateSpawn;
        float nextSpawn = 0;
        // Start is called before the first frame update
        void Start()
        {
            playerTransform = Controller.instance.player;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextSpawn)
            {
                Debug.Log("Spawn");

                SpawnMonsterAroundPlayer();

                nextSpawn = Time.time + rateSpawn;
            }
        }
        void SpawnMonsterAroundPlayer()
        {
            Vector3 playerPosition = playerTransform.position;

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPosition = playerPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * spawnDistance;

            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            monster.transform.parent = transform;
        }
    }
}
