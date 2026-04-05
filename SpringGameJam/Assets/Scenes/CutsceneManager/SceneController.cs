using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Не забудь подключить библиотеку для TextMeshPro

public class SlideController : MonoBehaviour
{
    // Компоненты, которые мы будем контролировать
    [SerializeField] private UnityEngine.UI.Image _slideImage;
    [SerializeField] private TextMeshProUGUI _slideText;

    [Header("Typewriter Settings")]
    [SerializeField] private float _letterDelay = 0.05f; // Задержка между каждой буквой

    /// <summary>
    /// Вызывается из CutsceneManager, чтобы показать слайд с эффектом печатной машинки.
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
        
        // Запускаем корутину, которая сначала покажет картинку, а потом текст
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        // 1. Сразу показываем картинку
        if (_slideImage != null)
        {
            _slideImage.gameObject.SetActive(true);
        }

        // 2. Скрываем текст на старте и начинаем печатать
        if (_slideText != null)
        {
            _slideText.gameObject.SetActive(true);
            yield return StartCoroutine(TypewriterEffect(_slideText));
        }
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI textComponent)
    {
        // Получаем исходный текст
        string fullText = textComponent.text;
        // Очищаем текстовое поле
        textComponent.text = "";

        // Постепенно добавляем по одной букве
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(_letterDelay);
        }
    }
}