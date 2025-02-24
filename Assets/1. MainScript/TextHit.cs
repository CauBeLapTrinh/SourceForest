using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHit : MonoBehaviour
{
    public float moveUp;
    public Text textUI;
    // Start is called before the first frame update
    void Start()
    {
        PlayEffect();
    }
    void OnDestroy()
    {
        DOTween.Kill(transform);
    }

    public void PlayEffect()
    {
        transform.DOMoveY(transform.position.y + moveUp, 1f).SetEase(Ease.Linear);
        textUI.DOFade(0.4f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
    public void SetText(string textSet)
    {
        textUI.text = textSet;
    }
}
