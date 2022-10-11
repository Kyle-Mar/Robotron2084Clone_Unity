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

    private static GameObject musicPlayerInstance;

    public static GameObject MusicPlayerInstance
    {
        get { return musicPlayerInstance ?? (musicPlayerInstance = Instantiate(Resources.Load("MusicPlayer") as GameObject)); }
    }


    // list to keep track of all enemies
    public int enemyCount = 0;
    public GameObject PlayerObject = null;
    public GameObject PreviousPlayerObject = null;
    public int PreviousScore = 0;
    public GameObject MusicPlayer = null;
    public GameObject HUDCanvasObject = null;
    private Coroutine changingLevels = null;
    private List<string> scenes = new List<string>();


    // Pause input action
    public bool gameIsPaused = false;
    InputAction pauseAction = new InputAction("pause", binding: "<Keyboard>/escape");
    Scene pauseScene;

    int currentScene = 0;
    void Awake()
    {
        levelManagerInstance = this;
        int numScenes = SceneManager.sceneCountInBuildSettings;
        for(int i = 0; i < numScenes; i++)
        {
            scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)));
        }
        
        // enable the pause input action
        pauseAction.Enable();
        InitializeMusic();
        
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
    }
    public void Init()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void InitializePlayer()
    {
        LevelManagerInstance.PlayerObject = PlayerInstance;
        LevelManagerInstance.PlayerObject.name = "Player";
        DontDestroyOnLoad(LevelManagerInstance.PlayerObject);
    }

    private void InitializeMusic()
    {
        LevelManagerInstance.MusicPlayer = MusicPlayerInstance;
        LevelManagerInstance.SetMixerGroupVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        LevelManagerInstance.MusicPlayer.GetComponent<MusicPlayer>().SetPitch(0.5f);
    }

    private void InitializeHud()
    {
        SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
        StartCoroutine(WaitForHud());
        DontDestroyOnLoad(LevelManagerInstance.MusicPlayer);
    }

    private void DeinitalizeSingletons()
    {
        Destroy(LevelManagerInstance.PlayerObject);
        playerInstance = null;
        Destroy(LevelManagerInstance.HUDCanvasObject);

    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        LevelManagerInstance.HUDCanvasObject.GetComponentInChildren<ScoreText>().SetScore(LevelManagerInstance.PreviousScore);
        DoUnpause();
        enemyCount = 0;
        ResetPlayer();
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        if (next.name.Contains("Level")){
            GetAllEnemies();
        }
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
        if(!LevelManagerInstance.PreviousPlayerObject == null)
        {
            LevelManagerInstance.PlayerObject = LevelManagerInstance.PreviousPlayerObject;
        }
        LevelManagerInstance.PlayerObject.transform.position = Vector3.zero;

    }
    
    public void SetMixerGroupVolume(string targetMixerGroup, float volumeLevel)
    {
        // Mathf.Log10(volumeLevel) * 20
        // Logarthmic sound courtesy of https://johnleonardfrench.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/

        // make sure it's not infinity
        if (Mathf.Log10(volumeLevel) * 20 < -80)
        {
            volumeLevel = -80;
        }
        else
        {
            volumeLevel = Mathf.Log10(volumeLevel) * 20;
        }
        LevelManagerInstance.MusicPlayer.GetComponent<MusicPlayer>().mixer.SetFloat(targetMixerGroup, volumeLevel);
    }

    private void GetAllEnemies()
    {
        //LevelManagerInstance.enemyCount = GameObject.FindGameObjectsWithTag("Enemy").ToList().Count;
    }

    public void AddToEnemyCount(GameObject enemy)
    {
        LevelManagerInstance.enemyCount += 1;
    }

    public void RemoveFromEnemyCount()
    {
        LevelManagerInstance.enemyCount -= 1;
        if(enemyCount <= 0 && changingLevels == null)
        {
            changingLevels = StartCoroutine(GoToNextLevel());
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        DeinitalizeSingletons();
    }

    public void LoadVictoryMenu()
    {
        SceneManager.LoadScene("VictoryMenu", LoadSceneMode.Single);
        DeinitalizeSingletons();
    }

    public void LoadGameOverMenu()
    {
        SceneManager.LoadScene("GameOverMenu", LoadSceneMode.Single);
        DeinitalizeSingletons();
    }

    public void StartGame()
    {
        InitializePlayer();
        InitializeHud();
        StartCoroutine(GoToNextLevel());
    }

    public void RestartGame()
    {
        currentScene = -1;
        StartCoroutine(GoToNextLevel());
    }

    IEnumerator WaitForHud()
    {

        // if i was making an actual game i'd probably pass a function pointer (i don't know if that exists in C#) 
        // and a scene name, then wait for that scene to load using a general Coroutine
        while (!SceneManager.GetSceneByName("HUD").isLoaded)
        {
            yield return null;
        }
        
        LevelManagerInstance.HUDCanvasObject = GameObject.Find("HUDCanvas");
        Canvas canvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
        Camera camera = GameObject.Find("Camera").GetComponent<Camera>();
        canvas.worldCamera = camera;
        DontDestroyOnLoad(canvas.transform.gameObject);  
      
    }

    IEnumerator GoToNextLevel()
    {
        AsyncOperation op;
        int nextScene = currentScene + 1;
        if (nextScene > scenes.Count)
        {
            yield break;
        }
        if (scenes[nextScene].Contains("MainMenu"))
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            DeinitalizeSingletons();

            currentScene = nextScene;
        }
        else if (scenes[nextScene].Contains("Level"))
        {
            op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
            LevelManagerInstance.PreviousPlayerObject = LevelManagerInstance.PlayerObject;
            while (!op.isDone)
            {
                yield return null;
            }
            changingLevels = null;
            LevelManagerInstance.PreviousScore = LevelManagerInstance.HUDCanvasObject.GetComponentInChildren<ScoreText>().GetScore();
            currentScene = nextScene;
        }
        else {
            SceneManager.LoadScene("VictoryMenu", LoadSceneMode.Single);
            DeinitalizeSingletons();
            currentScene = scenes.Count;
        }
    }
}
