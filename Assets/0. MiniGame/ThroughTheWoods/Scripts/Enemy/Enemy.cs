using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace ThroughTheWoods
{
    public class Enemy : MonoBehaviour
    {
        public float maxHeal;
        float currentHeal;
        Animator animator;
        bool isDead = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            currentHeal = maxHeal;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Hit(float damage)
        {
            currentHeal -= damage;
            animator.SetTrigger("Hit");

            Vector2 posSpawn = transform.position + Vector3.up;
            GameObject textHit = Instantiate(Controller.instance.textHit, posSpawn, Quaternion.identity);

            TextHit scriptText = textHit.GetComponent<TextHit>();
            scriptText.SetText($"{damage}");

            if (currentHeal <= 0)
            {
                Dead();
            }
        }
        public void Dead()
        {
            isDead = true;
            animator.SetBool("Dead", isDead);
        }
    }
}

