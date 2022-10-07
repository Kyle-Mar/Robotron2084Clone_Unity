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

    // Start is called before the first frame update
    void Start()
    {
		Enemy = Resources.Load<GameObject>("Enemy");
		StartCoroutine(WaveTimer(timeBetweenWaves));
    }

    // Update is called once per frame
    void Update()
    {
		deltaAngle = 360 / enemiesPerWave;
    }

	void SpawnEnemy()
    {
		currentAngle += deltaAngle;

		if (currentAngle > 360)
        {
			currentAngle = currentAngle - 360;
        }

		Vector3 currentAngleVector = Vector3.forward;
		Debug.Log(Vector3.forward);
		currentAngleVector = Quaternion.Euler(0, currentAngle , 0) * currentAngleVector * distanceFromSpawner;
		Debug.DrawRay(transform.position, currentAngleVector, Color.yellow, .5f);

		Vector3 spawnPosition = transform.position + currentAngleVector;
		spawnPosition.y = 0;
		Instantiate(Enemy, spawnPosition, Quaternion.identity);

		if( currentWave >= numWaves)
        {
			Destroy(this.gameObject);
        }
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
/*
 * extends "res://Scripts/enemy.gd"

export var enemy_scene = preload("res://Object Scenes/enemy.tscn")
export var distance_from_spawner:float = 10 
export var enemies_per_wave:int = 10
export var current_angle = 0
export var num_waves = 10

var delta_angle = 360/enemies_per_wave
var num_enemies = 0
var current_wave = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func spawn_enemy():
	var new_enemy = enemy_scene.instance()
	get_tree().get_root().get_node("world_root").add_child(new_enemy)
	
	if current_angle > 360:
		current_angle = current_angle - 360
	
	#rotation vector
	var v = Vector3.FORWARD.rotated(Vector3.UP, current_angle) * distance_from_spawner + transform.origin
	# vector rotates around the spawner and then spawns the enemy at the current spot.
	new_enemy.transform.origin = Vector3(v.x, transform.origin.y, v.z)
	current_angle += delta_angle

	if(current_wave >= num_waves):
		queue_free()
	

func spawn_wave():
	for i in range(enemies_per_wave):
		spawn_enemy()
	current_wave += 1
		


func _on_Timer_timeout():
	spawn_wave() # Replace with function body.

*/