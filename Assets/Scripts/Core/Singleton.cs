using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance 
    {
        get
        {            
            if (_programPowerOn == false)
            {
                return null;
            }
            else if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject newObj = new GameObject();
                    newObj.name = typeof(T).Name;
                    _instance = newObj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    private const int NOT_VALUE = -1;
    private static int _nowSceneNumber = NOT_VALUE;
    private static bool _programPowerOn = true;
    protected bool _initialized = false;

    protected void Awake() {        
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else 
        {
            if (_instance != this) 
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (_initialized)
        {
            OnGameQuit();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
            RunOnlyOnce_Initialize();
            Initialize();
    }

    protected virtual void RunOnlyOnce_Initialize()
    {
        if (_initialized == false) 
        {
            _initialized = true;
            Scene active = SceneManager.GetActiveScene();
            _nowSceneNumber = active.buildIndex;
        }
    }

    protected virtual void Initialize()
    {

    }

    private void OnGameQuit() 
    {
        _programPowerOn = false;
    }


}
