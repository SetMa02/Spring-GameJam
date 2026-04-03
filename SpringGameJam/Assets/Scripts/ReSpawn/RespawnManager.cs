using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Respawn(Transform player)
    {
        player.position = spawnPoint.position;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
