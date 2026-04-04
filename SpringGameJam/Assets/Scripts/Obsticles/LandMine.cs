using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LandMine : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f; // Радиус взрыва
    [SerializeField] private float _explosionForce = 500f; // Сила взрыва
    [SerializeField] private LayerMask _layersToAffect; // Слои, которые затронет взрыв

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            Explode();
        }
    }

    public void Explode()
    {
        // Находим все коллайдеры в радиусе взрыва на указанных слоях
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _layersToAffect);

        foreach (Collider2D obj in objectsInRange)
        {
            // Проверяем, есть ли у объекта Rigidbody2D
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Вычисляем направление от центра взрыва к объекту
                Vector2 direction = (obj.transform.position - transform.position).normalized;

                // Добавляем импульс
                // Сила умножается на "массу", чтобы эффект был одинаковым для легких и тяжелых объектов
                rb.AddForce(direction * _explosionForce * rb.mass, ForceMode2D.Impulse);
            }
        }
    }

    // Этот метод поможет тебе визуализировать радиус взрыва в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
