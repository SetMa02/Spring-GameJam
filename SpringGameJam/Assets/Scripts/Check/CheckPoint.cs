using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CheckPoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null && !isActivated)
        {
            RespawnManager.Instance.SetSpawnPoint(transform);
            isActivated = true;

            Debug.Log("Checkpoint activated");
        }
    }
}
