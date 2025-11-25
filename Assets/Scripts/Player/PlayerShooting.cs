using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float weaponOffset = 0.7f;


    [SerializeField] private float baseDamage = 5f;
    [SerializeField] private float baseFireRate = 2f;

    private PlayerStats stats;

    private Camera mainCamera;

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
        stats = GetComponent<PlayerStats>();
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
            // 1. Calcular Velocidad de Ataque Real
            // Base (2) * Multiplicador de Destreza (ej. 1.2)
            float attackSpeedMult = stats.GetTotalValue(StatType.AttackSpeedMult);
            // Protegemos para que no sea 0
            if (attackSpeedMult <= 0) attackSpeedMult = 1;

            float realFireRate = baseFireRate * attackSpeedMult;

            nextFireTime = Time.time + (1f / realFireRate);

            // 2. Calcular Daño Real
            // Base (5) * Multiplicador de Fuerza o Destreza (según clase)
            // Por ahora usaremos RangedDamageMult (DEX)
            float damageMult = stats.GetTotalValue(StatType.RangedDamageMult); // Asumiendo que es proyectil
            int totalDamage = Mathf.RoundToInt(baseDamage * (1 + damageMult)); // 1 + 0.1 (10%)

            // 3. Spawnear
            Projectile projectile = ProjectilePool.Instance.Get();

            Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
            projectile.transform.position = spawnPosition;

            // 4. Inicializar con DAÑO DINÁMICO
            projectile.Initialize(aimDirection, totalDamage);
        }
    }
}