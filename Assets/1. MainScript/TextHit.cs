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
        DOTween.Kill(transform); // Hủy tất cả các hiệu ứng liên quan đến transform
        DOTween.Kill(textUI);
    }

    public void PlayEffect()
    {
        // Di chuyển theo đường cong
        float randomX1 = Random.Range(-1f, 1f); // Giá trị ngẫu nhiên cho điểm giữa
        float randomX2 = Random.Range(-0.8f, 0.8f); // Giá trị ngẫu nhiên cho điểm cuối
        float randomY = Random.Range(moveUp / 2, moveUp * 2); // Giá trị ngẫu nhiên cho chiều cao

        Vector3[] path = new Vector3[]
        {
        transform.position,
        transform.position + new Vector3(randomX1, randomY / 2, 0),
        transform.position + new Vector3(randomX2, randomY, 0)
        };

        // Di chuyển theo đường cong ngẫu nhiên
        transform.DOPath(path, 1f, PathType.CatmullRom).SetEase(Ease.OutQuad);

        // Hiệu ứng phóng to rồi thu nhỏ
        transform.DOScale(Vector3.one * 1.5f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.InOutQuad);
        });

        // Xoay nhẹ
        transform.DORotate(new Vector3(0, 0, Random.Range(-30, 30)), 0.5f, RotateMode.LocalAxisAdd).SetLoops(2, LoopType.Yoyo);

        // Làm mờ dần Text
        textUI.DOFade(0.4f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
    public void SetText(string textSet, Color colorSet = default)
    {
        if (colorSet == default)
        {
            colorSet = Color.white;
        }
        textUI.color = colorSet;
        textUI.text = textSet;
    }
}
