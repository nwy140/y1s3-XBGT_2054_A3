using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowHideToggle : MonoBehaviour
{
    // Called in Button Component OnClick section
    public void ToggleShowHide() {
        gameObject.SetActive(   !gameObject.activeInHierarchy);
    }
}
