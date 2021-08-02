using System;
using System.Collections.Generic;
using UnityEngine; //Using aliases to make the script easier to understand
using Pool = System.Collections.Generic.Queue<UnityEngine.GameObject>;
using Prefab = UnityEngine.GameObject;

[DefaultExecutionOrder(-1)]
public class PoolingManager : MonoBehaviour
{
    [SerializeField] private Transform inactiveObjectsParent;
    [SerializeField] private List<PrewarmPoolPair> poolsToPrewarm;

    private readonly Dictionary<Prefab, Pool> pools = new Dictionary<Prefab, Pool>();

    //A map from spawned game objects to their pools
    private readonly Dictionary<GameObject, Pool> spawnedObjectsPools = new Dictionary<GameObject, Pool>();
    public static PoolingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("An Instanace of this singleton already exists, destroying current one.");
            Destroy(this);
        }

        for (var i = poolsToPrewarm.Count - 1; i >= 0; i--)
        {
            var pair = poolsToPrewarm[i];
            Prewarm(pair.prefab, pair.count);
            poolsToPrewarm.RemoveAt(i);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private Pool CreatePool(Prefab prefab)
    {
        var newPool = new Pool();
        pools.Add(prefab, newPool);
        return newPool;
    }

    public GameObject GetFromPool(Prefab prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (pools.TryGetValue(prefab, out var pool) == false)
            pool = CreatePool(prefab);
        GameObject result;
        if (pool.Count == 0)
        {
            result = Instantiate(prefab, position, rotation, parent);
        }
        else
        {
            result = pool.Dequeue();
            result.transform.parent = parent;
            result.transform.position = position;
            result.transform.rotation = rotation;
            result.SetActive(true);
        }

        spawnedObjectsPools.Add(result, pool);
        return result;
    }

    public GameObject GetFromPool(Prefab prefab, Transform parent, bool instantiateInWorldSpace = false)
    {
        if (pools.TryGetValue(prefab, out var pool) == false)
            pool = CreatePool(prefab);
        GameObject result;
        if (pool.Count == 0)
        {
            result = Instantiate(prefab, parent, instantiateInWorldSpace);
        }
        else
        {
            result = pool.Dequeue();
            result.transform.parent = parent;
            if (instantiateInWorldSpace)
            {
                result.transform.position = prefab.transform.position;
                result.transform.rotation = prefab.transform.rotation;
            }
            else
            {
                result.transform.localPosition = prefab.transform.localPosition;
                result.transform.localRotation = prefab.transform.localRotation;
            }

            result.SetActive(true);
        }

        spawnedObjectsPools.Add(result, pool);
        return result;
    }

    public GameObject GetFromPool(Prefab prefab) =>
        GetFromPool(prefab, prefab.transform.position, prefab.transform.rotation, null);

    public GameObject GetFromPool(Prefab prefab, Vector3 position, Quaternion rotation) =>
        GetFromPool(prefab, position, rotation, null);

    public void ReturnToPool(GameObject obj)
    {
        if (spawnedObjectsPools.TryGetValue(obj, out var pool) == false)
        {
            Destroy(obj); //not a pooled object
            return;
        }

        obj.SetActive(false);
        obj.transform.parent = inactiveObjectsParent;
        pool.Enqueue(obj);
        spawnedObjectsPools.Remove(obj);
    }

    public void Prewarm(Prefab prefab, int count)
    {
        var temporaryObjects = new List<GameObject>();
        for (var i = 0; i < count; i++) temporaryObjects.Add(GetFromPool(prefab));
        foreach (var go in temporaryObjects) ReturnToPool(go);
    }

    public void ShrinkPool(Prefab prefab, int count)
    {
        if (pools.TryGetValue(prefab, out var pool) == false) return;
        for (var i = 0; i < count; i++)
            if (pool.Count > 0)
                Destroy(pool.Dequeue());
            else
                break;
    }

    public void DeletePool(Prefab prefab)
    {
        if (pools.TryGetValue(prefab, out var pool) == false) return;
        while (pool.Count > 0) Destroy(pool.Dequeue());
        pools.Remove(prefab);
    }

    [Serializable]
    private struct PrewarmPoolPair
    {
        public Prefab prefab;
        public int count;
    }
}