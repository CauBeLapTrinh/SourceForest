using Minigame.Forest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class MeleeRotate : MonoBehaviour
    {
        public float radius;
        public float rotationSpeed = 5f;
        int currentIndex = -1;
        // Start is called before the first frame update
        void Start()
        {
            if (radius < 1f)
            {
                radius = 1f;
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(new Vector3(0, 0, -1) * rotationSpeed * Time.deltaTime);
        }

        public void SetPosChild()
        {
            float angle = 360 / transform.childCount;
            int count = 1;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform posChild = transform.GetChild(i);

                Vector2 pos = posChild.localPosition;

                pos.x = Mathf.Cos((Mathf.PI / 180) * (angle * count)) * radius;
                pos.y = Mathf.Sin((Mathf.PI / 180) * (angle * count)) * radius;

                posChild.localRotation = Quaternion.Euler(0, 0, angle * count - 90f);

                posChild.localPosition = pos;

                count++;
            }
        }
        public void AddWeapon(GameObject weaponPrefab)
        {
            GameObject weaponObj = Instantiate(weaponPrefab, transform);
            MeleeWeaponControl meleeWeaponControl = weaponObj.GetComponent<MeleeWeaponControl>();

            if (meleeWeaponControl.indexWeapon != currentIndex && currentIndex != -1)
            {
                ClearWeapon();
            }
            currentIndex = meleeWeaponControl.indexWeapon;

            SetPosChild();
        }

        public void ClearWeapon()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
