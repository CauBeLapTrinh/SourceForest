using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Forest
{

public class Weapon : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            enemy.AddDamage(damage);
        }
    }

    public void PlayAnimIdle()
    {
        transform.DOMoveY(transform.position.y + 0.2f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopDotWeen()
    {
        transform.DOKill();
    }
}
}
