using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;
        
        // Tweak Camera Performance
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1; // Bigger the number, longer for camera to reach its position
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30; // Lowest angle to look down
        [SerializeField] float maximumPivot = 60; // Highest angle to look up
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // Used for camera collisions (moves camera object to this position upon colliding)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition; // Value used for camera position
        private float targetCameraZPosition; // Value used for camera position

        [Header("Lock On")]
        [SerializeField] float lockOnRadius = 20;
        [SerializeField] float minimumViewableAngle = -50;
        [SerializeField] float maximumViewableAngle = 50;
        private List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        [SerializeField] float lockOnTargetFollowSpeed = 0.2f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // If locked, force rotation towards target
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                // This rotates the gameObject
                Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // This rotates the pivot object
                rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // Save our rotation values to our look angles,  so when we unlock it doesn't snap too far away
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            // Else rotate regularly
            else
            {
                // Rotate left and right based on horizontal movement on right joystick or mouse
                leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontal_Input * leftAndRightRotationSpeed) * Time.deltaTime;
                // Rotate up and down based on vertical movement
                upAndDownLookAngle -= (PlayerInputManager.instance.cameraVertical_Input * upAndDownRotationSpeed) * Time.deltaTime;
                // Clamp up and down look angle between min and max value
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

                Vector3 cameraRotation = Vector3.zero;
                Quaternion targetRotation;

                // Rotate gameObject left and right
                cameraRotation.y = leftAndRightLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                transform.rotation = targetRotation;

                // Rotate pivot gameObject up and down
                cameraRotation = Vector3.zero;
                cameraRotation.x = upAndDownLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;

            RaycastHit hit;
            // Direction for collision check
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // Check if object infront of camera
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                // If there is, we get distance from it
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // Then equate our target z position
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            // If our target position is less than our collision radius, subtract our collisiono radius (making it snap back)
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // Then apply final position using a lerp over a time of 0.2f
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }

        public void HandleLocatingLockOnTargets()
        {
            float shortestDistance = Mathf.Infinity; // Will be used to determine the target closet to us
            float shortestDistanceOfRightTarget = Mathf.Infinity; // Will be used to determine shortest distance on one axis to the right of current target (closet target to the right of current target, +)
            float shortestDistanceOfLeftTarget = -Mathf.Infinity; // Will be used to determine shortest distance on one axis to the left of current target (-)

            // To do use a layermask
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

                if (lockOnTarget != null)
                {
                    // Check if they are within our field of view
                    Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraObject.transform.forward);

                    // If target is dead, check next potential target
                    if (lockOnTarget.isDead.Value)
                        continue;

                    // If target is us, skip
                    if (lockOnTarget.transform.root == player.transform.root)
                        continue;

                    // Lastly, if the target is outside field of view or blocked by enviro, check next potential target
                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        // TODO add layer mask for enviro layers only
                        if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, 
                            lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, 
                            WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            // We hit something, we cannot see our lock on target
                            continue;
                        }
                        else
                        {
                            // Otherwise, add them to potential targets list
                            availableTargets.Add(lockOnTarget);
                        }
                    }
                }
            }

            // We know sort through our potential targets to see which one we lock onto first
            for (int k = 0; k < availableTargets.Count; k++)
            {
                if (availableTargets[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);
                    Vector3 lockTargetsDirection = availableTargets[k].transform.position - player.transform.position;

                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = availableTargets[k];
                    }
                }
                else
                {
                    ClearLockOnTargets();
                    player.playerNetworkManager.isLockedOn.Value = false;
                }
            }
        }

        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            availableTargets.Clear();
        }
    }
}
