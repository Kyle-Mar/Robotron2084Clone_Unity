using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public void OnClick()
    {
        LevelManager.Instance.DoUnpause();
    }
}
