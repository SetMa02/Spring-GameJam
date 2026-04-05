using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawObsticle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.Die();
        }
    }
}
