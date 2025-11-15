using UnityEngine;

// Responsabilidad: Moverse, detectar colisiones y devolverse al pool.
public class Projectile : MonoBehaviour
{
    // --- Configuración ---
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f; // Segundos antes de auto-destruirse

    // --- Estado Interno ---
    private Vector2 moveDirection;
    private float lifeTimer;
    private ProjectilePool myPool; // Referencia al pool que me creó (¡Importante para DIP!)

    /// <summary>
    /// Método de inicialización. Llamado por el Pool o el Disparador.
    /// Esto es mejor que un constructor para MonoBehaviours.
    /// </summary>
    public void Initialize(ProjectilePool pool, Vector2 direction)
    {
        this.myPool = pool;
        this.moveDirection = direction.normalized;
        this.lifeTimer = lifeTime;

        // Activa el proyectil (en caso de que el pool lo entregue desactivado)
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        // 1. Moverse
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // 2. Contar tiempo de vida
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            ReleaseToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Por ahora, cualquier cosa que golpea lo destruye.
        // Más adelante filtraremos por "Tags" (ej. "Enemy")
        // if (other.CompareTag("Enemy")) { ... }

        Debug.Log("Proyectil golpeó a: " + other.name);
        ReleaseToPool();
    }

    /// <summary>
    /// Se desactiva y se devuelve al pool.
    /// </summary>
    private void ReleaseToPool()
    {
        if (myPool != null)
        {
            myPool.Release(this.gameObject);
        }
        else
        {
            // Fallback por si no hay pool (malo para el rendimiento, pero seguro)
            Debug.LogWarning("¡No se encontró pool! Destruyendo objeto.");
            Destroy(gameObject);
        }
    }
}