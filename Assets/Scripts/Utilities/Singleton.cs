using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    private static T _instance;

    
    public static T Instance
    {
        // ReSharper disable Unity.PerformanceAnalysis
        get
        {
            #if UNITY_EDITOR
            if (Application.isEditor)
                _instance = FindObjectOfType<T>();
            #endif
            
            if (_instance == null)
                CreateInstance();
            return _instance;
        }
    }

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
        }
        else
        {
            Destroy(this);
        }
    }

    private static void CreateInstance()
    {
        var gameObject = new GameObject
        {
            name = typeof(T).Name
        };
        _instance = gameObject.AddComponent<T>();
    }
}