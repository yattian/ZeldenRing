using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class WeaponItem : Item
    {
        // Animator controller override (Charge attack animations based on weapon you are currently using)

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strengthREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int holyDamage = 0;
        public int lightningDamage = 0;

        // Weapon guard absorptions 

        [Header("Weapon Poise")]
        public float poiseDamage = 10;
        // Offensive poise bonus when attacking

        // Weapon Modifiers
        [Header("Attack Modifiers")]
        public float light_Attack_01_Modifier = 1.0f;
        public float light_Attack_02_Modifier = 1.2f;
        public float heavy_Attack_01_Modifier = 1.5f;
        public float heavy_Attack_02_Modifier = 1.6f;
        public float charge_Attack_01_Modifier = 2.0f;
        public float charge_Attack_02_Modifier = 2.2f;
        // Critical damage modifier etc

        [Header("Stamina Cost Modifiers")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;
        // Running attack stamina cost modifier
        // Heavy attack stamina cost modifier etc

        // Item based actions (RB, RT, LB, LT)
        [Header("Actions")]
        public WeaponItemAction oh_RB_Action; // One hand right bumper action
        public WeaponItemAction oh_RT_Action; // One hand right trigger action

        // Ash of war

        // Blocking sounds
        [Header("Whooshes")]
        public AudioClip[] whooshes;

    }
}
