using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerFireBullet))]
public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveInput = new Vector2(0, 0);
    public Vector2 lookInput = new Vector2(0, 0);
    public bool isWalking;
    float fireInput = 0;
    bool doesAimNeedRecalculated = false;
    float speed = 15;
    Ray playerAim;
    Camera mainCamera;
    GameObject meshObject;
    PlayerFireBullet playerFireBullet;
    Rigidbody rb;
    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>(); 
        playerFireBullet = GetComponent<PlayerFireBullet>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        meshObject = GameObject.Find("PlayerMesh");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * speed);
        rb.velocity = (new Vector3(moveInput.x, 0, moveInput.y) * speed);
        isWalking = rb.velocity.magnitude == 0 ? false : true;
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (LevelManager.LevelManagerInstance.gameIsPaused)
        {
            return;
        }
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        if(playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            doesAimNeedRecalculated = true;
            playerAim = mainCamera.ScreenPointToRay(mousePos);

            RaycastHit hit;
            LayerMask notPlayer = ~(LayerMask.GetMask("Player"));
            if (Physics.Raycast(playerAim.origin, playerAim.direction, out hit, 100, notPlayer))
            {
                // TODO: do something if hit.point doesn't exist 
                Vector3 hitPosition = hit.point;
                hitPosition.y = meshObject.transform.position.y;
                meshObject.transform.LookAt(hitPosition);
                //Debug.Log(meshObject.transform.forward);
                //Debug.DrawLine(hitPosition, transform.forward * 100, Color.yellow, 4);
            }
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (LevelManager.Instance.gameIsPaused)
        {
            return;
        }
        fireInput = context.ReadValue<float>();
        if (fireInput > .5f && context.performed)
        {
            playerFireBullet.fireBullet(meshObject.transform.rotation, rb.velocity);
        }

    }
}
