using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (Application.isEditor)
                _instance = FindObjectOfType<T>();
            
            if (_instance == null)
                CreateInstance();
            return _instance;
        }
    }

    private void Awake()
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