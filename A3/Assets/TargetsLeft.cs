using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TargetsLeft : MonoBehaviour
{
    public Transform targetsToDefeatParent;
    public List<GameObject> targetsToDefeat;
    public TextMeshProUGUI textMeshProUGUI;

    public int maxTargets;
    private void Awake()
    {
        foreach (Transform t in targetsToDefeatParent)
        {
            targetsToDefeat.Add(t.gameObject);
        }
        maxTargets = targetsToDefeat.Count;
    }
    public bool targetsDefeated;
    public UnityEvent OnAllTargetsDefeated;
    // Update is called once per frame
    void Update()
    {
        textMeshProUGUI.text = targetsToDefeat.Count.ToString() + " / " + maxTargets;
        foreach(var obj in targetsToDefeat)
        {
            if (obj == null)
            {
                targetsToDefeat.Remove(obj); //Causes error
            }
            else if(obj.activeInHierarchy == false)
            {
                targetsToDefeat.Remove(obj); //Causes error
            }
        }
        if(targetsToDefeat.Count == 0 && targetsDefeated == false)
        {
            targetsDefeated = true;
            OnAllTargetsDefeated.Invoke();
        }
    }
}
