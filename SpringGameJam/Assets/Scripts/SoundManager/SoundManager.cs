using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource footstepSource;

    [Header("Clips")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip trapDeathClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayJump()
    {
        if (jumpClip != null)
            sfxSource.PlayOneShot(jumpClip);
    }

    public void PlayDeath()
    {
        if (deathClip != null)
            sfxSource.PlayOneShot(deathClip);
    }

    public void PlayTrapDeath()
    {
        if (trapDeathClip != null)
            sfxSource.PlayOneShot(trapDeathClip);
    }

    public void StartFootsteps()
    {
        if (footstepClip == null || footstepSource == null)
            return;

        if (!footstepSource.isPlaying)
        {
            footstepSource.clip = footstepClip;
            footstepSource.loop = true;
            footstepSource.Play();
        }
    }

    public void StopFootsteps()
    {
        if (footstepSource != null && footstepSource.isPlaying)
            footstepSource.Stop();
    }
}