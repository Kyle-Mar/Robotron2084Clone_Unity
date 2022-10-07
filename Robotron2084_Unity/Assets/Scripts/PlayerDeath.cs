using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : Death
{

    public override void death()
    {
        // Kill player
        Debug.Log("dead");
    }
}
