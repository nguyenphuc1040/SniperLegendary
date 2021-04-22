using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class percentSetting : MonoBehaviour
{
    public Text percent;
    public Slider slider;
    void Start()
    {
        
    }

    void Update()
    {
        percent.text = slider.value+"%";
    }
}
