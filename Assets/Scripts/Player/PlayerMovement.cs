using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // YA NO NECESITAMOS ESTO:
    // [SerializeField] private float moveSpeed = 5f; 

    private Vector2 moveInput;

    // Referencia al cerebro de stats
    private PlayerStats stats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        Vector2 moveDirection = moveInput.normalized;

        // --- EL CAMBIO CLAVE ---
        // Le pedimos la velocidad actual a los Stats
        // Esto incluye la base (6) + Items + Buffs
        float currentSpeed = stats.GetTotalValue(StatType.MoveSpeed);

        transform.Translate(moveDirection * currentSpeed * Time.deltaTime);
    }
}