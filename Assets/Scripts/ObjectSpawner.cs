using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject spawnObject;
    public int minRange = 5;
    public int maxRange = 15;
    private int m_DestroyTime = 8;

    //store the value so maybe when player fails
    //can stop spawning stuff
    private Coroutine m_SpawnRoutine;

    private float m_SpawnRate;

    private List<GameObject> m_SpawnedObjects = new List<GameObject>();

    private void Awake() => m_SpawnRate = UnityEngine.Random.Range(minRange, maxRange);

    private void Start()
    {
        m_SpawnRoutine = StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        while (true)
        {
            SpawnObject();
            m_SpawnRate = UnityEngine.Random.Range(minRange, maxRange);
            yield return new WaitForSeconds(m_SpawnRate);
        }
    }

    private enum PrefabType
    {
        Platform,
        Pipe
    }

    struct SpawnInfo
    {
        public float spawnY;
        public float spawnDelay;
        public int spawnCount;
    }

    [SerializeField] private PrefabType m_SpawnType;

    private SpawnInfo m_SpawnInfo;
    private int m_SpawnAmount = 0;

    public void SpawnObject()
    {   
        switch(m_SpawnType)
        {
            //spawn 1 platform or a random cluster
            case PrefabType.Platform:
                m_SpawnInfo = GenerateRandSpawnInfo(-3, 3);
                InvokeRepeating("InstantiateObject", m_SpawnInfo.spawnDelay, m_SpawnInfo.spawnDelay);
                break;
            case PrefabType.Pipe:
                InstantiateObject();
                break;
        }
    }

    //spawn object type until max is hit
    private void InstantiateObject()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y += RandomYPos(-3, 3);
        GameObject spawnedObject = Instantiate(spawnObject, spawnPos, transform.rotation);
        m_SpawnedObjects.Add(spawnedObject);
        Invoke("DestroyObject", m_DestroyTime);
        m_SpawnAmount++;

        if (m_SpawnAmount >= m_SpawnInfo.spawnCount)
        {
            m_SpawnAmount = 0;
            CancelInvoke("InstantiateObject");
        }
            
    }

    private SpawnInfo GenerateRandSpawnInfo(float minY, float maxY)
    {
        SpawnInfo info;
        info.spawnY = UnityEngine.Random.Range(minY, maxY);
        info.spawnDelay = UnityEngine.Random.Range(1f, 2f);
        info.spawnCount = UnityEngine.Random.Range(1, 4);
        return info;
    }

    private float RandomYPos(float minY, float maxY)
    {
        return UnityEngine.Random.Range(minY, maxY);
    }

    private void DestroyObject()
    {
        if(m_SpawnedObjects.Count > 0 && m_SpawnedObjects.First<GameObject>() != null)
        {
            Destroy(m_SpawnedObjects.First<GameObject>());
            m_SpawnedObjects.RemoveAt(0);
        }
    }
}
