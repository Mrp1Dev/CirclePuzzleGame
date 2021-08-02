using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [ClearOnReload] public static T Instance { get; protected set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"A singleton of type: {typeof(T).Name} already exists. Destroying this Instance.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this as T;
        }
    }
}