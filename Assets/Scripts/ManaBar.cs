using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void setMaxMana(float mana) {
        slider.maxValue = mana;
        if (slider.value > mana) {
            slider.value = mana;
        }
        // slider.value = mana;

        fill.color = gradient.Evaluate(1f);
    }

    public void setMana(float mana) {
        slider.value = mana;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public float getMana() {
        return slider.value;
    }
}
