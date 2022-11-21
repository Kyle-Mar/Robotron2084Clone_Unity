using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    void Start()
    {
    }
    public void StartGame()
    {
        LevelManager.Instance.StartGame();
    }
}
