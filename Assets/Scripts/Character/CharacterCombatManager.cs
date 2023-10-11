using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        protected virtual void Awake()
        {

        }
    }
}
