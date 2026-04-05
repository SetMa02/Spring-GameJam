using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GLTFast.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CutsceneManager : MonoBehaviour
{
   public List<GameObject> sceneObjects = new List<GameObject>();

   private void Update()
   {
       if (Input.GetKeyDown(KeyCode.Space))
       {
           GameObject gameObject = sceneObjects.First();
           gameObject.SetActive(false);
           sceneObjects.Remove(gameObject);
           if (sceneObjects.Count == 0)
           {
               SceneManager.LoadScene(2);
           }
       }
   }
}
