using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Copied code from Brackeys for ObjectPooling.
// Modified to be a static class for more efficiency (Courtesy of Nocturne on YouTube)
public static class ObjectPooler
{
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static List<Pool> pools;

    public static Dictionary<string, Queue<GameObject>> poolDictionary;

    public static bool Initialized {get; private set;}

    // Have oa script run this initialization on their Awake method
    public static void RunttimeInitializeOnLoad()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Object.Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

        Initialized = true;
    }

    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {

        // Change this to a try-catch
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Ineed to create an IPooledObject interface for this part to work
        /*
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();  // Can be more performant by getting rid of GetComponent call

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }*/

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
