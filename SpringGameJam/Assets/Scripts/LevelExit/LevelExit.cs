using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private float delay = 2f;

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player == null)
            player = collision.GetComponentInParent<Player>();

        if (player != null && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(CompleteLevel());
        }
    }

    private IEnumerator CompleteLevel()
    {
        // показываем UI
        if (completePanel != null)
            completePanel.SetActive(true);

        // останавливаем игру
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(delay);

        // возвращаем время
        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }
}