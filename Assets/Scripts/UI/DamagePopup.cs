using UnityEngine;
using TMPro; // Necesario para TextMeshPro

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float disappearSpeed = 3f;
    [SerializeField] private float disappearTimerMax = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    // Método Setup: Reinicia el texto como si fuera nuevo
    public void Setup(int damageAmount)
    {
        textMesh.text = damageAmount.ToString();

        // Configuración inicial
        textColor = textMesh.color;
        textColor.a = 1f; // Alfa al máximo (visible)
        textMesh.color = textColor;
        disappearTimer = disappearTimerMax;

        // Un poco de movimiento aleatorio lateral para que no se encimen
        moveVector = new Vector3(Random.Range(-0.5f, 0.5f), 1f) * moveSpeed;

        // Resetear escala (efecto "pop")
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        // 1. Moverse hacia arriba
        transform.position += moveVector * Time.deltaTime;

        // 2. Efecto de escala (opcional, da un toque "jugoso")
        if (disappearTimer > disappearTimerMax * 0.5f)
        {
            // Primera mitad: Crece un poco
            transform.localScale += Vector3.one * 1f * Time.deltaTime;
        }
        else
        {
            // Segunda mitad: Se encoge
            transform.localScale -= Vector3.one * 1f * Time.deltaTime;
        }

        // 3. Desvanecerse
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            // Empezar a bajar el alpha
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
            {
                // Cuando es invisible, devolver al pool
                DamagePopupPool.Instance.Release(this);
            }
        }
    }
}