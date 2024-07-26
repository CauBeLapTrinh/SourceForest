using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Minigame.MonsterRush
{
    public class BoxControl : MonoBehaviour
    {
        public float timeOpening;
        public ProgressBar progressBar;
        Animator animator;
        bool isOpen = false;

        bool isOpening = false;
        int percentProgress = 0;

        float nextPercent = 0;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isOpening && !isOpen)
            {
                if (Time.time > nextPercent)
                {
                    percentProgress += 1;

                    progressBar.SetValue(percentProgress);
                    progressBar.SetText($"{percentProgress}%");

                    if (percentProgress == 100)
                    {
                        OpenBox();

                        return;
                    }

                    nextPercent = Time.time + timeOpening / 100;
                }
            }
        }

        public bool IsOpen()
        {
            return isOpen;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (transform.GetChild(0).gameObject.activeSelf == false)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }

                isOpening = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isOpening = false;
            }
        }
        public void OpenBox()
        {
            isOpen = true;

            transform.GetChild(0).gameObject.SetActive(false);

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

        IEnumerator DestroyBox()
        {
            yield return new WaitForSeconds(1.5f);

            Destroy(gameObject);
        }
    }
}

