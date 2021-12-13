using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void setMaxStamina(float stamina) {
        slider.maxValue = stamina;
        if (slider.value > stamina) {
            slider.value = stamina;
        }
        // slider.value = stamina;

        fill.color = gradient.Evaluate(1f);
        // Debug.Log("Max Stamina Set To Be : " + stamina);
    }

    public float getMaxStamina() {
        return slider.maxValue;
    }

    public void setStamina(float stamina) {
        slider.value = stamina;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public float getStamina() {
        return slider.value;
    }

    //Getter/Setter
    
}
