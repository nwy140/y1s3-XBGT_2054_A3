using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBody2DAutoAddRelativeForceUp : MonoBehaviour
{
    [Header("UnitComp - Motion")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveAcelSpeed = 2.0f;
    public float maxMoveAcelSpeed = 10f;
    public float curAcelSpeed = 0;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintAcelSpeed = 5.335f;
    public float maxSprintAcelSpeed = 20f;


    [Header("Set in Awake")]

    private bool hasRigidbody2D;
    public Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        hasRigidbody2D = TryGetComponent(out _rigidbody2D);
    }
    void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        curAcelSpeed = Mathf.Clamp( MoveAcelSpeed * Time.deltaTime, -maxMoveAcelSpeed, maxMoveAcelSpeed);
        _rigidbody2D.AddRelativeForce(curAcelSpeed * Vector2.up);
    }

}

