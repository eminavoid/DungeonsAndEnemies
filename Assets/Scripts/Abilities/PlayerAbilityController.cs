using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityController : MonoBehaviour
{
    [Header("Habilidades Equipadas")]
    [SerializeField] private Ability activeAbility;

    private float cooldownTimer;
    private bool isOnCooldown = false;

    private PlayerInput playerInput;
    private InputAction abilityAction;

    // Nueva Referencia
    private PlayerStats stats;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        stats = GetComponent<PlayerStats>(); // <--- Agarrar stats

        // Asegúrate de que esta acción existe en tu Input Asset
        abilityAction = playerInput.actions["SecondaryAbility"];
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isOnCooldown = false;
                Debug.Log("Habilidad Lista.");
            }
        }

        if (abilityAction.WasPressedThisFrame() && !isOnCooldown && activeAbility != null)
        {
            UseAbility();
        }
    }

    private void UseAbility()
    {
        // 1. Activar habilidad
        activeAbility.Activate(this.gameObject);

        // 2. Calcular Cooldown usando WISDOM
        // Obtenemos el % de reducción (ej. 10 WIS = 5%)
        float cdrPercentage = stats.GetTotalValue(StatType.CooldownReduction);

        // Matemática: Si el CD es 2s y tienes 10% CDR -> 2 * (1 - 0.10) = 1.8s
        float reducedCooldown = activeAbility.cooldownTime * (1f - (cdrPercentage / 100f));

        // Protegemos para que no sea negativo
        reducedCooldown = Mathf.Max(reducedCooldown, 0.1f);

        // 3. Iniciar Cooldown
        isOnCooldown = true;
        cooldownTimer = reducedCooldown;

        Debug.Log($"Habilidad usada. CD Base: {activeAbility.cooldownTime}s | CD Real (con WIS): {reducedCooldown}s");
    }
}