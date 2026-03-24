using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    [Header("префабы бонусов")]
    public GameObject[] bonusPrefabs;

    [Header("настройки спавна")]
    public Transform player;
    public float spawnDistance = 20f;
    public float spawnInterval = 10f;
    public float[] lanes = new float[] { -2f, 0f, 2f };

    private float nextSpawnTime;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnBonus();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnBonus()
    {
        if (bonusPrefabs.Length == 0) return;

        int typeIndex = Random.Range(0, bonusPrefabs.Length);
        int laneIndex = Random.Range(0, lanes.Length);
        float xPos = lanes[laneIndex];

        Vector3 spawnPos = new Vector3(xPos, 0.5f, player.position.z + spawnDistance);
        Instantiate(bonusPrefabs[typeIndex], spawnPos, Quaternion.identity);
    }
}