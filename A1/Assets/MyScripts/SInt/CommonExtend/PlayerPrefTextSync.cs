using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerPrefTextSync : MonoBehaviour
{
    public TextMeshProUGUI text;
    bool hasText;
    public string playerPrefsFloatName = "Score";
    // Start is called before the first frame update
    void Start()
    {
        hasText = TryGetComponent(out text);

    }

    // Update is called once per frame
    void Update()
    {
        if (hasText)
        {
            text.text = PlayerPrefs.GetFloat(playerPrefsFloatName).ToString();
        }
    }
}
