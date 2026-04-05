using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CutsceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public List<GameObject> sceneObjects = new List<GameObject>();
    public CanvasGroup FadeGroup;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.0f;

    // НОВЫЙ ФЛАГ ДЛЯ БЛОКИРОВКИ
    private bool _isTransitioning = false;

    private void Start()
    {
        if (sceneObjects.Count > 0)
        {
            sceneObjects[0].SetActive(true);
            for (int i = 1; i < sceneObjects.Count; i++)
            {
                sceneObjects[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        // НОВОЕ УСЛОВИЕ: Проверяем флаг перед тем, как реагировать на нажатие
        if (Input.GetKeyDown(KeyCode.Space) && !_isTransitioning)
        {
            StartCoroutine(NextSlideRoutine());
        }
    }

    private IEnumerator NextSlideRoutine()
    {
        // --- НОВОЕ: В самом начале корутины ставим блокировку ---
        _isTransitioning = true;

        // --- ШАГ 1: FADE OUT (затемнение экрана) ---
        yield return StartCoroutine(FadeCanvasGroup(FadeGroup, 1f));

        // --- ШАГ 2: СМЕНА СЛАЙДА ---
        ChangeSlideContent();

        // --- ШАГ 3: FADE IN (проявление экрана) ---
        yield return StartCoroutine(FadeCanvasGroup(FadeGroup, 0f));

        // --- НОВОЕ: В самом конце корутины снимаем блокировку ---
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
    
    private void ChangeSlideContent()
    {
        if (sceneObjects.Count == 0)
        {
            Debug.Log("Cutscene finished. Loading next scene.");
            SceneManager.LoadScene(2);
            return;
        }

        GameObject currentSlide = sceneObjects.First();
        currentSlide.SetActive(false);
        sceneObjects.Remove(currentSlide);

        if (sceneObjects.Count > 0)
        {
            sceneObjects[0].SetActive(true);
        }
    }
}