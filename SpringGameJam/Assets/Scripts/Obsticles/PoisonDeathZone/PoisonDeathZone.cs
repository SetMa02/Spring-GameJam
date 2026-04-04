using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonDeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.Die();
        }
        else if (other.gameObject.TryGetComponent(out Corps corps))
        {
            gameObject.SetActive(false);
        }
        
    }
}
