using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerInitializer : MonoBehaviour
{
    void Awake()
    {
        LevelManager.Instance.Init();
    }
}
