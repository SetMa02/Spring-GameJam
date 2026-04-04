using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FireDathZone : MonoBehaviour
{
    public bool IsPlayerInDeathZone = false;
    public Player Player;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Player = player;
            IsPlayerInDeathZone = true;
            Debug.Log("player entered");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Player = player;
            IsPlayerInDeathZone = false;
            Debug.Log("player left");
        }
    }
}
