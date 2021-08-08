using UnityEngine;

public class LookAtCameraMain : MonoBehaviour
{
    private Transform cam;

    public Vector3 rotOffset;
    // Use this for initialization
    void Start()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(cam);
        transform.rotation=Quaternion.Euler(transform.eulerAngles+rotOffset);

    }
}