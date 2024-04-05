using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace YT
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
        [HideInInspector] public CharacterUIManager characterUIManager;

        [Header("Character Group")]
        public CharacterGroup characterGroup;

        [Header("Flags")]
        public bool isPerformingAction = false;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterUIManager = GetComponent<CharacterUIManager>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", characterLocomotionManager.isGrounded);
            // If character is controlled by us, then assign network position to position of our transform
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // If character is being controlled from elsewhere, then assign its position locally
            else
            {
                // Position
                transform.position = Vector3.SmoothDamp(
                    transform.position, 
                    characterNetworkManager.networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime);

                // Rotation
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            animator.SetBool("isMoving", characterNetworkManager.isMoving.Value);
            characterNetworkManager.OnIsActiveChanged(false, characterNetworkManager.isActive.Value);

            characterNetworkManager.isMoving.OnValueChanged += characterNetworkManager.OnIsMovingChanged;
            characterNetworkManager.isActive.OnValueChanged += characterNetworkManager.OnIsActiveChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            characterNetworkManager.isMoving.OnValueChanged -= characterNetworkManager.OnIsMovingChanged;
            characterNetworkManager.isActive.OnValueChanged -= characterNetworkManager.OnIsActiveChanged;
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // Reset any flags here that need to be reset
                // Nothing yet (eg attack animations)

                // If not grounded, play aerial death animation

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
                }
            }

            // Play some death SFX

            yield return new WaitForSeconds(5);

            // Award players with runes 

            // Disable character
        }

        public virtual void ReviveCharacter()
        {

        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();

            // Adds all of our damageable character colliders, to the list that will be used to ignore collisions
            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            // Adds our character controller collider to the list that will be used to ignore collisions
            ignoreColliders.Add(characterControllerCollider);

            foreach (var collider in ignoreColliders)
            {
                foreach (var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }

    }
}

