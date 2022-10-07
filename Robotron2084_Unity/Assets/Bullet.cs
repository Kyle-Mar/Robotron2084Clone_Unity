using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{

    MeshRenderer meshRenderer;
    TrailRenderer trailRenderer;
    BulletMovement bulletMovement;
    Collider col;
    Rigidbody rb;
    void Start()
    {
        
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        bulletMovement = GetComponent<BulletMovement>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Debug.Log(trailRenderer.positionCount);

    }

    void destroyBullet()
    {
        // disable the Meshrenderer to stop rendering the object
        meshRenderer.enabled = false;
        // set velocity to zero
        rb.velocity = Vector3.zero;
        // prevent bulletmovement from updating the velocity
        bulletMovement.bulletVelocity = Vector3.zero;
        // disable the collider
        col.enabled = false;
        // wait to delete the trail for the time that it will take for the trail to disappear.
        StartCoroutine(WaitForTrailThenDelete(trailRenderer.time));

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            destroyBullet();
        }
    }

    IEnumerator WaitForTrailThenDelete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
