using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotateControl : MonoBehaviour
{
    PlayerControl playerControl;

    public float radius;
    public float rotationSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponentInParent<PlayerControl>();
        if (radius < 1f)
        {
            radius = 1f;
        }

        StartWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,-1) * rotationSpeed * Time.deltaTime);
    }
    public void StartWeapon()
    {
        if (StaticPropertis.indexWeapon.Count > 0)
        {
            for (int i = 0; i < StaticPropertis.indexWeapon.Count; i++)
            {
                GameObject weapon = Instantiate(Controller.instance.weapons[StaticPropertis.indexWeapon[i]], transform);
                weapon.transform.GetChild(0).gameObject.SetActive(false);
            }

            SetPosChild();
        }
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
}
