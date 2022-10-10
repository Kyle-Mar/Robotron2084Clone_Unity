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

	// Enemy Prefab GameObject
	GameObject Enemy;
	Death death;

    // Start is called before the first frame update
    void Start()
    {
		death = GetComponent<Death>();
		Enemy = Resources.Load<GameObject>("Enemy");
		StartCoroutine(WaveTimer(timeBetweenWaves));
    }

    // Update is called once per frame
    void Update()
    {
		deltaAngle = 360 / enemiesPerWave;
		if (currentWave >= numWaves)
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
		Instantiate(Enemy, spawnPosition, Quaternion.identity);
    }

	void SpawnWave()
    {
		for (int i = 0; i < enemiesPerWave; i++)
        {
			SpawnEnemy();
        }
		currentWave += 1;

	}


	IEnumerator WaveTimer (float time)
    {
        while (true)
        {
			yield return new WaitForSeconds(time);
			SpawnWave();
        }
    }
}