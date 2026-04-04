using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public static CameraFollow2D Instance;
      public Transform target;
      
      [SerializeField] private float smoothSpeed = 5f;
      [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
  
      
      private void Awake()
      {
          // Singleton
          if (Instance == null)
              Instance = this;
          else
              Destroy(gameObject);
      }
      private void LateUpdate()
      {
          if (target == null)
              return;
  
          Vector3 desiredPosition = target.position + offset;
          Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
          transform.position = smoothedPosition;
      }

}
