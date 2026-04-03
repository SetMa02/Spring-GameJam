using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 8f;
    [SerializeField] private float _acceleration = 50f;
    [SerializeField] private float _deceleration = 40f;
    [SerializeField] private float _airControlPercent = 0.5f;
    
    [SerializeField] private float _jumpForce = 15f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _isGrounded = false; 
    private float _horizontalInput;
    private int _platformsContactCount = 0; 

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter(_horizontalInput);
        UpdateAnimations(_horizontalInput);
    }

    private void MoveCharacter(float horizontal)
    {
        float actualAcceleration = _isGrounded ? _acceleration : _acceleration * _airControlPercent;
        float actualDeceleration = _isGrounded ? _deceleration : _deceleration * _airControlPercent;

        if (horizontal != 0)
        {
            _rb.AddForce(new Vector2(horizontal * actualAcceleration, 0));
            
            if (Mathf.Abs(_rb.velocity.x) > _maxSpeed)
            {
                _rb.velocity = new Vector2(_maxSpeed * Mathf.Sign(_rb.velocity.x), _rb.velocity.y);
            }
        }
        else
        {
            if (Mathf.Abs(_rb.velocity.x) > 0.01f)
            {
                _rb.AddForce(new Vector2(-_rb.velocity.x * actualDeceleration, 0));
            }
            else if (Mathf.Abs(_rb.velocity.x) < 0.01f)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }
    }

    private void UpdateAnimations(float horizontal)
    {
        if (horizontal > 0.01f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (horizontal < -0.01f)
        {
            _spriteRenderer.flipX = true;
        }


    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Platforms>() != null)
        {
            _platformsContactCount++;
            if (_platformsContactCount > 0)
            {
                _isGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Platforms>() != null)
        {
            _platformsContactCount--;
            if (_platformsContactCount <= 0)
            {
                _isGrounded = false;
                _platformsContactCount = 0;
            }
        }
    }

    public void Die()
    {
        Debug.Log("Player died");

        Destroy(gameObject);
    }

}