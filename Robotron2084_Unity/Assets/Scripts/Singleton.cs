using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//courtesy of https://gamedev.stackexchange.com/a/151547 with a few tweaks to make it more extensible


public abstract class Singleton<T> : Singleton where T: MonoBehaviour
{

    #region Fields 
    private static T _instance;

    private static readonly object Lock = new object();

    [SerializeField]
    private bool _persistent = true;
    #endregion

    #region Properties


    public static T Instance
    {
        get
        {
            if (quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)} <{typeof(T)}> Instance will not be returned because application is quitting.]"); 
                return null;
            }
            // prevent multiple accesses at same time
            lock (Lock){
                
                // if there is an instance return it
                if(_instance != null)
                {
                    return _instance;
                }
                var instances = FindObjectsOfType<T>();
                var count = instances.Length;
                
                if(count > 0)
                {
                    // if there is one instance 
                    if(count == 1)
                    {
                        return _instance = instances[0];
                    }
                    // if more than one instance
                    else
                    {
                        // enfore rules of singleton
                        Debug.LogWarning($"[{nameof(Singleton)} <{typeof(T)}> There should never be more than one {nameof(Singleton)} of type <{typeof(T)}>" +
                            $" but {count} were found. The first instance will be used and all others will be destroyed.");
                        for(var i = 1; i < instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }
                        return _instance = instances[0];
                    }
                }

                // if there are no instances
                Debug.Log($"[{nameof(Singleton)} <{typeof(T)}>] An ins  tance is needed in the scene and no existing instance instances were found, so a new instance will be created.");

                //make a new instance

                try
                {
                    // attempt to load an object from resources
                    return _instance = Instantiate(Resources.Load(typeof(T).Name) as GameObject).AddComponent<T>();
                }
                catch
                {
                    // otherwise create it as a gameobject
                    Debug.LogWarning($"[({nameof(Singleton)}) <{typeof(T)}> was not found in Resources folder. Falling back to contructing as GameObject]");
                    return _instance = new GameObject($"({nameof(Singleton)}){typeof(T)}").AddComponent<T>();
                }

            
            }
        }
    }
    #endregion

    #region Methods

    private void Awake()
    {
        if (_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }
        OnAwake();
    }

    protected virtual void OnAwake() { }

    #endregion

}

public abstract class Singleton : MonoBehaviour
{
    #region Properties
    public static bool quitting { get; private set; }
    #endregion

    #region Methods

    private void OnApplicationQuit()
    {
        quitting = true;
    }
    #endregion
}
//Here be dragons! 
