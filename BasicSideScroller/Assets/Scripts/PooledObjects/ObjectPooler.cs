using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on code from Brackeys for ObjectPooling (https://www.youtube.com/watch?v=tdSmKaJvCoA&list=PLmg4VwU2gNYjqu4xdmOe_3WqPyCDCvvug)
// Modified to be a static class for more efficiency (Courtesy of Nocturne's comment on YouTube in Brackeys video)
public static class ObjectPooler
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static List<Pool> pools;

    public static Dictionary<(GameObject, string), (GameObject, Queue<GameObject>)> poolDictionary;     // Key = Tuple(Owner of Pool, objectPool Type) ; Value = Tuple(Object Prefab, objectPoolQueue)

    //public static bool Initialized {get; private set;}

    /*
    // Have a script run this initialization on their Awake method
    // EDIT: This may not be necessary. AddPool may just be called by each script that needs their own pool of objects.
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
    }*/

    // This is where I'm straying away from both Brackeys and Notcturne's implementation. 
    // Method to be called to add a new a pool of objects. Should be called in Awake method of script using a pooled object.
    public static void AddPool(GameObject owner, string pooledObjectType, GameObject prefab, int size)
    {
        // If this is the first time AddPool is called, that means poolDictionary isn't instantiated yet.
        if (poolDictionary == null)
            poolDictionary = new Dictionary<(GameObject, string), (GameObject, Queue<GameObject>)>();

        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Object.Instantiate(prefab, new Vector3(1000, 1000, 0), Quaternion.identity);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    
        (GameObject, string) poolKey = (owner, pooledObjectType);

        poolDictionary.Add(poolKey, (prefab, objectPool));
    }

    // public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    // {

    //     // Change this to a try-catch
    //     if (!poolDictionary.ContainsKey(tag))
    //     {
    //         Debug.LogWarning("Pool with tag " + tag + "doesn't exist.");
    //         return null;
    //     }

    //     GameObject objectToSpawn = poolDictionary[tag].Dequeue();

    //     objectToSpawn.SetActive(true);
    //     objectToSpawn.transform.position = position;
    //     objectToSpawn.transform.rotation = rotation;

    //     // Ineed to create an IPooledObject interface for this part to work
    //     /*
    //     IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();  // Can be more performant by getting rid of GetComponent call

    //     if (pooledObj != null)
    //     {
    //         pooledObj.OnObjectSpawn();
    //     }*/

    //     poolDictionary[tag].Enqueue(objectToSpawn);

    //     return objectToSpawn;
    // }

    public static GameObject SpawnFromPool(GameObject owner, string pooledObjectType, Vector3 position, Quaternion rotation)
    {

        // Change this to a try-catch
        if (!poolDictionary.ContainsKey((owner, pooledObjectType)))
        {
            Debug.LogWarning("Pool with tag (" + owner.name + ", " + pooledObjectType + ") doesn't exist.");
            return null;
        }
        
        if (poolDictionary[(owner, pooledObjectType)].Item2.Count == 0)
        {
            GameObject newObject = Object.Instantiate(poolDictionary[(owner, pooledObjectType)].Item1, new Vector3(1000, 1000, 0), Quaternion.identity);
            newObject.SetActive(false);
            poolDictionary[(owner, pooledObjectType)].Item2.Enqueue(newObject);
        }

        GameObject objectToSpawn = poolDictionary[(owner, pooledObjectType)].Item2.Dequeue();

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

        //oolDictionary[(owner, pooledObjectType)].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public static void ReturnToPool(GameObject owner, string pooledObjectType, GameObject pooledObject)
    {
        poolDictionary[(owner, pooledObjectType)].Item2.Enqueue(pooledObject);
    }

}
