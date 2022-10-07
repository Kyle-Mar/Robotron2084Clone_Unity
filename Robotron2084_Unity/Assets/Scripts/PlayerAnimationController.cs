using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    PlayerMovement playerMovement;
    Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponentInParent(typeof(PlayerMovement)) as PlayerMovement;
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        playerAnimator.SetBool("Walking", playerMovement.isWalking);
    }
}
