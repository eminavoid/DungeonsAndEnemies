using UnityEngine;
using UnityEngine.InputSystem; // Necesario

public class PlayerShooting : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 10f;

    // --- Referencias ---
    private Camera mainCamera;
    private ProjectilePool projectilePool;
    private PlayerInput playerInput; // <--- NUEVA REFERENCIA
    private InputAction fireAction;  // <--- Para guardar la acción y no buscarla cada frame

    // --- Estado ---
    private Vector2 lookInput; // Este lo mantenemos por mensaje, el mouse siempre se mueve
    private Vector2 aimDirection = Vector2.up;
    private float nextFireTime = 0f;

    private void Awake() // Usamos Awake para inicializar referencias propias
    {
        playerInput = GetComponent<PlayerInput>();
        // Cacheamos la acción "Fire" para optimizar
        fireAction = playerInput.actions["Fire"];
    }

    private void Start()
    {
        mainCamera = Camera.main;
        projectilePool = ProjectilePool.Instance;
    }

    // --- Mantenemos OnLook (para la posición del mouse está bien el mensaje) ---
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    // --- BORRA EL MÉTODO OnFire ---
    // Ya no lo necesitamos.

    private void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(lookInput);
        mouseWorldPos.z = 0;
        aimDirection = (mouseWorldPos - transform.position).normalized;

        if (firePoint != null)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void HandleShooting()
    {
        // AQUÍ ESTÁ LA MAGIA:
        // Preguntamos directamente a la acción: "¿Estás presionada?"
        bool isFiring = fireAction.IsPressed();

        if (isFiring && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);

            GameObject projectileGO = projectilePool.Get();

            Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
            projectileGO.transform.position = spawnPosition;

            Projectile projectileScript = projectileGO.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(projectilePool, aimDirection);
            }
        }
    }
}