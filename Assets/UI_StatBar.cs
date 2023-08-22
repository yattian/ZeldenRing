using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YT
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        // Variable to scale bar size depending on stat (Higher stat = longer bar across screen)
        // Secondary bar behind for poslish effect

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
}

