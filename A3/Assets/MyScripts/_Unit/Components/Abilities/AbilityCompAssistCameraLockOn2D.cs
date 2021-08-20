using System.Linq; //Sorting for Orderby
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.Animations;
using UnityEngine.Events;

#region Script (modified slightly by SInt) Taken From https://amirazmi.net/targeting-system/ // Uses a Trigger Collider for lock on
/*******************************************************************/
/* \project: Spicy Dice
 * \author: Amir Azmi
 * \date: 11/13/2019
 * \brief: Locks onto the closest object when a key F is pressed and
 *         unlocks when F is pressed again, if the ai gets out of the 
 *         range of the camera collider, then targeting is broken free,
 *         if the camera gets out of range, then it also breaks free
 *         You can also cycle targets with the left and right arrow
 *         key within the m_Angle range
 *         
*/
/*******************************************************************/



public class AbilityCompAssistCameraLockOn2D : AbilityBaseComp
{
    [Header("Ability Functionalities")]
    public float m_Angle = 35.0f; //default for angle variable
    [Tooltip("// SInt Edit // Can Also be Main Camera or Root Gameobject")]
    public GameObject m_AnglePivotPoint; // SInt Edit // Can Also be Main CamerA
    public bool useMainCameraAsAnglePivotPoint;

    public GameObject m_targetInFrontClosest; //object your targeting
    public GameObject m_LockedOnTargetClosest; //object your targeting
    public bool m_IsTargeting = false; //if targeting is true
    public List<GameObject> m_CandidateTargets; //list of candidate game objects
    public List<GameObject> m_ObjectsInCollider;
    private bool m_TargetButton = false; //target button
    public bool m_AxisRight = false; //moving axis to the right
    public bool m_AxisLeft = false; //moving the axis to the left
    //private int m_NumberOfTargetsWithinRange = 0; //number of targets within range
    private Animator m_Animator; //animator for palyer

    public UnityEvent OnDetected_TargetInFrontClosest;

    protected override void Awake()
    {
        eAbilityTechniques = EAbilityTechniques.LockOn;
        base.Awake();
        desc = "Look Camera towards a possible character target";
        devComment = "Detects target via Sphere Collider, and picks the closet one within a certain angle";
        //willStartAbility_OnButtonDown = true;
        //m_Animator = Player.Instance.GetComponent<Animator>();
        if (useMainCameraAsAnglePivotPoint)
        {
            m_AnglePivotPoint = Camera.main.gameObject;
        }
        else
        {
            if (m_AnglePivotPoint == null)
            {
                m_AnglePivotPoint = gameObject;
            }
        }
        LockOnTargetIndicatorParentConstraint.constraintActive = true;
        TargetInFrontIndicatorParentConstraint.constraintActive = true;
    }
    private void Start()
    {
    }

