using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveThreshold = 0.1f;

    public bool IsGrounded;

    private bool wasGrounded;
    private bool footstepsPlaying;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > moveThreshold;

        if (isMoving && IsGrounded)
        {
            if (!footstepsPlaying)
            {
                SoundManager.Instance.StartFootsteps();
                footstepsPlaying = true;
            }
        }
        else
        {
            if (footstepsPlaying)
            {
                SoundManager.Instance.StopFootsteps();
                footstepsPlaying = false;
            }
        }

        if (wasGrounded && !IsGrounded)
        {
            SoundManager.Instance.PlayJump();
        }

        wasGrounded = IsGrounded;
    }

    public void PlayNormalDeath()
    {
        SoundManager.Instance.PlayDeath();
        SoundManager.Instance.StopFootsteps();
    }

    public void PlayTrapDeath()
    {
        SoundManager.Instance.PlayTrapDeath();
        SoundManager.Instance.StopFootsteps();
    }
}