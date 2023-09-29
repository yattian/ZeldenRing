using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar staminaBar;

        public void SetNewStamniaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
    }
}
