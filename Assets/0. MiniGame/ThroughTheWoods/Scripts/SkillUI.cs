using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughTheWoods
{
    public class SkillUI : MonoBehaviour
    {
        public int skillIndex;
        public Sprite skillSprite;
        public Image delayImg;
        Image highlightImage;
        private bool isDelayActive = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            highlightImage = GetComponent<Image>();
        }

        public void OnClickSkillButton()
        {
            Controller.instance.controlCanvasUI.ResetSkillUI();
            Controller.instance.SetWeaponPlayer(skillIndex, this);

            Color currentColor = highlightImage.color;

            // Thay đổi alpha thành 0 (trong suốt)
            currentColor.a = 1f;

            // Gán lại màu mới cho Image
            highlightImage.color = currentColor;
        }
        public void StartDelay(float delayTime)
        {
            isDelayActive = true; // Bật trạng thái delay
            delayImg.fillAmount = 1; // Đặt hình ảnh delay đầy

            // Chạy hiệu ứng giảm dần fillAmount
            delayImg.fillAmount = 1;
            delayImg.DOFillAmount(0, delayTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                isDelayActive = false; // Kết thúc delay
            });
        }

        public bool IsDelayActive()
        {
            return isDelayActive;
        }
    }
}