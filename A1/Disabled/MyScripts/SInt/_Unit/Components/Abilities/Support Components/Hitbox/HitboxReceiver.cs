 
using UnityEngine;

public class HitboxReceiver : MonoBehaviour // Hitbox receiver
{
    public void Hit(HitData data)
    {
        Debug.Log("Hit by AtkID: " + data.id);
        //gameObject.SetActive(false); // Temp
    }
}
