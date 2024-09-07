using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class GunControl : MonoBehaviour
    {
        [Header("--------- Properties ---------")]
        public int levelGun;
        public float damage;
        public float speedBullet;
        public float recoil;
        public GameObject bulletPrefabs;
        public Transform pointGun;

        public Sprite gunImg;
        public void SetGun(GunControl gunSet)
        {
            levelGun = gunSet.levelGun;
            damage = gunSet.damage;
            speedBullet = gunSet.speedBullet;
            recoil = gunSet.recoil;
            bulletPrefabs = gunSet.bulletPrefabs;
        }

        public void Shoot(Transform enemyNearest)
        {
            if (levelGun == 1)
            {
                TommyGunShoot(enemyNearest);
            }
            else if (levelGun == 2)
            {
                StartCoroutine(MachineGunShoot(enemyNearest));
            }
            else
            {
                ShotGunShoot(enemyNearest);
            }
            
        }

        public void TommyGunShoot(Transform enemyNearest)
        {
            Vector3 eulerBullet = enemyNearest.transform.position - pointGun.position;

            CreateBullet(eulerBullet);
        }
        public IEnumerator MachineGunShoot(Transform enemyNearest)
        {
            Vector3 eulerBullet = enemyNearest.transform.position - pointGun.position;

            for (int i = 0; i < 2; i++)
            {
                CreateBullet(eulerBullet);

                yield return new WaitForSeconds(0.1f);
            }
        }
        public void ShotGunShoot(Transform enemyNearest)
        {
            Vector3 eulerBullet = enemyNearest.transform.position - pointGun.position;

            for (int i = 0; i < 3; i++)
            {
                CreateBullet(eulerBullet);
            }
        }

        public void CreateBullet(Vector3 eulerAngle)
        {
            float zAxis = Mathf.Atan2(eulerAngle.x, eulerAngle.y) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, -zAxis + Random.Range(-recoil, recoil));

            GameObject bullet = Instantiate(bulletPrefabs, pointGun.position, rotation);

            //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
            Bullet sciptBullet = bullet.GetComponent<Bullet>();
            sciptBullet.SetDamage(damage);

            float angle = (bullet.transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;

            Vector2 vectorF = new(Mathf.Cos(angle), Mathf.Sin(angle));
            sciptBullet.Shoot(vectorF, speedBullet);
        }
    }
}
