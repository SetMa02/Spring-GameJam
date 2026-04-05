using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Статическая ссылка на экземпляр класса (паттерн Singleton)
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    // Один AudioSource для звуков, которые не нужно прерывать (шаги, подбор предметов)
    [SerializeField] private AudioSource _sfxSource; 
    
    // Второй AudioSource для важных звуков, которые должны играть сами (музыка, фоновые шумы)
    [SerializeField] private AudioSource _musicSource; 

    [Header("Audio Clips")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip trapDeathClip;
    // ... добавь сюда любые другие звуки, которые понадобятся

    private void Awake()
    {
        // Стандартная реализация Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            // Делаем так, чтобы объект с AudioManager не уничтожался при загрузке новой сцены
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // --- ПУБЛИЧНЫЕ МЕТОДЫ ДЛЯ ВЫЗОВА ИЗ ЛЮБОГО СКРИПТА ---

    public void PlayFootstepSound()
    {
        PlaySound(footstepClip);
    }

    public void PlayJumpSound()
    {
        PlaySound(jumpClip);
    }

    public void PlayDeathSound()
    {
        PlaySound(deathClip);
    }

    public void PlayTrapDeathSound()
    {
        PlaySound(trapDeathClip);
    }

    // Универсальный метод для воспроизведения любого звука
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }

    // Если захочешь управлять музыкой отдельно
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip != null)
        {
            _musicSource.clip = musicClip;
            _musicSource.Play();
        }
    }
}