using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using System.Linq; // Он нам больше не нужен, так как мы не используем .First()

public class CutsceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public List<GameObject> sceneObjects = new List<GameObject>();
    public CanvasGroup FadeGroup;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.0f;

    private bool _isTransitioning = false;
    private int _currentSlideIndex = 0; // Используем индекс вместо удаления из списка

    private void Start()
    {
        // Показываем первый слайд при старте
        ChangeSlideContent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isTransitioning)
        {
            StartCoroutine(NextSlideRoutine());
        }
    }

    private IEnumerator NextSlideRoutine()
    {
        _isTransitioning = true;

        // --- ШАГ 1: FADE OUT (затемнение экрана) ---
        yield return StartCoroutine(FadeCanvasGroup(FadeGroup, 1f));

        // --- ШАГ 2: СМЕНА СЛАЙДА ---
        ChangeSlideContent();

        // --- ШАГ 3: FADE IN (проявление экрана) ---
        yield return StartCoroutine(FadeCanvasGroup(FadeGroup, 0f));

        _isTransitioning = false;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
    
    // --- ИЗМЕНЕННЫЙ МЕТОД ---
    private void ChangeSlideContent()
    {
        // Если слайды закончились, загружаем новую сцену
        if (_currentSlideIndex >= sceneObjects.Count)
        {
            Debug.Log("Cutscene finished. Loading next scene.");
            SceneManager.LoadScene(2);
            return;
        }
        
        // Сначала выключаем ВСЕ слайды на всякий случай
        foreach (var slide in sceneObjects)
        {
            slide.SetActive(false);
        }

        // Теперь берем нужный слайд по индексу и активируем его
        GameObject currentSlideObject = sceneObjects[_currentSlideIndex];
        currentSlideObject.SetActive(true);

        // --- НОВАЯ ЛОГИКА: Пытаемся найти SlideController и запустить его ---
        SlideController slideController = currentSlideObject.GetComponent<SlideController>();

        if (slideController != null)
        {
            // Если скрипт есть, вызываем его метод Show()
            slideController.Show();
        }
        else
        {
            // Если скрипта нет, слайд просто включится (картинка и текст будут видны сразу)
            Debug.LogWarning($"На слайде '{currentSlideObject.name}' нет компонента SlideController. Текст появится мгновенно.");
        }
        
        // Увеличиваем индекс для следующего перехода
        _currentSlideIndex++;
    }
}