using UnityEngine;

public class TransformAutoRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = Vector3.zero;
    void Update()
    {
        transform.Rotate(
             rotationSpeed.x * Time.deltaTime,
             rotationSpeed.y * Time.deltaTime,
             rotationSpeed.z * Time.deltaTime
        );
    }
}
