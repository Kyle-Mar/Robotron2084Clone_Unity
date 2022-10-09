using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    // list to keep track of all enemies
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject PlayerObject = null;


    // Pause input action
    public bool gameIsPaused = false;
    InputAction pauseAction = new InputAction("pause", binding: "<Keyboard>/escape");
    Scene pauseScene;

    int currentScene = 0;
    void Awake()
    {
        // set the levelmanagerinstance to this instance.
        levelManagerInstance = this;

        // create a player object instance if it doesn't already exist.
        LevelManagerInstance.PlayerObject = PlayerInstance;
        LevelManagerInstance.PlayerObject.name = "Player";

        // enable the pause input action
        pauseAction.Enable();
        //pauseScene = SceneManager.GetSceneByBuildIndex(2);


        // preserve the levelmanager and the player.
        DontDestroyOnLoad(LevelManagerInstance.PlayerObject);
        DontDestroyOnLoad(this.gameObject);
        

        //subscribe to the active scene changed delegate
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.triggered)
        {
            DoPauseOrUnpause();
        }



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

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        DoUnpause();
        ResetPlayer();
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        GetAllEnemies();
    }

    public void DoPauseOrUnpause()
    {
        LevelManagerInstance.gameIsPaused = !LevelManagerInstance.gameIsPaused;
        if (LevelManagerInstance.gameIsPaused)
        {
            DoPause();
        }
        else
        {
            DoUnpause();
        }
    }


    public void DoPause()
    {
        LevelManagerInstance.gameIsPaused = true;
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        Time.timeScale = 0f;
    }

    public void DoUnpause() {
        LevelManagerInstance.gameIsPaused = false;
        SceneManager.UnloadScene("PauseMenu");
        Time.timeScale = 1f;
    }
    
    public void ResetPlayer()
    {
        LevelManagerInstance.PlayerObject.transform.position = Vector3.zero;
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
