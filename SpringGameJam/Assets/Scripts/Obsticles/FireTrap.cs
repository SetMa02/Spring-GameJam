using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("Cycle Settings")]
    [SerializeField] private float _activeDuration = 2f; 
    [SerializeField] private float _inactiveDuration = 3f; 

    [Header("Components")]
    [SerializeField] private ParticleSystem _flameEffect; 
    [SerializeField] private FireDathZone _deathZone; 

    private void Start()
    {
       
        if (_flameEffect == null || _deathZone == null)
        {
            Debug.LogError("На FireTrap не назначены все компоненты!");
            return;
        }
       
        StartCoroutine(TrapCycle());
    }

    private IEnumerator TrapCycle()
    {
        while (true) 
        {
            _flameEffect.Play();
            
            float timer = 0f;
            while (timer < _activeDuration)
            {
               
                if (_deathZone.IsPlayerInDeathZone && _deathZone.Player != null)
                {
                    _deathZone.Player.Die();
                  
                    yield break; 
                }
                timer += Time.deltaTime;
                yield return null; 
            }
            
            _flameEffect.Stop();
           
            yield return new WaitForSeconds(_inactiveDuration);
        }
    }
}
