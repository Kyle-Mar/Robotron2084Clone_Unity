using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletMovement : MonoBehaviour
{
    public float speed = 10;
    public Vector3 bulletVelocity;
    Vector3 initDirection;
    Rigidbody rb;
    public Vector3 playerVelocity;


    void Start()
    {
        initDirection = transform.forward;
        //Debug.Log(initDirection);
        rb = GetComponent<Rigidbody>();
        transform.Rotate(new Vector3(90, 0, 0));
        bulletVelocity = (initDirection) * (speed + playerVelocity.magnitude);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = bulletVelocity;
    }

    public void setInstantaneousOwnerVelocity(Vector3 vel)
    {
        playerVelocity = vel;
    }
}
