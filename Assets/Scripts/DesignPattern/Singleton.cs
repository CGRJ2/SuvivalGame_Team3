using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance != null)
                    DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    public static void CreateInstance()
    {
        if (_instance == null)
        {
            T prefab = Resources.Load<T>($"SingleTons/{typeof(T).Name}");
            _instance = Instantiate(prefab);
            DontDestroyOnLoad(_instance.gameObject);
        }
    }


    protected void SingletonInit()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
            DontDestroyOnLoad(_instance);
        }
    }
}
