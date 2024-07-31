using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class GunControl : MonoBehaviour
    {
        [Header("--------- Properties ---------")]
        public float damage;
        public float speedBullet;
        public GameObject bulletPrefabs;
        public Transform pointGun;

        public Sprite gunImg;

        public void Shoot(Transform enemyNearest)
        {
            Vector3 eulerBullet = enemyNearest.transform.position - pointGun.position;

            CreateBullet(eulerBullet);
        }

        public void CreateBullet(Vector3 eulerAngle)
        {
            float zAxis = Mathf.Atan2(eulerAngle.x, eulerAngle.y) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, -zAxis);

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
