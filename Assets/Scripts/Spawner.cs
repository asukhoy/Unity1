using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    public Transform player;
    public float spawnDistance = 30f;
    public float spawnInterval = 0.5f;
    public float[] lanes = new float[] { -2f, 0f, 2f };

    private float nextSpawnTime;

    public void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + spawnInterval;
    }

    public void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    public void SpawnObstacle()
    {

        int typeIndex = Random.Range(0, obstaclePrefabs.Length);
        int laneIndex = Random.Range(0, lanes.Length);
        float xPos = lanes[laneIndex];

        Vector3 spawnPos = new Vector3(xPos, 0, player.position.z + spawnDistance);

        Instantiate(obstaclePrefabs[typeIndex], spawnPos, Quaternion.identity);
    }
}