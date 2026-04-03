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
    
    [Header("Wall Jump Settings")]
    [SerializeField] private float _wallSlideSpeed = 2f; // Скорость сползания по стене
    [SerializeField] private float _wallJumpForceX = 10f; // Сила отталкивания от стены по горизонтали
    [SerializeField] private float _wallJumpForceY = 12f; // Сила отталкивания от стены по вертикали
    [SerializeField] private float _wallJumpDuration = 0.2f; // Время, на которое блокируется управление после прыжка


    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _isTouchingWall = false; // Касаемся ли мы стены
    private bool _isGrounded = false; 
    private float _horizontalInput;
    private int _wallsContactCount = 0;
    private int _platformsContactCount = 0; 
    
    // Для определения направления стены
    private int _wallSide = 0; // -1 для левой стены, 1 для правой, 0 если нет стены

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
        else if (_isTouchingWall && !_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Стеновой прыжок
            Vector2 wallJumpDirection = new Vector2(-_wallSide * _wallJumpForceX, _wallJumpForceY);
            _rb.velocity = wallJumpDirection;
            // Можно добавить корутину для блокировки управления на короткое время
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter(_horizontalInput);
        UpdateAnimations(_horizontalInput);
        
        if (_isTouchingWall && !_isGrounded && _rb.velocity.y < 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -_wallSlideSpeed);
        }
    }

    private void MoveCharacter(float horizontal)
    {
        float actualAcceleration = _isGrounded ? _acceleration : _acceleration * _airControlPercent;
        float actualDeceleration = _isGrounded ? _deceleration : _deceleration * _airControlPercent;

        if (horizontal != 0)
        {
            if (!(_isTouchingWall && Mathf.Sign(horizontal) == _wallSide))
            {
                _rb.AddForce(new Vector2(horizontal * actualAcceleration, 0));
            }

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
            if (_platformsContactCount > 0) _isGrounded = true;
        }

        if (other.gameObject.GetComponent<Wall>() != null)
        {
            _wallsContactCount++;
            if (_wallsContactCount > 0) _isTouchingWall = true;

            // Определяем, с какой стороны стена
            Vector2 contactPoint = other.contacts[0].point;
            Vector2 playerCenter = transform.position;
            _wallSide = (int)Mathf.Sign(contactPoint.x - playerCenter.x);
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

        if (other.gameObject.GetComponent<Wall>() != null)
        {
            _wallsContactCount--;
            if (_wallsContactCount <= 0)
            {
                _isTouchingWall = false;
                _wallSide = 0;
                _wallsContactCount = 0;
            }
        }
    }

    public void Die()
    {
        Debug.Log("Dead");
    }
}