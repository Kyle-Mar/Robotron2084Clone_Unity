using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOverButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartOver()
    {
        LevelManager.Instance.LoadMainMenu();
    }
}
