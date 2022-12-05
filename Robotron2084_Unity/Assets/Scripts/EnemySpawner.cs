using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
	public float distanceFromSpawner = 10;
	public int enemiesPerWave = 10;
	public int numWaves = 10;
	public float timeBetweenWaves = 5; 
	[SerializeField] float currentAngle;
	[SerializeField] int currentWave = 0;
	float deltaAngle;
	private int outstandingEnemySpawns;
	Timer waveTimer;


	enum Enemies
    {
		WALKING,
		SHOOTING,
		EXPLODING
	}

	// Enemy Prefab GameObject
	GameObject WalkingEnemy;
	GameObject ShootingEnemy;
	GameObject ParticleEffect;
	Death death;

    // Start is called before the first frame update
    void Start()
    {

		death = GetComponent<Death>();
		WalkingEnemy = Resources.Load<GameObject>("WalkingEnemy");
		ShootingEnemy = Resources.Load<GameObject>("ShootingEnemy");
		ParticleEffect = Resources.Load<GameObject>("EnemySpawningParticleEmitter");


		deltaAngle = 360 / enemiesPerWave;
		SpawnWave();
		
		waveTimer = gameObject.AddComponent<Timer>() as Timer;
		waveTimer.SetTimer(timeBetweenWaves, () => { SpawnWave(); }, true);
    }

    // Update is called once per frame
    void Update()
    {
		
		if (currentWave >= numWaves && outstandingEnemySpawns <= 0)
		{
			death.death();
		}
	}

	void SpawnEnemy()
    {
		currentAngle += deltaAngle;

		if (currentAngle > 360)
        {
			currentAngle = currentAngle - 360;
        }

		Vector3 currentAngleVector = Vector3.forward;
		currentAngleVector = Quaternion.Euler(0, currentAngle , 0) * currentAngleVector * distanceFromSpawner;
		Debug.DrawRay(transform.position, currentAngleVector, Color.yellow, .5f);
		Vector3 spawnPosition = transform.position + currentAngleVector;
		spawnPosition.y = 0;

		Instantiate( ParticleEffect, spawnPosition, Quaternion.identity);
		
		Timer particleEffectTimer = gameObject.AddComponent<Timer>() as Timer;
		particleEffectTimer.SetTimer(1.0f, () => { SpawnEnemyCallback(spawnPosition); }, false);
		outstandingEnemySpawns++;
    }

	public void SpawnEnemyCallback(Vector3 spawnPosition)
    {
		int rand = Random.Range(0, 3);
        switch (rand)
        {
			case 0:
				Instantiate(WalkingEnemy, spawnPosition, Quaternion.identity);
				break;
			case 1:
				Instantiate(ShootingEnemy, spawnPosition, Quaternion.identity);
				break;
			case 2:
				Instantiate(WalkingEnemy, spawnPosition, Quaternion.identity);
				break;
			default:
				break;
        }
		outstandingEnemySpawns--;
    }


	void SpawnWave()
    {
		for (int i = 0; i < enemiesPerWave; i++)
        {
			SpawnEnemy();
        }
		currentWave += 1;
	}
}