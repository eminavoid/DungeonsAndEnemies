using UnityEngine;

public class SimpleEnemy : MonoBehaviour, IDamageable
{
    private EnemyData myData;
    private int currentHealth;
    private Transform playerTarget;


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

        if (DamagePopupManager.Instance != null)
        {
            DamagePopupManager.Instance.ShowDamage(damage, transform.position);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (ExperienceGemPool.Instance != null)
        {
            // Nota: ExperienceGemPool.Instance.Get() devuelve una ExperienceGem ahora
            var gem = ExperienceGemPool.Instance.Get();
            gem.transform.position = transform.position;
        }

        // Devolverse al pool usando 'this' (el componente), no gameObject
        EnemyPool.Instance.Release(this);
    }
}