using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    public bool isInteractActive;
    public void OnInteractEnter()
    {
        gameObject.SetActive(false);
    }

    public void OnInteractExit()
    {
        gameObject.SetActive(false);
    }

    public void ToggleInteract()
    {
        isInteractActive = !isInteractActive;
        if (isInteractActive)
        {
            OnInteractEnter();
        }
        else
        {
            OnInteractExit();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
