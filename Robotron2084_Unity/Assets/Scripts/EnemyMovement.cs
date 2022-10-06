using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    GameObject player;
    GameObject meshObject;
    Vector3 currentDirection;
    [SerializeField] Vector3 currentRandomDirection;
    Rigidbody rb;
    bool isCollidingWithPlayer;
    bool isMovingTowardsPlayer;
    float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        meshObject = transform.Find("EnemyMesh").gameObject;
        StartCoroutine(ChooseNewDirection(5f));
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPositionOnEnemyPlane = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        if (Vector3.Distance(transform.position, playerPositionOnEnemyPlane) < 10.0f)
        {
            isMovingTowardsPlayer = true;
            currentDirection = playerPositionOnEnemyPlane;
            meshObject.transform.LookAt(currentDirection);
        }
        else
        {
            isMovingTowardsPlayer = false;
            currentDirection = currentRandomDirection;
        }

        if (!isCollidingWithPlayer)
        {
            rb.velocity = meshObject.transform.forward * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == player.name)
        {
            isCollidingWithPlayer = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == player.name)
        {
            isCollidingWithPlayer = false;
        }
    }

    IEnumerator ChooseNewDirection(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            currentRandomDirection = new Vector3(meshObject.transform.position.x + Random.Range(-2, 2), 0, meshObject.transform.position.z + Random.Range(-2,2));
            if (!isMovingTowardsPlayer)
            {
                meshObject.transform.LookAt(currentRandomDirection);
            }
            Debug.Log("EVENT!");

        }
    }

}
