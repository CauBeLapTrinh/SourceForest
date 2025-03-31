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
        [Header("--- PauseUI ---")]
        public GameObject panelPause;
        [Header("--- InfoUI ---")]
        public GameObject panelInfo;
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
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void OnPanelUI(int index)
        {
            switch (index)
            {
                case 0:
                    panelPause.SetActive(true);
                    Time.timeScale = 0;
                    break;
                case 1:
                    panelInfo.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        public void OffPanelUI(int index)
        {
            switch (index)
            {
                case 0:
                    panelPause.SetActive(false);
                    Time.timeScale = 1;
                    break;
                case 1:
                    panelInfo.SetActive(false);
                    break;
                default:
                    break;
            }
        }
        public void PlusAttribute(EAttribute attributeSet, int attributeCount, int valueSet)
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
    }
}