    public override void OnInit()
    {
        if (eUnitPossesion == EUnitPossesionType.ai)
        {
        }
        else if (eUnitPossesion == EUnitPossesionType.player)
        {
            willActivateAbility_OnUpdate = true;
            willActivateAbility_OnButton = false;
        }
    }
    public override void OnUsageRequirementsNotMet()
    {
        base.OnUsageRequirementsNotMet();
        m_LockedOnTargetClosest = null;
    }
    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveEnter();
        AbilityFunctionality();
    }

    public override void AbilityFunctionalityPlayer()
    {
        base.AbilityFunctionalityPlayer();
        //check if the targeting button has been pressed
        //buttonDown = Input.GetKeyDown(KeyCode.F); // Temp // Comment this out later

        //if candidate object happens to be null reset targeting
        if (m_LockedOnTargetClosest == null)
        {
            m_TargetButton = false;
        }

        //remove null objects in the list and decrement the counter
        // Could optimize through some onDelete event system, probably really not worth
        for (int i = m_CandidateTargets.Count - 1; i >= 0; --i)
        {
            if (m_CandidateTargets[i] == null || !m_CandidateTargets[i].activeInHierarchy)
            {
                m_CandidateTargets.RemoveAt(i);
                //m_NumberOfTargetsWithinRange--;
            }
        }
        // SInt also remove objects where linecast fails
        for (var i = 0; i < m_ObjectsInCollider.Count(); ++i)
        {
            if (ValidateTargetDetectedLinecast(gameObject, m_ObjectsInCollider[i]) == false)
            {
                if (m_CandidateTargets.Contains(m_ObjectsInCollider[i]) == true)
                {
                    //m_CandidateTargets.RemoveAt(i);
                    m_CandidateTargets.Remove(m_ObjectsInCollider[i]);
                    //m_NumberOfTargetsWithinRange--;
                }
            }
            else
            {
                if (m_CandidateTargets.Contains(m_ObjectsInCollider[i]) == false)
                {
                    m_CandidateTargets.Add(m_ObjectsInCollider[i]);
                    //m_NumberOfTargetsWithinRange++;
                }
            }
        }
        //id target button has been pressed and there are targets within the targeting radius
        if (/*m_NumberOfTargetsWithinRange > 0 &&*/ m_CandidateTargets.Count > 0)
        {
            //if you want to sort by distance uncomment line below
            //List<GameObject> Sorted_List = m_CandidateTargets.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).ToList(); // sort objects in order as they enter
            if (isSortCandidatesByCameraAngleOrder == true)
            {
                //sorts objects by angle and stores it into the Sorted_List
                List<GameObject> Sorted_List = m_CandidateTargets.OrderBy(go =>
                {
                    Vector3 target_direction = go.transform.position - m_AnglePivotPoint.transform.position; //get vector from camera to target
                    var camera_forward = new Vector2(m_AnglePivotPoint.transform.forward.x, m_AnglePivotPoint.transform.forward.z); //convert camera forward direction into 2D vector
                    var target_dir = new Vector2(target_direction.x, target_direction.z); //do the same with target direction
                    float angle = Vector2.Angle(camera_forward, target_dir); //get the angle between the two vectors
                    return angle;
                }).ToList(); //store the objects based off of the angle into the Sorted_List


                //copy objects into the main game_object list
                for (var i = 0; i < m_CandidateTargets.Count(); ++i)
                {
                    m_CandidateTargets[i] = Sorted_List[i];
                }
                //remove objects that happen to die before selecting next target
                for (var i = 0; i < m_ObjectsInCollider.Count(); ++i)
                {
                    if (!m_ObjectsInCollider[i].activeInHierarchy)
                    {
                        m_CandidateTargets.Remove(m_ObjectsInCollider[i]);
                        //m_NumberOfTargetsWithinRange--;
                    }
                }
            }


            if (buttonDown)
            {
                GameObject old_object = m_LockedOnTargetClosest;  //current object to untarget

                UnTarget(old_object); //untarget old object shows to not show the indicator


                //Super cool thing to note,  "float angle = Vector2.Angle(camera_forward, target_dir);" sorts by abs(angle) aka unsigned so the first object you target is always the object you are most looking at
                m_LockedOnTargetClosest = m_CandidateTargets.First();// m_CandidateTargets.First(); //set target as the target you are most looking at
                                                                     //target the new object
                if (old_object != m_LockedOnTargetClosest)
                {
                    Target(m_LockedOnTargetClosest);  //show the indicator on the new object
                }
                m_TargetButton = !m_TargetButton; //this handles the unlocking / locking
            }

        }


        //if I am targeting, there are candidate objects within my radius, and current target is not null and the object is alive aka in the scene
        if (m_TargetButton && /*m_NumberOfTargetsWithinRange > 0 &&*/ m_CandidateTargets.Count > 0 && m_LockedOnTargetClosest != null && m_LockedOnTargetClosest.activeInHierarchy)
        {
            //gets the angle of the between the current game object and camera
            Vector3 target_direction = m_LockedOnTargetClosest.transform.position - m_AnglePivotPoint.transform.position;
            var camera_forward = new Vector2(m_AnglePivotPoint.transform.forward.x, m_AnglePivotPoint.transform.forward.z);
            var target_dir = new Vector2(target_direction.x, target_direction.z);
            float angle = Vector2.Angle(camera_forward, target_dir);

            //check if the object's angle is within the zone of targeting
            if (angle < Mathf.Abs(m_Angle))
            {
                m_IsTargeting = true; //set targeting to true
                ///*
                if (Axis > 0.0f || Input.GetKeyDown(KeyCode.RightArrow) && m_IsTargeting) //if the right stick was moved to the right
                {
                    //List<GameObject> Sorted_List = m_CandidateTargets.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).ToList();

                    if (m_AxisRight == false) //if axis initally was false
                    {
                        //sort objects in the list while targeting, yes you want to do this so if enemies move around it still keeps the list correct
                        List<GameObject> Sorted_List = m_CandidateTargets.OrderBy(go =>
                        {
                            Vector3 target_dir_vec3 = go.transform.position - Camera.main.transform.position;
                            var camera_forward_dir = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
                            var new_target_dir = new Vector2(target_dir_vec3.x, target_dir_vec3.z);
                            float angle_from_sorted_list = Vector2.SignedAngle(camera_forward_dir, new_target_dir);
                            return angle_from_sorted_list;
                        }).ToList();

                        //copy the objects from the sorted list into the main list
                        for (var i = 0; i < m_CandidateTargets.Count(); ++i)
                        {
                            m_CandidateTargets[i] = Sorted_List[i];
                        }

                        //check if there is an object to the right
                        if (m_CandidateTargets.IndexOf(m_LockedOnTargetClosest) - 1 >= 0)
                        {
                            //turn off current idicator
                            UnTarget(m_LockedOnTargetClosest);
                            int index = m_CandidateTargets.IndexOf(m_LockedOnTargetClosest) - 1;

                            //check its angle
                            GameObject next_targeted_object = m_CandidateTargets[index];
                            Vector3 target_direction_vec3 = next_targeted_object.transform.position - Camera.main.transform.position;
                            var camera_forward_vec2 = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
                            var new_target_dir = new Vector2(target_direction_vec3.x, target_direction_vec3.z);
                            float angle_between_vectors = Vector2.Angle(camera_forward_vec2, new_target_dir);

                            //if the angle is within range of targeting angle
                            if (angle_between_vectors < Mathf.Abs(m_Angle))
                            {

                                //set the new object as the target
                                m_LockedOnTargetClosest = m_CandidateTargets[index];
                            }

                            //show the indicator of the target
                            Target(m_LockedOnTargetClosest);

                            //treat angle as button type
                            m_AxisRight = !m_AxisRight;
                        }
                    }
                }
                else if (Axis < 0.0f || Input.GetKeyDown(KeyCode.LeftArrow) && m_IsTargeting)
                {
                    //initially the leftAxis bool should be false for togglable logic       
                    if (m_AxisLeft == false)
                    {
                        //sort objects by the SignedAngle  for the same reasons statef for right cycling
                        List<GameObject> Sorted_List = m_CandidateTargets.OrderBy(go =>
                        {
                            //direction from  Camera to the target object indicated by go
                            Vector3 targetDir = go.transform.position - Camera.main.transform.position;

                            //convert Camera direction into 2D vector
                            var cameraForward = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
                            //convert target direction into the 2D vector
                            var new_target_dir = new Vector2(targetDir.x, targetDir.z);

                            // calculate angle between the two vectors
                            float angle_between_vectors = Vector2.SignedAngle(cameraForward, new_target_dir);
                            return angle_between_vectors;
                        }).ToList();

                        //put the sorted list into the candidate list
                        for (var i = 0; i < m_CandidateTargets.Count(); ++i)
                        {
                            m_CandidateTargets[i] = Sorted_List[i];
                        }

                        //notice here how I check if there is a next valid object in the list
                        if (m_CandidateTargets.IndexOf(m_LockedOnTargetClosest) + 1 < m_CandidateTargets.Count)
                        {
                            //turn off indicator
                            UnTarget(m_LockedOnTargetClosest);
                            int index = m_CandidateTargets.IndexOf(m_LockedOnTargetClosest) + 1;


                            //store this next object into a gameObject 
                            GameObject nextTargetedObject = m_CandidateTargets[index];

                            //calcuate the angle to see if the next object is within defined targeting range
                            Vector3 targeDir = nextTargetedObject.transform.position - Camera.main.transform.position;

                            var cameraFor = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);

                            var newtarget_dir = new Vector2(targeDir.x, targeDir.z);
                            float anglex = Vector2.Angle(cameraFor, newtarget_dir);

                            //if the next targeted object is a valid target
                            if (anglex < Mathf.Abs(m_Angle))
                            {

                                // set the m_ObjectClosestToCamera as the next object in the list
                                m_LockedOnTargetClosest = m_CandidateTargets[index];
                            }

                            //tell indicator what to do
                            Target(m_LockedOnTargetClosest);

                            //for togglable logic
                            m_AxisLeft = !m_AxisLeft;
                        }
                    }
                }
                else
                {
                    //once the axis is 0 it means the locking has been reset
                    m_AxisRight = false;
                    m_AxisLeft = false;
                }
                //*/
            }
            else
            {
                //reset the target button
                m_TargetButton = false; // SInt -  Uncomment this to lose lock on target when you look away out of the angle variable from your lock on target 
            }
        }
        else
        {
            //set targeting to false here
            if (m_LockedOnTargetClosest != null)
            {
                m_IsTargeting = false;
                //if th object is not null here make it null and untarget
                UnTarget(m_LockedOnTargetClosest);

                m_LockedOnTargetClosest = null;

            }
            //Tell the ANIMATOR
            //m_Animator.SetBool("TargetLocked", m_IsTargeting);
        }


        // SInt Edits
        if (m_CandidateTargets.Count > 0)
        {

            m_targetInFrontClosest = m_CandidateTargets.First();
        }
        else
        {
            m_targetInFrontClosest = null;
        }
        if (m_LockedOnTargetClosest != null)
        {
            SetCosmeticsVisibility(m_LockedOnTargetClosest.activeInHierarchy);

        }
        else
        {
            SetCosmeticsVisibility(false);

        }

    }

    public override void OnAbilityActiveExit()
    {
        base.OnAbilityActiveExit();
        //m_AxisRight = false;
        //m_AxisLeft = false;
        if (m_AxisLeft || m_AxisRight)
        {
            Axis = 0;
        }
    }

    #region SInt's Edits
    [Header("SInt's Tweaks")]
    public bool isSortCandidatesByCameraAngleOrder = true;
    [Header("SInt's Variables")]
    public CinemachineVirtualCameraBase CM_LockOn;
    [Header("Cosmetics")]
    public ParentConstraint LockOnTargetIndicatorParentConstraint;
    public GameObject LockOnTargetIndicatorCosmeticsObj; 
    public ParentConstraint TargetInFrontIndicatorParentConstraint;
    public GameObject TargetInFrontIndicatorCosmeticsObj;

    public LayerMask allowedLayers; // layers that targeting system detects
    public LayerMask occlusionLayers; // layers that targeting system block Linecast
    public float occlusionCastOffsetHeight;
    public Color occlusionCastColor = Color.red;
    public Color sensedCastColor = Color.green;
    public bool isUseAllowedOnlyTags;
    public List<String> allowedOnlyTags;
    public List<String> ignoreTags;
    public List<GameObject> ignoreObjs;

    //void OnDrawGizmosSelected()
    private void OnDrawGizmos()
    {
        if (isDebugLog)
        {
            SphereCollider col;
            CircleCollider2D col2D;
            float totalFOV = m_Angle;
            float rayRange = 0;

            if (TryGetComponent<SphereCollider>(out col))
            {
                rayRange = col.radius;

            }
            else if (TryGetComponent<CircleCollider2D>(out col2D))
            {
                rayRange = col2D.radius;
            }
            float halfFOV = totalFOV / 2.0f;
            //Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV,  Vector3.up);
            //Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV,  Vector3.up);
            //Vector3 leftRayDirection = leftRayRotation * transform.forward;
            //Vector3 rightRayDirection = rightRayRotation * transform.forward;
            //Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
            //Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);

            var coneDirection = totalFOV;
            //Ref: https://stackoverflow.com/questions/52130986/can-we-create-a-gizmos-like-cone-in-unity-with-script   Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
            Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
            Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

            Vector3 upRayDirection = upRayRotation * transform.right * rayRange;
            Vector3 downRayDirection = downRayRotation * transform.right * rayRange;

            Gizmos.DrawRay(transform.position, upRayDirection);
            Gizmos.DrawRay(transform.position, downRayDirection);
            Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
        }
    }
    public void SetCosmeticsVisibility(bool isCurrentlyLockedOn)
    {
        if (CM_LockOn)
        {
            if (isCurrentlyLockedOn)
            {
                CM_LockOn.Priority = 100;
                // LockOnTarget
                if (m_LockedOnTargetClosest)
                {
                    ConstraintCommon.SetFirstCSourceSnapTargetParent(LockOnTargetIndicatorParentConstraint, m_LockedOnTargetClosest.transform);
                    LockOnTargetIndicatorCosmeticsObj.SetActive(true);
                }

                // target in front
                if (m_targetInFrontClosest && m_targetInFrontClosest.activeInHierarchy == true)
                {
                    TargetInFrontIndicatorCosmeticsObj.SetActive(false);
                    ConstraintCommon.SetFirstCSourceSnapTargetParent(TargetInFrontIndicatorParentConstraint, transform);
                } 
            }
            else
            {
                CM_LockOn.Priority = -100;
                // LockOnTarget
                if (m_LockedOnTargetClosest)
                {
                    ConstraintCommon.SetFirstCSourceSnapTargetParent(LockOnTargetIndicatorParentConstraint, transform);
                }
                LockOnTargetIndicatorCosmeticsObj.SetActive(false);

                // target in front
                if (m_targetInFrontClosest && m_targetInFrontClosest.activeInHierarchy == true)
                {
                    ConstraintCommon.SetFirstCSourceSnapTargetParent(TargetInFrontIndicatorParentConstraint, m_targetInFrontClosest.transform);
                    TargetInFrontIndicatorCosmeticsObj.SetActive(true);
                }
                else
                {
                    TargetInFrontIndicatorCosmeticsObj.SetActive(false);
                }
            }
            if (m_targetInFrontClosest == null || m_targetInFrontClosest.activeInHierarchy == false)
            {
                TargetInFrontIndicatorCosmeticsObj.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        m_CandidateTargets.Clear();
    }

    #endregion SInt's Edits
    public bool ValidateTargetDetectedLinecast(GameObject pivot, GameObject other)
    {
        if (other == null)
        {
            return false;
        }
        Vector3 origin = pivot.transform.position;
        Vector3 dest = other.transform.position;
        origin.y = origin.y + occlusionCastOffsetHeight;
        // Perform linecast to see if a wall/occlusion layer is blocking our sight
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            //if (isAllowDebug)
            //{
            Debug.DrawLine(origin, dest, occlusionCastColor, 2, false);
            //}
            //print("Linecast blocked");
            return false;
        }
        else
        {
            //if (isAllowDebug)
            //{
            Debug.DrawLine(origin, dest, sensedCastColor, 2, false);
            //}
        }
        return true; // target accepted
    }
    public bool ValidateTargetDetectedTagsLayers(GameObject other)
    {
        // Evaluate Tags and Layers
        if (isUseAllowedOnlyTags)
        {
            if (allowedOnlyTags.Contains(other.tag) == false)
            {
                return false; // target rejected
            }
        }
        if (ignoreTags.Contains(other.tag) /*|| ignoreLayers == (ignoreLayers | (1 << other.layer))*/)
        {
            print(1);
            return false;
        }
        if (ignoreObjs.Contains(other))
        {
            print(2);
            return false;
        }
        if (allowedLayers != (allowedLayers | (1 << other.layer)))
        {
            //print(3 + other.layer.ToString());
            return false;
        }
        return true; // target accepted
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ValidateTargetDetectedTagsLayers(other.gameObject) == false)
        {
            return;
        }

        //++m_NumberOfTargetsWithinRange;
        m_ObjectsInCollider.Add(other.gameObject);
        m_CandidateTargets = new List<GameObject>(m_ObjectsInCollider); 
    }
    private void OnTriggerStay(Collider other)
    {
    }
    private void OnTriggerExit(Collider other)
    {
        m_ObjectsInCollider.Remove(other.gameObject);
        if (m_CandidateTargets.Contains(other.gameObject))
        {
            m_CandidateTargets.Remove(other.gameObject);
            //--m_NumberOfTargetsWithinRange;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            //Debug.Log(ValidateTargetDetectedTagsLayers(collision.gameObject));
        if (ValidateTargetDetectedTagsLayers(collision.gameObject) == false)
        {

            return;
        }

        //++m_NumberOfTargetsWithinRange;
        m_ObjectsInCollider.Add(collision.gameObject);
        m_CandidateTargets = new List<GameObject>(m_ObjectsInCollider);
        OnDetected_TargetInFrontClosest.Invoke();

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        m_ObjectsInCollider.Remove(collision.gameObject);
        if (m_CandidateTargets.Contains(collision.gameObject))
        {
            m_CandidateTargets.Remove(collision.gameObject);
            //--m_NumberOfTargetsWithinRange;
        }
    }
    #region Private

    private void Target(GameObject possibleLockedOnTarget)
    {

        if (possibleLockedOnTarget != null)
        {
            // SInt's Edits
            //var agent = possibleLockedOnTarget.GetComponentInParent<IAgent>();

            //if (agent != null)
            //{
            //    //agent.OnTargeted(GetComponent<IAgent>());
            //    //agent.OnDie.AddListener(AgentDies);
            //}
        }
    }

    private void UnTarget(GameObject lockedOnTarget)
    {
        if (lockedOnTarget != null)
        {
            // SInt's Edits

            //var agent = lockedOnTarget.GetComponentInParent<IAgent>();

            //if (agent != null)
            //{
            //    agent.OnUntargeted(GetComponent<IAgent>());
            //    agent.OnDie.RemoveListener(AgentDies);
            //}
        }
    }

    //private void AgentDies(IAgent agent)
    //{
    //    m_CandidateTargets.Remove(agent.gameObject.gameObject);
    //    UnTarget(agent.gameObject);
    //    m_ObjectClosestToCamera = null;
    //}
    #endregion
}
#endregion New Script (modified slightly by SInt) Taken From https://amirazmi.net/targeting-system/ // Uses a Trigger Collider for lock on

