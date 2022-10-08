using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    private static LevelManager levelManagerInstance;

    public static LevelManager LevelManagerInstance
    {
        get { return levelManagerInstance ?? (levelManagerInstance = new GameObject("LevelManager").AddComponent<LevelManager>()); }
    }

    private static GameObject playerInstance;

    public static GameObject PlayerInstance
    {
        get { return playerInstance ?? (playerInstance = Instantiate(Resources.Load("Player") as GameObject)); }
    }


    public List<GameObject> enemies = new List<GameObject>();
    public GameObject PlayerObject = null;

    int currentScene = 0;
    void Awake()
    {
        levelManagerInstance = this;
        LevelManagerInstance.PlayerObject = PlayerInstance;
        LevelManagerInstance.PlayerObject.name = "Player";
        DontDestroyOnLoad(LevelManagerInstance.PlayerObject);
        DontDestroyOnLoad(this.gameObject);
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count <= 0) {
            GoToNextLevel();
        }
    }

    void GoToNextLevel()
    {
        if(currentScene < SceneManager.sceneCountInBuildSettings)
        {
            currentScene = 1;
        }
        SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        GetAllEnemies();
    }
    
    private void GetAllEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
    }

    public void AddToEnemiesList(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveFromEnemiesList(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
