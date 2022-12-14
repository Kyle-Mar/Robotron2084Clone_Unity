using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class LevelManager : Singleton<LevelManager>
{

    private static GameObject musicPlayerInstance;

    public static GameObject MusicPlayerInstance
    {
        get { return musicPlayerInstance ?? (musicPlayerInstance = Instantiate(Resources.Load("MusicPlayer") as GameObject)); }
    }


    // list to keep track of all enemies
    public int enemyCount = 0;
    public GameObject PlayerObject = null;
    public float PreviousHealth = 100;
    public int PreviousScore = 0;
    public GameObject MusicPlayer = null;
    public GameObject HUDCanvasObject = null;
    private Coroutine changingLevels = null;
    private List<string> scenes = new List<string>();


    // Pause input action
    public bool gameIsPaused = false;
    InputAction pauseAction = new InputAction("pause", binding: "<Keyboard>/escape");
    Scene pauseScene;

    int currentSceneNumber = 0;
    protected override void OnAwake()
    {

        int numScenes = SceneManager.sceneCountInBuildSettings;
        for(int i = 0; i < numScenes; i++)
        {
            scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)));
        }
        
        // enable the pause input action
        pauseAction.Enable();

        //Get active scene and set the scene number based off of the position in the scene list.
        Scene currentScene = SceneManager.GetActiveScene();
        currentSceneNumber = findSceneNumberBySceneNameInScenesList(currentScene.name, ref scenes);

        InitializeMusic();
        
        //subscribe to the active scene changed delegate
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemyCount);
        if (pauseAction.triggered)
        {
            DoPauseOrUnpause();
        }
    }
    public void Init()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void InitializeMusic()
    {
        LevelManager.Instance.MusicPlayer = MusicPlayerInstance;
        LevelManager.Instance.SetMixerGroupVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        LevelManager.Instance.MusicPlayer.GetComponent<MusicPlayer>().SetPitch(0.5f);
        DontDestroyOnLoad(LevelManager.Instance.MusicPlayer);
    }

    private void InitializeHud()
    {
        Debug.Log(SceneManager.GetSceneByName("HUD").isLoaded);
        if (SceneManager.GetSceneByName("HUD").isLoaded){
            return;
        }
        SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
        StartCoroutine(WaitForHud());
    }

    private void DeinitalizeSingletons()
    {
        Destroy(PlayerObject);
        Destroy(LevelManager.Instance.HUDCanvasObject);
    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        LevelManager.Instance.HUDCanvasObject.GetComponentInChildren<ScoreText>().SetScore(LevelManager.Instance.PreviousScore);
        DoUnpause();
        enemyCount = 0;
        ResetPlayer();
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        currentSceneNumber = next.buildIndex;
        if (next.name.Contains("Level")){
            if (PlayerObject){
                PlayerObject.transform.position = Vector3.zero;
            }
            else{
                PlayerObject = Instantiate(Resources.Load<GameObject>("Player"), Vector3.zero, Quaternion.identity);
                DontDestroyOnLoad(PlayerObject);
            }
            InitializeHud();
        }
    }

    public void DoPauseOrUnpause()
    {
        LevelManager.Instance.gameIsPaused = !LevelManager.Instance.gameIsPaused;
        if (LevelManager.Instance.gameIsPaused)
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
        LevelManager.Instance.gameIsPaused = true;
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        Time.timeScale = 0f;
    }

    public void DoUnpause() {
        LevelManager.Instance.gameIsPaused = false;
        SceneManager.UnloadScene("PauseMenu");
        Time.timeScale = 1f;
    }
    
    public void ResetPlayer()
    {
        PlayerObject.GetComponent<Health>().setHealth(PreviousHealth); 
        LevelManager.Instance.PlayerObject.transform.position = Vector3.zero;

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
        LevelManager.Instance.MusicPlayer.GetComponent<MusicPlayer>().mixer.SetFloat(targetMixerGroup, volumeLevel);
    }

    private void GetAllEnemies()
    {
        LevelManager.Instance.enemyCount = GameObject.FindGameObjectsWithTag("Enemy").ToList().Count;
    }

    public void AddToEnemyCount(GameObject enemy)
    {
        LevelManager.Instance.enemyCount += 1;
    }

    public void RemoveFromEnemyCount()
    {
        LevelManager.Instance.enemyCount -= 1;
        if(enemyCount <= 0 && changingLevels == null)
        {
            changingLevels = StartCoroutine(GoToNextLevel());
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        enemyCount = 0;
        DeinitalizeSingletons();
    }

    public void LoadVictoryMenu()
    {
        SceneManager.LoadScene("VictoryMenu", LoadSceneMode.Single);
        enemyCount = 0;
        DeinitalizeSingletons();
    }

    public void LoadGameOverMenu()
    {
        SceneManager.LoadScene("GameOverMenu", LoadSceneMode.Single);
        enemyCount = 0;
        DeinitalizeSingletons();
    }

    public void StartGame()
    {
        StartCoroutine(GoToNextLevel());
    }

    private int findSceneNumberBySceneNameInScenesList(string sceneName, ref List<string> scenes)
    {
        int i = 0;
        foreach (string scene in scenes)
        {
            if (scene.Contains(sceneName))
            {
                return i;
            }
            i++;
        }
        return -1;
    }




    IEnumerator WaitForHud()
    {

        // if i was making an actual game i'd probably pass a function pointer (i don't know if that exists in C#) 
        // and a scene name, then wait for that scene to load using a general Coroutine
        while (!SceneManager.GetSceneByName("HUD").isLoaded)
        {
            yield return null;
        }
        
        LevelManager.Instance.HUDCanvasObject = GameObject.Find("HUDCanvas");
        Debug.Log(HUDCanvasObject);
        Canvas canvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
        Camera camera = GameObject.Find("Camera").GetComponent<Camera>();
        canvas.worldCamera = camera;
        LevelManager.Instance.HUDCanvasObject.GetComponentInChildren<ScoreText>().SetScore(LevelManager.Instance.PreviousScore);
    }

    IEnumerator GoToNextLevel()
    {
        AsyncOperation op;
        int nextScene = currentSceneNumber + 1;
        if (nextScene > scenes.Count)
        {
            yield break;
        }
        if (scenes[nextScene].Contains("MainMenu"))
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            DeinitalizeSingletons();
        }
        else if (scenes[nextScene].Contains("Level"))
        {
            op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
            while (!op.isDone)
            {
                yield return null;
            }
            changingLevels = null;
            LevelManager.Instance.PreviousScore = LevelManager.Instance.HUDCanvasObject.GetComponentInChildren<ScoreText>().GetScore();
            PreviousHealth = PlayerObject.GetComponent<Health>().getHealth();
        }
        else {
            SceneManager.LoadScene("VictoryMenu", LoadSceneMode.Single);
            DeinitalizeSingletons();
        }
    }
}
