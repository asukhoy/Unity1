using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleData data;

    private Transform player;
    public float destroyDistance = 5f;

    public void Start()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = data.color;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;
    }

    public void Update()
    {
        if (player != null && transform.position.z < player.position.z - destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CubeScript playerScript = other.GetComponent<CubeScript>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(data.damage);
            }
            Destroy(gameObject);
        }
    }
}