using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    private static T instance;
    
    public static T Instance
    {
        // ReSharper disable Unity.PerformanceAnalysis
        get
        {
            instance = FindObjectOfType<T>();
            if (instance == null)
                CreateInstance();
            return instance;
        }
    }

    protected void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else if (instance != this)
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
        instance = gameObject.AddComponent<T>();
    }
}