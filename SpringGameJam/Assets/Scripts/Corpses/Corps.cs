using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corps : MonoBehaviour
{
    [SerializeField] private Sprite _normalCorpse;
    [SerializeField] private Sprite _drownCorpse;
    [SerializeField] private Sprite _burnedCorpse;
    [SerializeField]private Sprite _bouncyCorpse;
    
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer =  GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out WaterDeathZone waterDeathZone))
        {
            _spriteRenderer.sprite = _drownCorpse;
        }
        else if (other.gameObject.TryGetComponent(out FireDathZone fireDathZone))
        {
            _spriteRenderer.sprite = _burnedCorpse;
        }
        else if (other.gameObject.TryGetComponent(out PoisonDeathZone poisonDeathZone))
        {
            _spriteRenderer.sprite = _bouncyCorpse;
        }
        
        else
        {
            _spriteRenderer.sprite = _normalCorpse;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && _spriteRenderer.sprite == _bouncyCorpse)
        {
            player.IsOnPoisonedCorpse();
        }
        
    }

   
}
