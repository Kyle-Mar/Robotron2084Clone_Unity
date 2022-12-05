using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    GameObject meshObject;
    Vector3 currentDirection;
    [SerializeField] Vector3 currentRandomDirection;
    Rigidbody rb;
    FireBullet enemyFireBullet;
    bool isCollidingWithPlayer;
    bool isMovingTowardsPlayer = false;
    float speed = 5;
    Timer bulletTimer;


    // Start is called before the first frame update
    void Start()
    {
        enemyFireBullet = GetComponent<FireBullet>();
        player = GameObject.FindWithTag("Player");
        meshObject = transform.Find("EnemyMesh").gameObject;
        NewRandomDirection();
        StartCoroutine(ChooseNewDirection(5f));
        rb = GetComponent<Rigidbody>();
        bulletTimer = gameObject.AddComponent<Timer>() as Timer;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPositionOnEnemyPlane = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        var distance_to_player = Vector3.Distance(transform.position, playerPositionOnEnemyPlane);
        
        rb.velocity = meshObject.transform.forward * speed;
        if (distance_to_player < 5.0f)
        {
            bulletTimer.SetActive(false);
            Debug.Log("RUN AWAY!");
            isMovingTowardsPlayer = true;
            meshObject.transform.LookAt((transform.position - playerPositionOnEnemyPlane).normalized + transform.position);
            rb.velocity = (transform.position - playerPositionOnEnemyPlane).normalized * speed;
        }
        else if (10.0f > distance_to_player && distance_to_player >= 5.0f){
            Debug.Log("FIRE!");
            rb.velocity = Vector3.zero;
            isMovingTowardsPlayer = true;
            meshObject.transform.LookAt(playerPositionOnEnemyPlane);
            if (!bulletTimer.active)
            {
                bulletTimer.SetActive(true);
                bulletTimer.SetTimer(1.0f, () => { enemyFireBullet.fireBullet(transform.forward, rb.velocity); }, true);
            }
        } 

        else if (distance_to_player < 20.0f)
        {
            bulletTimer.SetActive(false);
            Debug.Log("IMMA GET YA");
            isMovingTowardsPlayer = true;
            currentDirection = playerPositionOnEnemyPlane;
            meshObject.transform.LookAt(currentDirection);

        }
        else
        {
            bulletTimer.SetActive(true);
            Debug.Log("WHERE?");
            isMovingTowardsPlayer = false;
            currentDirection = currentRandomDirection;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == player.name)
        {
            isCollidingWithPlayer = true;
        }

        if (collision.gameObject.CompareTag("Obstacle") && !isMovingTowardsPlayer)
        {
            currentRandomDirection *= -1;
            meshObject.transform.LookAt(currentRandomDirection);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == player.name)
        {
            isCollidingWithPlayer = false;
        }
    }

    void NewRandomDirection()
    {
        currentRandomDirection = new Vector3(meshObject.transform.position.x + Random.Range(-2, 2), 0, meshObject.transform.position.z + Random.Range(-2, 2));
        if (!isMovingTowardsPlayer)
        {
            meshObject.transform.LookAt(currentRandomDirection);
        }
    }

    IEnumerator ChooseNewDirection(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            NewRandomDirection();
        }
    }
}
