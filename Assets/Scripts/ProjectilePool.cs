using System.Collections.Generic;
using UnityEngine;

// Responsabilidad: Gestionar el ciclo de vida de los proyectiles (crear, entregar, reciclar).
public class ProjectilePool : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; // El prefab que creamos en el Paso 2.A

    [SerializeField]
    private int initialPoolSize = 100; // Empezamos con 100 proyectiles listos

    // Usamos una Cola (Queue) para almacenar los proyectiles inactivos.
    private Queue<GameObject> pool = new Queue<GameObject>();

    // Hacemos un Singleton estático simple para que el PlayerShooting pueda encontrarlo.
    // (Hay patrones más avanzados, pero esto es limpio y efectivo para empezar).
    public static ProjectilePool Instance { get; private set; }

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // --- El corazón del Pool ---
        // Pre-calentamos el pool: creamos los objetos DE ANTEMANO.
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false); // Los guardamos inactivos
            pool.Enqueue(projectile);
        }
    }

    /// <summary>
    /// Pide un proyectil al pool.
    /// </summary>
    public GameObject Get()
    {
        // Si tenemos objetos en el pool...
        if (pool.Count > 0)
        {
            GameObject projectile = pool.Dequeue(); // Sacar uno de la cola
            // (El script Projectile.Initialize() se encargará de activarlo)
            return projectile;
        }

        // Si el pool está vacío (¡mal! necesitamos más proyectiles)
        // Creamos uno nuevo "al vuelo". Esto es más lento (causa GC)
        // pero evita que el juego se rompa.
        Debug.LogWarning("El Pool de Proyectiles está vacío. Aumenta el initialPoolSize.");
        GameObject newProjectile = Instantiate(projectilePrefab);
        // Este proyectil no pasa por el pool al ser creado,
        // pero se añadirá cuando llame a Release().
        return newProjectile;
    }

    /// <summary>
    /// Devuelve un proyectil al pool.
    /// </summary>
    public void Release(GameObject projectile)
    {
        projectile.SetActive(false); // Lo desactivamos
        pool.Enqueue(projectile);    // Lo guardamos de nuevo en la cola
    }
}