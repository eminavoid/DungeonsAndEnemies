using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 10f;
    [SerializeField] private float weaponOffset = 0.7f;

    // --- Referencias ---
    private Camera mainCamera;
    // ProjectilePool ya no necesita ser cacheado necesariamente porque es singleton estático,
    // pero mantenerlo aquí está bien si prefieres acceso local.

    private PlayerInput playerInput;
    private InputAction fireAction;

    // --- Estado ---
    private Vector2 lookInput;
    private Vector2 aimDirection = Vector2.up;
    private float nextFireTime = 0f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions["Fire"];
    }

    private void Start()
    {
        mainCamera = Camera.main;
        // No necesitamos buscar el ProjectilePool aquí, accederemos vía Instance
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

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
            firePoint.position = transform.position + (Vector3)(aimDirection * weaponOffset);
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void HandleShooting()
    {
        bool isFiring = fireAction.IsPressed();

        if (isFiring && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);

            // --- CAMBIO PRINCIPAL AQUÍ ---
            // 1. Obtenemos directamente el componente Projectile (No GameObject)
            Projectile projectile = ProjectilePool.Instance.Get();

            // 2. Posicionamos usando el transform del componente
            Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
            projectile.transform.position = spawnPosition;

            // 3. Inicializamos (Ya no hay GetComponent, es mucho más rápido)
            projectile.Initialize(aimDirection);
        }
    }
}