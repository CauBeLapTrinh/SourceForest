using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughTheWoods
{
    public enum EAttribute
    {
        Health,
        Damage,
        Defend
    }
    public class ControlCanvasUI : MonoBehaviour
    {
        bool isOncanvas = false;
        public bool isLoading = false;
        [Header("--- PanelUI ---")]
        public GameObject canvasLoading;
        public ProgressBar progressLoading;
        float currentLoading = 0;
        public GameObject[] panelIndex;
        [Header("--- InfoUI ---")]
        public Text healthText;
        public Text damageText;
        public Text cristicalText;
        [Header("- Attribute")]
        public Text attributeCountText;
        public Text attributeHealthText;
        public Text attributeDamageText;
        public Text attributeCristicalText;
        [Header("- CharacterBar")]
        public Text levelText;
        [Header("--- Skill ---")]
        public List<Image> skillImages;
        void Start()
        {
            if (isLoading)
            {
                canvasLoading.SetActive(true);
                progressLoading.SetMaxValue(100);
                progressLoading.SetValue(0);
                progressLoading.SetText(progressLoading.GetValue().ToString() + "%");
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!isOncanvas)
                {
                    OnPanelUI(1);
                }
                else
                {
                    OffPanelUI(1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && !isOncanvas)
            {
                OnPanelUI(0);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isOncanvas)
            {
                OffAllPanelUI();
            }
        }
        void FixedUpdate()
        {
            if (isLoading)
            {
                LoadingUI();
            }
        }
        public void LoadingUI()
        {
            if (currentLoading < 100)
            {
                // Tăng giá trị loading một cách ngẫu nhiên để tạo cảm giác sinh động
                float increment = Random.Range(1f, 2f); // Tăng từ 1 đến 5 mỗi lần
                currentLoading = Mathf.Min(currentLoading + increment, 100); // Đảm bảo không vượt quá 100

                // Cập nhật thanh progress và text
                progressLoading.SetValue(currentLoading);
                progressLoading.SetText($"{Mathf.FloorToInt(currentLoading)}%");

                // Tạm dừng một chút để tạo hiệu ứng
                if (currentLoading < 100)
                {
                    StartCoroutine(WaitAndContinueLoading());
                }
                else
                {
                    // Khi loading hoàn tất
                    canvasLoading.SetActive(false);
                    Debug.Log("Loading complete!");
                }
            }
        }

        private IEnumerator WaitAndContinueLoading()
        {
            float waitTime = Random.Range(0.4f, 0.6f); // Thời gian chờ ngẫu nhiên từ 0.1 đến 0.3 giây
            yield return new WaitForSeconds(waitTime);

            // Tiếp tục cập nhật loading
            LoadingUI();
        }
        public bool IsOncanvas()
        {
            return isOncanvas;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void OnPanelUI(int index)
        {
            if (isOncanvas) return;

            isOncanvas = true;
            panelIndex[index].SetActive(true);

            if (index == 0)
            {
                Time.timeScale = 0;
            }
        }
        public void OffPanelUI(int index)
        {
            isOncanvas = false;
            panelIndex[index].SetActive(false);

            if (index == 0)
            {
                Time.timeScale = 1;
            }
        }
        public void OffAllPanelUI()
        {
            isOncanvas = false;
            foreach (var panel in panelIndex)
            {
                panel.SetActive(false);
            }
            Time.timeScale = 1;
        }
        public void UpdateAttribute(EAttribute attributeSet, int attributeCount, int valueSet)
        {
            attributeCountText.text = $"{attributeCount}";

            switch (attributeSet)
            {
                case EAttribute.Health:
                    attributeHealthText.text = $"{valueSet}";
                    break;
                case EAttribute.Damage:
                    attributeDamageText.text = $"{valueSet}";
                    break;
                case EAttribute.Defend:
                    attributeCristicalText.text = $"{valueSet}";
                    break;
                default:
                    break;
            }
        }
        public void ResetSkillUI()
        {
            foreach (var img in skillImages)
            {
                Color currentColor = img.color;

                // Thay đổi alpha thành 0 (trong suốt)
                currentColor.a = 0f;

                // Gán lại màu mới cho Image
                img.color = currentColor;
            }
        }
    }
}

