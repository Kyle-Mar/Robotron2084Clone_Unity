using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

    void Start()
    {

    }

    public virtual void death()
    {
        Destroy(this.gameObject);
    }
}
