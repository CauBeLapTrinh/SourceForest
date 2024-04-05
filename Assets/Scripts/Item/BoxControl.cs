using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    Animator animator;
    bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsOpen()
    { 
        return isOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.transform.CompareTag("Weapon") 
            || collision.transform.CompareTag("Arrow")) && !isOpen)
        {
            isOpen = true;
            animator.SetTrigger("Open");

            StartCoroutine(DestroyBox());

            GameObject itemDrop = Instantiate(Controller.instance.weapons[Random.Range(0, Controller.instance.weapons.Count)], 
                transform.position, Quaternion.identity);
            
            Weapon weapon = itemDrop.GetComponent<Weapon>();

            if (weapon != null)
            {
                weapon.PlayAnimIdle();
            }
        }
    }

    IEnumerator DestroyBox()
    {
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }
}
