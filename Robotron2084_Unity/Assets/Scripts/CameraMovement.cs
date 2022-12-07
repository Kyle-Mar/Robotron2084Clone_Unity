using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 originalPos;
    Quaternion originalRot;
    [SerializeField] GameObject player;
    PlayerMovement playerScript;
    Transform playerTransform;
    private Vector3 velocity = Vector3.zero;
    public float shakeDuration = 0f;
    public float shakeAmount = .05f;
    public float decreaseAmount = 100f;
    Vector3 playerPositionOnCameraY;


    // Start is called before the first frame update
    void Start()
    {
        Player.OnPlayerHurt += BeginShake;
        playerScript = player.GetComponent<PlayerMovement>();
        playerTransform = player.GetComponent<Transform>();
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Lerp();
        Shake();

    }

    void Lerp()
    {
        //convert player movement into vector3

        // calculate the direction in which the player is moving relative to the player
        Vector3 degreeAndDirectionOfMovement = Vector3.zero;
        degreeAndDirectionOfMovement.x = playerScript.moveInput.x + playerTransform.position.x;
        degreeAndDirectionOfMovement.z = playerScript.moveInput.y + playerTransform.position.z;
        // move camera to the original position so we don't slowly move down
        degreeAndDirectionOfMovement.y = originalPos.y;

        // necessary as well to prevent us from moving down
        playerPositionOnCameraY = playerTransform.position;
        playerPositionOnCameraY.y = originalPos.y;


        // https://www.youtube.com/watch?v=YJB1QnEmlTs Timestamp: t=7:25
        double k = Mathf.Abs(1.0f - Mathf.Pow(10f, Time.deltaTime));

        // if the player isn't moving
        if (playerScript.moveInput == Vector2.zero)
        {
            // move back towards the player
            Vector3 interpolatedPosition = Vector3.Slerp(transform.position, playerPositionOnCameraY, (float)k);
            transform.position = interpolatedPosition;
        }
        // interpolate between the current position and the direction of the movement
        else
        {
            Vector3 interpolatedPosition = Vector3.Lerp(transform.position, degreeAndDirectionOfMovement, (float)k);
            transform.position = interpolatedPosition;
        }
        //transform.rotation = originalRot;
    }

    void Shake()
    {
        if(shakeDuration <= 0f)
        {
            shakeDuration = 0f;
            return;
        }
        Vector2 rand = Random.insideUnitCircle;
        transform.position = playerPositionOnCameraY + new Vector3(rand.x, 0, rand.y) * shakeAmount * shakeDuration;
        shakeDuration -= Time.deltaTime * decreaseAmount;
    }

    void BeginShake()
    {
        
        shakeDuration = .5f;
    }
}
