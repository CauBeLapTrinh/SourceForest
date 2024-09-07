using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class ExpControl : MonoBehaviour
    {
        public float expPlus;
        Transform targetMove = null;
        bool isSet = false;

        // Update is called once per frame
        void Update()
        {
            if (targetMove != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetMove.position, 4 * Time.deltaTime);

                if (Vector2.Distance(transform.position, targetMove.position) < 0.1f)
                {
                    PlayerControl playerControl = targetMove.GetComponent<PlayerControl>();

                    playerControl.TakeExp(expPlus);

                    Destroy(gameObject);
                }
            }
        }

        public void SetTarget(Transform target)
        {
            if (!isSet)
            {
                isSet = true;
                targetMove = target;
            }
        }
    }

}
