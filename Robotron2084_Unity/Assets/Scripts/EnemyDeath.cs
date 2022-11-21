using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : Death
{
    public override void death(){
        if(this != null)
        {
            LevelManager.Instance.RemoveFromEnemyCount();
            Destroy(this.gameObject);
            LevelManager.Instance.HUDCanvasObject.GetComponentInChildren<ScoreText>().AddScore(10);
            Instantiate(Resources.Load("EnemyParticleEmitter"), transform.position, Quaternion.identity);
        }
    }
}
