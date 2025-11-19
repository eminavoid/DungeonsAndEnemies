using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;

    private Vector2 moveDirection;
    private float lifeTimer;

    private int damageToDeal;

    // Ya no necesitamos pasar la referencia del pool en Initialize, 
    // usaremos el Singleton estático al morir.
    public void Initialize(Vector2 direction, int damage)
    {
        this.moveDirection = direction.normalized;
        this.lifeTimer = lifeTime;
        this.damageToDeal = damage;
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) ReleaseToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Usamos el valor dinámico
            damageable.TakeDamage(damageToDeal);
            ReleaseToPool();
        }
    }

    private void ReleaseToPool()
    {
        // Llamada limpia y tipada
        ProjectilePool.Instance.Release(this);
    }
}