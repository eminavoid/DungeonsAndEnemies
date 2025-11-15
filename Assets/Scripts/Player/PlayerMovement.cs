using UnityEngine;
using UnityEngine.InputSystem; // ¡Importante!

// Este componente SÓLO se encarga de mover el transform.
// No sabe de dónde viene la entrada (teclado, IA, etc.).
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Vector2 moveInput;

    // Esta es la función que será llamada por el componente "Player Input"
    // (porque lo pusimos en modo "Send Messages")
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Usamos FixedUpdate para la física o movimiento basado en Rigidbody.
    // Si no usas Rigidbody, puedes usar Update.
    // Para un bullet hell, ¡Update() probablemente sea mejor para respuesta instantánea!
    private void Update()
    {
        // Normalizamos el vector para que el movimiento diagonal no sea más rápido
        Vector2 moveDirection = moveInput.normalized;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}