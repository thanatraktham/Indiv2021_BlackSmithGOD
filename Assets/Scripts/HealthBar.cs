using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text healthText;

    public void setMaxHealth(float health) {
        slider.maxValue = health;
        // slider.value = health;
        if (slider.value > health) {
            slider.value = health;
        }

        fill.color = gradient.Evaluate(1f);
    }

    public void setHealth(float health) {
        health = (int) health;
        slider.value = (int) health;

        fill.color = gradient.Evaluate(slider.normalizedValue);

        healthText.text = health.ToString();
    }
}
