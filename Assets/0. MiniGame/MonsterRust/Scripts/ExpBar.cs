using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.MonsterRush
{
    public class ExpBar : MonoBehaviour
    {
        public Text txtLevel;

        ProgressBar progressBar;

        int currentLevel = 1;
        float currentExp = 0;
        float nextExp = 4;


        // Start is called before the first frame update
        void Start()
        {
            progressBar = GetComponent<ProgressBar>();
            progressBar.SetMaxValue(nextExp);
            progressBar.SetValue(currentExp);
            progressBar.SetText($"{currentExp}/{nextExp}");
        }

        public void TakeExperience(float exp)
        {
            currentExp += exp;

            progressBar.SetValue(currentExp);
            progressBar.SetText($"{currentExp}/{nextExp}");

            if (currentExp >= nextExp)
            {
                LevelUp();
            }
        }
        public void LevelUp()
        {
            currentLevel += 1;

            txtLevel.text = $"Level: {currentLevel}";

            currentExp = 0;
            nextExp += nextExp * 0.5f;
            nextExp = Mathf.RoundToInt(nextExp);

            progressBar.SetMaxValue(nextExp);
            progressBar.SetValue(currentExp);
            progressBar.SetText($"{currentExp}/{nextExp}");
        }
    }

}
