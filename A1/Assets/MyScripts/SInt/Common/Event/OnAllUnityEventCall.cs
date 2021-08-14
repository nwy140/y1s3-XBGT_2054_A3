using UnityEngine;
using UnityEngine.Events;

// Read https://forum.unity.com/threads/unityevent-where-have-you-been-all-my-life.321103/
//          And Read https://www.gamasutra.com/blogs/VivekTank/20180703/321126/How_to_use_C_Events_in_Unity.php
// Some Awesome stuff I found
// You can now call methods by via Unity inspector Events
//  i.e like doing it via an onclick button event,
// for all major MonoBehavior Events with this script
// Monobehavior Events List: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
// You can also inherit from this script if you want make that script to be able to call events from your inspector
// By nwy
public class OnAllUnityEventCall : MonoBehaviour
{

    public UnityEvent eventToCall;

    [Header("Unity Activation Events")]
    public bool isPlayOnEnable = true;
    public bool isPlayOnStart = false;
    public bool isPlayOnAwake = false;
    public bool isPlayOnUpdate = false;
    public bool isPlayOnFixedUpdate = false;
    public bool isPlayOnLateUpdate = false;

    public bool isPlayOnGUI = false;

    [Header("Unity De-Activation Events")]
    public bool isPlayOnDisable = false;
    public bool isPlayOnDestroy = false;

    [Header("Unity Application Events ")]
    public bool isPlayOnApplicationPause = false;

    [Header("Unity Transform Change Events")]
    public bool isPlayOnTransformChildrenChanged = false;
    public bool isPlayOnTransformParentChanged = false;
    public bool isPlayOnBeforeTransformParentChanged = false;


    [Header("Unity 3D Collision Events")]
    public bool isPlayOnCollisionEnter = false;
    public bool isPlayOnCollisionExit = false;
    public bool isPlayOnCollisionStay = false;
    public bool isPlayOnTriggerEnter = false;
    public bool isPlayOnTriggerExit = false;
    public bool isPlayOnTriggerStay = false;

    [Header("Unity 2D Collision Events")]
    public bool isPlayOnCollisionEnter2D = false;
    public bool isPlayOnCollisionExit2D = false;
    public bool isPlayOnCollisionStay2D = false;
    public bool isPlayOnTriggerEnter2D = false;
    public bool isPlayOnTriggerExit2D = false;
    public bool isPlayOnTriggerStay2D = false;



    //public bool isPlayOnDelay = false; TODO: Optional for extending Functionality of this script
    public void PlayEventToCall()
    {
        //transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        eventToCall.Invoke();
    }

    #region Unity Activation Events
    private void OnEnable()
    {
        if(isPlayOnEnable)
            PlayEventToCall();
    }

    private void Awake()
    {
        if (isPlayOnAwake)
        {
            PlayEventToCall();
        }
    }

    private void Start()
    {
        if (isPlayOnStart)
        {
            PlayEventToCall();
        }
    }

    private void Update()
    {
        if (isPlayOnUpdate)
        {
            PlayEventToCall();
        }
    }

    private void FixedUpdate()
    {
        if (isPlayOnFixedUpdate)
        {
            PlayEventToCall();
        }
    }
    private void LateUpdate()
    {
        if (isPlayOnLateUpdate)
        {
            PlayEventToCall();
        }
    }
    private void OnGUI()
    {
        if (isPlayOnGUI)
        {
            PlayEventToCall();
        }
    }
    #endregion Unity Activation Events
    #region Unity De-Activation Events
    private void OnDisable()
    {
        if (isPlayOnDisable)
            PlayEventToCall();
    }

    private void OnDestroy()
    {
        if (isPlayOnDestroy)
            PlayEventToCall();
    }
    #endregion Unity De-Activation Events
    #region Unity Transform Change Events
    private void OnTransformChildrenChanged()
    {
        if(isPlayOnTransformChildrenChanged)
            PlayEventToCall();
    }
    private void OnTransformParentChanged()
    {
        if (isPlayOnTransformParentChanged)
            PlayEventToCall();
    }
    private void OnBeforeTransformParentChanged()
    {
        if (isPlayOnBeforeTransformParentChanged)
            PlayEventToCall();
    }
    #endregion Unity Transform Change Events

    
    #region Unity Application Events 
    private void OnApplicationPause(bool pause)
    {
        if (isPlayOnApplicationPause)
            PlayEventToCall();
    }
    #endregion Unity Application Events 
    #region Unity 2D Collision Events
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPlayOnCollisionEnter2D)
            PlayEventToCall();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isPlayOnCollisionExit2D)
            PlayEventToCall();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isPlayOnCollisionStay2D)
            PlayEventToCall();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayOnTriggerEnter2D)
            PlayEventToCall();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isPlayOnTriggerExit2D)
            PlayEventToCall();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isPlayOnTriggerStay2D)
            PlayEventToCall();
    }
    #endregion Unity 3D Collision Events
    #region Unity 3D Collision Events
    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayOnCollisionEnter)
            PlayEventToCall();
    }
    private void OnCollisionExit(Collision collision)
    {
        if (isPlayOnCollisionExit)
            PlayEventToCall();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (isPlayOnCollisionStay)
            PlayEventToCall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayOnTriggerEnter)
            PlayEventToCall();
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlayOnTriggerExit)
            PlayEventToCall();
    }
    private void OnTriggerStay(Collider other)
    {
        if (isPlayOnTriggerStay)
            PlayEventToCall();
    }
    #endregion Unity 3D Collision Events
     

}

// TODO: Optional OnCollisionEvent
