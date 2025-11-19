using System;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour, IDamageable
{
    private EnemyData myData;
    private int currentHealth;
    private Transform playerTarget;

    public event Action<SimpleEnemy> OnEnemyDeath;

    public void Configure(EnemyData data)
    {
        myData = data;

        currentHealth = myData.maxHealth;

        GetComponent<SpriteRenderer>().color = myData.color;
        transform.localScale = Vector3.one * myData.scale;
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTarget = player.transform;
    }

    private void Update()
    {
        if (playerTarget != null && myData != null)
        {
            Vector3 direction = (playerTarget.position - transform.position).normalized;
            transform.Translate(direction * myData.moveSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (DamagePopupPool.Instance != null)
        {
            DamagePopupPool.Create(transform.position, damage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 1. Avisar a la sala (si alguien está escuchando)
        OnEnemyDeath?.Invoke(this);

        // 2. Devolver al Pool (Ya no soltamos gemas físicas)
        EnemyPool.Instance.Release(this);
    }
}