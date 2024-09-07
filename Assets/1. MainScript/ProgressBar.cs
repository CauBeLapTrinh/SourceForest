using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text txtSet;

    public void SetMaxValue(float valueSet)
    {
        slider.maxValue = valueSet;
        slider.value = valueSet;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(float valueSet)
    {
        slider.value = valueSet;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetText(string textSet)
    {
        txtSet.text = textSet;
    }
}

