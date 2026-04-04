using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public event UnityAction DieInit;
    public event UnityAction LockCorpse;
    
    [SerializeField] private float _maxSpeed = 8f;
    [SerializeField] private float _acceleration = 50f;
    [SerializeField] private float _deceleration = 40f;
    [SerializeField] private float _airControlPercent = 0.5f;
    [SerializeField] private ParticleSystem _bloodSplash;
    
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
    private bool _freezMovement = false;
    
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
            _animator.SetTrigger("JumpInit");
        }
        else if (_isTouchingWall && !_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Стеновой прыжок
            Vector2 wallJumpDirection = new Vector2(-_wallSide * _wallJumpForceX, _wallJumpForceY);
            _rb.velocity = wallJumpDirection;
            _animator.SetTrigger("JumpInit");
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
        
        Suicide();
    }

    private void Suicide()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _animator.SetTrigger("Suicide");
        }
      
    }

    private void MoveCharacter(float horizontal)
    {
        if (_freezMovement == false)
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
    }

    private void UpdateAnimations(float horizontal)
    {
        if (_isTouchingWall && !_isGrounded)
        {
            // _wallSide = -1 для левой стены, 1 для правой
            // Если стена слева (_wallSide == -1), flipX должен быть true (смотрим влево)
            // Если стена справа (_wallSide == 1), flipX должен быть false (смотрим вправо)
            _spriteRenderer.flipX = (_wallSide == -1);
        }
        else // В любом другом случае (на земле, в обычном прыжке)
        {
            // Стандартная логика поворота от ввода игрока
            if (horizontal > 0.01f)
            {
                _spriteRenderer.flipX = false;
            }
            else if (horizontal < -0.01f)
            {
                _spriteRenderer.flipX = true;
            }
        }
    
        // Остальная часть анимаций остается без изменений
        _animator.SetFloat("Speed", Mathf.Abs(_rb.velocity.x));
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetBool("Sliding", _isTouchingWall);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Для земли логика остается прежней
        if (other.gameObject.GetComponent<Platforms>() != null)
        {
            _platformsContactCount++;
            if (_platformsContactCount > 0) _isGrounded = true;
        }
    
        // Для стен мы просто определяем сторону при первом контакте
        if (other.gameObject.GetComponent<Wall>() != null)
        {
            // Определяем, с какой стороны стена
            Vector2 contactPoint = other.contacts[0].point;
            Vector2 playerCenter = transform.position;
            _wallSide = (int)Mathf.Sign(contactPoint.x - playerCenter.x);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        // Эта магия решает проблему со стыками!
        // Выполняется каждый кадр, пока мы на стене.
        if (other.gameObject.GetComponent<Wall>() != null)
        {
            _isTouchingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Для земли логика остается прежней
        if (other.gameObject.GetComponent<Platforms>() != null)
        {
            _platformsContactCount--;
            if (_platformsContactCount <= 0)
            {
                _isGrounded = false;
                _platformsContactCount = 0;
            }
        }

        // Для стен мы сбрасываем флаг только при выходе
        if (other.gameObject.GetComponent<Wall>() != null)
        {
            // Это может сработать некорректно на стыках, но Stay нас спасет.
            // Однако, если у игрока в моменте нет контакта НИ с ОДНОЙ стеной, флаг сбросится.
            _isTouchingWall = false;
            _wallSide = 0;
        }
    }

    public void PlayBlood()
    {
        _bloodSplash.Play();
    }

    public void Die()
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        Debug.Log("Death process started.");

        _freezMovement = true;

        if (_bloodSplash != null)
        {
            _bloodSplash.Play();
        }
        
        yield return new WaitForSeconds(0.2f);
        
        DieInit?.Invoke();
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(3f);
        LockCorpse?.Invoke();
        
        RespawnManager.Instance.Respawn(this.gameObject.transform);
        _spriteRenderer.enabled = true;
        _freezMovement = false;

        Debug.Log("Player respawned.");
    }
    
    
}