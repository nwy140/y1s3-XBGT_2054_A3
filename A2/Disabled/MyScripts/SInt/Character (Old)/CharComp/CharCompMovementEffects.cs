using System;
using StandardAssets.Characters.Effects;
using StandardAssets.Characters.ThirdPerson;
using UnityEngine;

namespace MyScripts.SInt.Character.CharComp
{
    public class CharCompMovementEffects : MonoBehaviour
    {
        [Tooltip("Properties of the movement event handler")]
        public CharCompMovementEventHandler m_MovementEffects;

        public float planarSpeed;

        private void Awake()
        {
            m_MovementEffects.Init(this);
        }
    }

    /// <summary>
    /// Handles the third person movement event triggers and event IDs.
    /// As well as collider movement detections <see cref="ColliderMovementDetection"/>
    /// </summary>
    [Serializable]
    public class CharCompMovementEventHandler : MovementEventHandler
    {
        [Tooltip("Reference to the left foot collider movement detection")]
        public ColliderMovementDetection m_LeftFootDetection;

        [Tooltip("Reference to the right foot collider movement detection")]
        public ColliderMovementDetection m_RightFootDetection;

        [SerializeField, Tooltip("The maximum speed used to normalized planar speed")]
        float m_MaximumSpeed = 10f;

        // Cached reference to the ThirdPersonBrain
        CharCompMovementEffects m_CharCompMovementEffects;


        /// <summary>
        /// Gives the <see cref="ThirdPersonMovementEventHandler"/> context of the <see cref="ThirdPersonBrain"/>
        /// </summary>
        /// <param name="brainToUse">The <see cref="ThirdPersonBrain"/> that called Init</param>
        public void Init(CharCompMovementEffects brainToUse)
        {
            base.SetCurrentMovementEventLibrary(defaultLibrary);
            m_CharCompMovementEffects = brainToUse;
        }

        /// <summary>
        /// Subscribe to the movement detection events
        /// </summary>
        public void Subscribe()
        {
            if (m_LeftFootDetection != null)
            {
                m_LeftFootDetection.detection += HandleLeftFoot;
            }

            if (m_RightFootDetection != null)
            {
                m_RightFootDetection.detection += HandleRightFoot;
            }
        }

        /// <summary>
        /// Unsubscribe to the movement detection events
        /// </summary>
        public void Unsubscribe()
        {
            if (m_LeftFootDetection != null)
            {
                m_LeftFootDetection.detection -= HandleLeftFoot;
            }

            if (m_RightFootDetection != null)
            {
                m_RightFootDetection.detection -= HandleRightFoot;
            }
        }

        /// <summary>
        /// Plays the Jumping movement events
        /// </summary>
        public void Jumped()
        {
            // We do not neccesarily have to use the standard asset character's audio play method,
            // we could use our own
            PlayJumping(new MovementEventData(m_CharCompMovementEffects.transform));
            Debug.Log("Jump Movement Effect");
        }

        /// <summary>
        /// Plays the landing movement events
        /// </summary>
        public void Landed()
        {
            PlayLanding(new MovementEventData(m_CharCompMovementEffects.transform));
            Debug.Log("Landed Movement Effect");
        }

        // Plays the left foot movement events
        //      movementEventData: the data need to play the event
        //      physicMaterial: physic material of the footstep
        void HandleLeftFoot(MovementEventData movementEventData, PhysicMaterial physicMaterial)
        {
            SetPhysicMaterial(physicMaterial);
            movementEventData.normalizedSpeed = Mathf.Clamp01(m_CharCompMovementEffects.planarSpeed / m_MaximumSpeed);
            PlayLeftFoot(movementEventData);
            // Debug.Log("Left Foot");
        }

        // Plays the right foot movement events
        //      movementEventData: the data need to play the event
        //      physicMaterial: physic material of the footstep
        void HandleRightFoot(MovementEventData movementEventData, PhysicMaterial physicMaterial)
        {
            SetPhysicMaterial(physicMaterial);
            movementEventData.normalizedSpeed = Mathf.Clamp01(m_CharCompMovementEffects.planarSpeed / m_MaximumSpeed);
            PlayRightFoot(movementEventData);
            // Debug.Log("Right Foot");
        }
    }
}