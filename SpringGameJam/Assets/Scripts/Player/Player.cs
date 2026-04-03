using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField]private float _jumpForce;

    private bool _platformDetector;
    private Vector3 _direction;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Movement();
        StartJump();
    }

    private void Movement()
    {
        _direction = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            _direction = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _direction = Vector3.right;
        }
        
        _direction *= _speed;
        _direction.y = _rb.velocity.y;
        _rb.velocity = _direction;
    }
    
    private void StartJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _platformDetector == true)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Platforms platform))
        {
            _platformDetector = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Platforms platform))
        {
            _platformDetector = false;
        }
    }
}
