using UnityEngine;
using UnityEngine.InputSystem;

// Responsabilidad: Orquestar el disparo (cadencia, input) y apuntado.
public class PlayerShooting : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    [SerializeField]
    private Transform firePoint; // El punto desde donde nacen los proyectiles

    [SerializeField]
    [Tooltip("Disparos por segundo")]
    private float fireRate = 10f; // 10 disparos por segundo

    // --- Referencias Internas ---
    private Camera mainCamera;
    private ProjectilePool projectilePool; // Nuestra dependencia (DIP)

    // --- Estado Interno ---
    private Vector2 lookInput;
    private Vector2 aimDirection = Vector2.up; // Dirección de apuntado (default)
    private bool isFiring = false;
    private float nextFireTime = 0f;

    private void Start()
    {
        // Cachear referencias es clave para el rendimiento.
        // Evita Camera.main en Update() y usa el Singleton del Pool.
        mainCamera = Camera.main;

        // Inyección de Dependencia simple usando el Singleton
        projectilePool = ProjectilePool.Instance;

        if (projectilePool == null)
        {
            Debug.LogError("¡No se encontró ProjectilePool! Asegúrate de que existe en la escena.");
        }
    }

    // --- Métodos de Input (Llamados por Player Input "Send Messages") ---

    // Esta función será llamada por el evento "Performed" (presionar)
    public void OnFireStart()
    {
        isFiring = true;
    }

    // Esta función será llamada por el evento "Canceled" (soltar)
    public void OnFireStop()
    {
        isFiring = false;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    // --- Lógica Principal ---

    private void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        // 1. Convertir la posición del mouse (Screen Space) a (World Space)
        // Usamos ScreenToWorldPoint.
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane)
        );

        // (En 2D, a menudo es más simple si la cámara es Ortográfica,
        // pero esto funciona para ambas. Aseguramos Z=0)
        // ¡Corrección! Para 2D Ortográfico es más simple:
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(lookInput);
        mouseWorldPos.z = 0; // Estamos en 2D

        // 2. Calcular la dirección desde el jugador hacia el mouse
        aimDirection = (mouseWorldPos - transform.position).normalized;

        // Opcional: Rotar el "firePoint" para que mire hacia el mouse
        if (firePoint != null)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void HandleShooting()
    {
        // Si el jugador está disparando Y ha pasado el tiempo de espera
        if (isFiring && Time.time >= nextFireTime)
        {
            // 1. Actualizar el tiempo para el próximo disparo
            nextFireTime = Time.time + (1f / fireRate);

            // 2. Determinar la posición de spawn
            Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

            // 3. ¡Pedir un proyectil al Pool! (Cero 'Instantiate')
            GameObject projectileGO = projectilePool.Get();

            // 4. Posicionar el proyectil
            projectileGO.transform.position = spawnPosition;

            // 5. Inicializar el proyectil (decirle a dónde ir)
            // Esto usa GetComponent, lo cual es aceptable aquí porque
            // solo ocurre al disparar, no cada frame.
            Projectile projectileScript = projectileGO.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(projectilePool, aimDirection);
            }
            else
            {
                Debug.LogError("¡El Prefab del proyectil no tiene el script 'Projectile'!");
            }
        }
    }
}