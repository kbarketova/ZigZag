using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    [Header("Platform road settings")]
    public GameObject prefab;
    public Transform startPoint;

    public int roadLenght = 10;

    public float secondsBetweenSpawning = 0.1f;

    [Header("Loot")]
    public GameObject lootPrefab;
    public int lootSpawnPercent;

    private List<GameObject> _road;
    private Vector3 _prefabScale;
    private Vector3 _lootPrefabScale;
    private float _nextSpawnTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        lootSpawnPercent = 100 / lootSpawnPercent;
        _prefabScale = prefab.transform.localScale;

        if (startPoint == null)
        {
            Debug.LogWarning("Start point is missing");
            startPoint = Instantiate(gameObject, Vector3.zero, Quaternion.identity).transform;
        }

        InitiateRoad(startPoint);
        _nextSpawnTime = Time.time + secondsBetweenSpawning;
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            CreatePlatform();

            DestroyPlatform();
            _nextSpawnTime = Time.time + secondsBetweenSpawning;
        }
    }

    private void InitiateRoad(Transform start)
    {
        var firstPlatform = Instantiate(prefab, start.position, Quaternion.identity);
        _road = new List<GameObject> { firstPlatform };

        for (int i = 0; i < roadLenght * .5f; i++)
        {
            CreatePlatform();
        }
    }

    private void CreatePlatform()
    {
        var current = _road.Last();
        var forward = Mathf.Floor(UnityEngine.Random.Range(1, 1001)) % 2 == 0;

        var position = current.transform.position;

        if (forward)
        {
            position = position + Vector3.forward * _prefabScale.z;
        }
        else
        {
            position = position + Vector3.right * _prefabScale.x;
        }
        _road.Add(Instantiate(prefab, position, Quaternion.identity));

        SpawnLoot();
    }

    private void DestroyPlatform()
    {
        if (_road.Count > roadLenght)
        {
            var platform = _road.First();
            _road.Remove(platform);

            Destroy(platform);
        }
    }

    public void SpawnLoot()
    {
        var spawnAllowed = Mathf.Floor(Random.Range(1, 101)) % lootSpawnPercent == 0;

        if(spawnAllowed)
        {
            var currentPlatform = _road.Last();
            var position = currentPlatform.transform.position;

            position = position + Vector3.up * lootPrefab.transform.position.y;

            Instantiate(lootPrefab, position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
    }
}
