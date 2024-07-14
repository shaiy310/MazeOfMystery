using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    public Slider[] affectedSliders;
    public float[] diffRelations;
    public Slider cageSlider;
    public float cageRelation;

    Slider slider;
    float last_value;
    
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        last_value = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        last_value = slider.value;
    }

    public void OnValueChange(float value)
    {
        float affectedNewValue;
        float diff = value - last_value;

        for (int i = 0; i < affectedSliders.Length; i++) {
            affectedNewValue = affectedSliders[i].value + diff * diffRelations[i];
            // Check if one of the affected sliders is blocking the move
            if (affectedSliders[i].minValue > affectedNewValue || affectedNewValue > affectedSliders[i].maxValue) {
                slider.value = last_value;
                return;
            }
        }

        if (cageSlider != null) {
            affectedNewValue = cageSlider.value + diff * cageRelation;
            if (cageSlider.minValue > affectedNewValue || affectedNewValue > cageSlider.maxValue) {
                slider.value = last_value;
                return;
            }
        }

        
        last_value = value;
        for (int i = 0; i < affectedSliders.Length; i++) {
            affectedNewValue = affectedSliders[i].value + diff * diffRelations[i];
            affectedSliders[i].SetValueWithoutNotify(affectedNewValue);
        }
        if (cageSlider != null) {
            cageSlider.value += diff * cageRelation;
        }
    }
}
