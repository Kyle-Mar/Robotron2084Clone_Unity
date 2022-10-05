using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletMovement : MonoBehaviour
{
    public float speed = 10;
    Vector3 initDirection;
    Rigidbody rb;
    public Vector3 playerVelocity;


    void Start()
    {
        initDirection = transform.forward;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = (initDirection) * (speed + playerVelocity.magnitude);
        Debug.Log(rb.velocity);
    }

    public void setInstantaneousPlayerVelocity(Vector3 vel)
    {
        playerVelocity = vel;
    }
}
