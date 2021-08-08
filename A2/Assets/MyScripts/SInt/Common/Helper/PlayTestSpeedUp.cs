using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTestSpeedUp : MonoBehaviour
{
    [Header("Press tab to speedup time.timeScale for playtesting debugging purposes")]
    public float tabPressedSpeedTimeScaleSpeed = 4.0f;
    float originalTimeScaleSpeed;
	
	public KeyCode key = KeyCode.Tab;
    private void Start()
    {
        originalTimeScaleSpeed = Time.timeScale;
    }
    void Update()
    {
        // Just Like in An Emulator
        // Hold Tab to speed up game for playtest/debug purposes inside debug build only
        if (Debug.isDebugBuild)
        {
            if (Input.GetKey(key))
            {
                Time.timeScale = tabPressedSpeedTimeScaleSpeed;
            }
            else
            {
                Time.timeScale = originalTimeScaleSpeed;
            }
        }
    }
}
