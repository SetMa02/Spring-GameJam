using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MenuManager : MonoBehaviour
{
   public void MainMenuPlay()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

   public void QuitGame()
   {
      Debug.Log("Quit");
      Application.Quit();
   }
}