// Old Script
#region SInt's Old Lock On Implementation with Custom made AI Sensor 
/*
public class AbilityCompAssistCameraLockOn : AbilityBaseComp
{
    protected override void Awake()
    {
        eAbilityTechniques = EAbilityTechniques.LockOn;
        base.Awake();
        desc = "Jump off the ground to reach greater heights";
        devComment = "Sync ButtonDown with abilityIsJumping bool variable in _unitCharacterController";
        //willStartAbility_OnButtonDown = true;
    }
    [Header("AbilityFunctionality")]
    public bool isLockedOn;
    public override void AbilityFunctionality()
    {
        base.AbilityFunctionality();
        isLockedOn = !isLockedOn;
    }

    //public override IEnumerator OnAbilityStarted()
    //{
    //    AbilityFunctionality();
    //    return base.OnAbilityStarted();
    //}

    /*
    public void OnGeneralLockOn()
    {
        if (GeneralLockOn_GetIsLockedOn() == false)
        {
            OnGeneralLockOn_KeyDown();
        }
        else
        {
            OnGeneralLockOn_SecondKeyDown();
        }
    }
     

    #region Key Helper methods

    #region OnGeneralLockOn

    [Header("General Locked ON")]
    public bool isGeneralLockedOn = false;

    public void OnGeneralLockOn_KeyDown()
    {
        if (isGeneralLockedOn == false && _unitRefs._charCompAITargetingSystem_LineOfSightVision.HasTarget)
        {
            var cineLookAtTargetObj = _unitRefs._charCompAITargetingSystem_LineOfSightVision.Target;

            if (cineLookAtTargetObj != null)
            {
                if (cineLookAtTargetObj.activeInHierarchy &&
                    cineLookAtTargetObj.gameObject != _unitRefs.gameObject)
                {
                    cineLookAtTargetObj = OnGeneralLockOn_GetPossibleTargetChildOffsetPoint(cineLookAtTargetObj);
                    // Add a source target to LockedOnIndicator's Parent Constraint to enable LockOn for that target Object
                    ConstraintCommon.SetFirstCSourceSnapTargetParent(_unitRefs.HUD_LockOnChosenTargetPrnCstrain,
                        cineLookAtTargetObj.transform);

                    //? is a null checker shortcut // A widely unknown C# feature I think
                    GeneralLockOn_SetIsLockedOn(true);
                }
            }
        }
    }
    public GameObject OnGeneralLockOn_GetPossibleTargetChildOffsetPoint(GameObject cineLookAtTargetObj)
    {
        GameObject result = cineLookAtTargetObj;
        if (cineLookAtTargetObj.transform.childCount > 0)
        {
            //if first child of target has offset point obj, use the transform of that offset point obj
            if (cineLookAtTargetObj.transform.GetChild(0).CompareTag("TargetOffsetPoint"))
            {
                result = cineLookAtTargetObj.transform.GetChild(0).gameObject;
            }
            // else reuse original AISensor detected target as cineLookAtTarget rather than its offsetpoint
        }
        return result;
    }
    // Check for the second time you press the same button
    public void OnGeneralLockOn_SecondKeyDown()
    {
        GeneralLockOn_SetIsLockedOn(false);
    }

    public void OnGeneralLockOn_SecondKeyDown_Handling()
    {
        if (_unitRefs._charCompAITargetingSystem_360Vision_Far.HasTarget == false)
        {
            GeneralLockOn_SetIsLockedOn(false);
        }
        if (_unitRefs.HUD_LockOnChosenTargetPrnCstrain.sourceCount > 0)
        {
            if (_unitRefs.HUD_LockOnChosenTargetPrnCstrain.GetSource(0).sourceTransform == null)
            {
                GeneralLockOn_SetIsLockedOn(false);
            }
            if (_unitRefs.HUD_LockOnChosenTargetPrnCstrain.GetSource(0).sourceTransform.gameObject.activeInHierarchy == false)
            {
                GeneralLockOn_SetIsLockedOn(false);
            }
        }
    }

    public void GeneralLockOn_SetIsLockedOn(bool newIsLockedOn)
    {
        isGeneralLockedOn = newIsLockedOn;
        //_cr.HUD_LockOnChosenTargetPrnCstrain.enabled = newIsLockedOn;
        GameObject cineLookAtTargetObj = _unitRefs.HUD_LockOnChosenTargetIndicator;
        if (newIsLockedOn == true)
        {
            _unitRefs.CM_LockOn.Priority = 12;
        }
        else
        {
            _unitRefs.CM_LockOn.Priority = 0;
        }
        _unitRefs.HUD_LockOnChosenTargetIndicator.gameObject.SetActive(newIsLockedOn);
        if (_unitRefs._lookAtIK != null)
        {
            if (newIsLockedOn == true)
            {
                cineLookAtTargetObj = OnGeneralLockOn_GetPossibleTargetChildOffsetPoint(cineLookAtTargetObj);
                nextLookAtIK_Target = cineLookAtTargetObj.transform;
            }
        }

        if (newIsLockedOn == false)
        {
            if (_unitRefs.HUD_LockOnChosenTargetPrnCstrain.sourceCount > 0)
            {
                _unitRefs.HUD_LockOnChosenTargetPrnCstrain.RemoveSource(0);
            }
        }
    }

    public bool GeneralLockOn_GetIsLockedOn()
    {
        return isGeneralLockedOn;
    }

    #endregion

    #endregion Key Helper methods

    #region Automated

    public bool isPossibleTargetFound;


    #region OnPossiblePriorityTargetFound

    public bool isAllow_LookAtIK_OnPossibleTargetFound = true;

    public void AutomatedOnPossiblePriorityTargetFound_Handling()
    {
        if (GeneralLockOn_GetIsLockedOn() == true)
        {
            _unitRefs.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(false);
            isPossibleTargetFound = false;
            return;
        }

        if (_unitRefs.HUD_LockOnPossibleTargetPrnCstrain != null
            && _unitRefs._charCompAITargetingSystem_LineOfSightVision.HasTarget)
        {
            var cineLookAtTargetObj = _unitRefs._charCompAITargetingSystem_LineOfSightVision.Target;
            if (cineLookAtTargetObj != null)
            {
                if (cineLookAtTargetObj.activeInHierarchy &&
                    cineLookAtTargetObj.gameObject != _unitRefs.gameObject)
                {

                    cineLookAtTargetObj = OnGeneralLockOn_GetPossibleTargetChildOffsetPoint(cineLookAtTargetObj);
                    ConstraintCommon.SetFirstCSourceSnapTargetParent(_unitRefs.HUD_LockOnPossibleTargetPrnCstrain,
                        cineLookAtTargetObj.transform);

                    _unitRefs.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(true);
                    isPossibleTargetFound = true;
                    if (_unitRefs._lookAtIK != null)
                    {
                        if (nextLookAtIK_Target == null)
                        {
                            nextLookAtIK_Target = cineLookAtTargetObj.transform;
                        }
                        if (Vector3.Distance(cineLookAtTargetObj.transform.position, nextLookAtIK_Target.position) > 1.08) // targets not too close to each other
                        {
                            nextLookAtIK_Target = cineLookAtTargetObj.transform;
                        }
                    }
                }
                else
                {
                    isPossibleTargetFound = false;
                    _unitRefs.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(false);
                }
            }
            else
            {
                isPossibleTargetFound = false;
                _unitRefs.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(false);
            }
        }
        else
        {
            isPossibleTargetFound = false;
            _unitRefs.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(false);
        }
    }

    public bool Automated_GetIsPossibleTargetFound()
    {
        return isPossibleTargetFound;
    }

    #region LookAtIKLerpWeight

    public float LookAtIk_WeightLerpSpeed = 0.5f;
    public float LookAtIK_isLockedWeightLerpMultiplier = 1.5f;
    public Transform nextLookAtIK_Target;
    public bool isLookAtIKCurrentlySwitchingTargets;

    //void AutomatedLookAtIKLerpWeight_Handling()
    //{
    //    // lerp lookatik weight
    //    if (Automated_GetIsPossibleTargetFound() || GeneralLockOn_GetIsLockedOn())
    //    {
    //        if (((isAllow_LookAtIK_OnPossibleTargetFound && Automated_GetIsPossibleTargetFound() &&
    //              !GeneralLockOn_GetIsLockedOn())
    //             || GeneralLockOn_GetIsLockedOn()) &&
    //             (_unitRefs._lookAtIK.solver.target != null &&
    //             _unitRefs._lookAtIK.solver.target == nextLookAtIK_Target))
    //        {
    //            if (_unitRefs._lookAtIK != null)
    //            {
    //                AutomatedLookAtIKLerpWeight_IncrementWeight();
    //            }
    //        }
    //        else
    //        {
    //            AutomatedLookAtIKLerpWeight_DecrementWeight();
    //        }
    //    }
    //    else
    //    {
    //        AutomatedLookAtIKLerpWeight_DecrementWeight();
    //    }

    //    if (nextLookAtIK_Target != null)
    //    {
    //        if (_unitRefs._lookAtIK.solver.target != nextLookAtIK_Target)
    //        {
    //            isLookAtIKCurrentlySwitchingTargets = true;
    //        }
    //        else
    //        {
    //            isLookAtIKCurrentlySwitchingTargets = false;
    //        }
    //    }

    //    if (_unitRefs._lookAtIK.solver.IKPositionWeight == 0)
    //    {
    //        _unitRefs._lookAtIK.solver.target = nextLookAtIK_Target;
    //    }
    //}


    //void AutomatedLookAtIKLerpWeight_IncrementWeight()
    //{
    //    if (_unitRefs._lookAtIK != null)
    //    {
    //        float multiplier = 1f;
    //        if (GeneralLockOn_GetIsLockedOn() == true)
    //        {
    //            multiplier = LookAtIK_isLockedWeightLerpMultiplier;
    //        }
    //        _unitRefs._lookAtIK.solver.SetLookAtWeight(_unitRefs._lookAtIK.solver.IKPositionWeight +=
    //            Time.deltaTime * LookAtIk_WeightLerpSpeed * multiplier);
    //    }
    //}

    //void AutomatedLookAtIKLerpWeight_DecrementWeight()
    //{
    //    if (_unitRefs._lookAtIK != null)
    //    {
    //        float multiplier = 1f;
    //        if (GeneralLockOn_GetIsLockedOn() == true)
    //        {
    //            multiplier = LookAtIK_isLockedWeightLerpMultiplier;
    //        }
    //        _unitRefs._lookAtIK.solver.SetLookAtWeight(_unitRefs._lookAtIK.solver.IKPositionWeight -=
    //            Time.deltaTime * LookAtIk_WeightLerpSpeed * multiplier);
    //    }
    //}
    */

#endregion SInt's Old Lock On Implementation with Custom made AI Sensor 

