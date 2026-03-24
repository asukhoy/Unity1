using UnityEngine;

public class Bonus : MonoBehaviour
{
    public BonusData data;

    private Transform player;
    public float destroyDistance = 10f;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null && data != null)
            rend.material.color = data.color;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player != null && transform.position.z < player.position.z - destroyDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CubeScript playerScript = other.GetComponent<CubeScript>();
            if (playerScript != null)
                playerScript.ApplyBonus(data);

            Destroy(gameObject);
        }
    }
}