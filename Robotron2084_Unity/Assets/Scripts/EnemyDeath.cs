using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : Death
{
    public override void death(){
        LevelManager.LevelManagerInstance.RemoveFromEnemyCount();
        Destroy(this.gameObject);
        LevelManager.LevelManagerInstance.HUDCanvasObject.GetComponentInChildren<ScoreText>().AddScore(10);
        Instantiate(Resources.Load("EnemyParticleEmitter"), transform.position, Quaternion.identity);
    }
}
