using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : Death
{
    public override void death(){
        Destroy(this.gameObject);
        Instantiate(Resources.Load("EnemyParticleEmitter"), transform.position, Quaternion.identity);
    }
}
