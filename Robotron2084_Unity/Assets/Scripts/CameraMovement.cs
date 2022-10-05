using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    Vector3 originalPos;
    Quaternion originalRot;
    Vector3 degreeAndDirectionOfMovement;
    [SerializeField] GameObject player;
    PlayerMovement playerScript;
    Transform playerTransform;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerMovement>();
        playerTransform = player.GetComponent<Transform>();
        originalPos = transform.position;
        originalRot = transform.rotation;
        degreeAndDirectionOfMovement = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //convert player movement into vector3
        degreeAndDirectionOfMovement += playerTransform.forward;
        degreeAndDirectionOfMovement.x = playerScript.moveInput.x + transform.position.x;
        degreeAndDirectionOfMovement.z = playerScript.moveInput.y + transform.position.z;

        degreeAndDirectionOfMovement.y = originalPos.y;


        Vector3 playerPositionOnCameraY = playerTransform.position;
        playerPositionOnCameraY.y = originalPos.y;

        if (playerScript.moveInput == Vector2.zero)
        {

            Vector3 interpolatedPosition = Vector3.Lerp(transform.position, playerPositionOnCameraY, Time.deltaTime);
            transform.position = interpolatedPosition;
        }
        else
        {

            float cameraToPlayerDistance = Vector3.Distance(playerPositionOnCameraY, transform.position);

            Vector3 interpolatedPosition = Vector3.Lerp(transform.position, degreeAndDirectionOfMovement, Time.deltaTime);
            if(cameraToPlayerDistance < 1.5f)
            {
                transform.position = interpolatedPosition;
            }
        }

        //interpolate between the current position and the direction of the movement

        transform.rotation = originalRot;
    }
}
