using UnityEngine;
using UnityEngine.UI;

public class ExperienceBarUI : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        // Suscribirse al evento del LevelManager
        LevelManager.Instance.OnExperienceChanged += UpdateBar;

        // Inicializar en 0
        UpdateBar(0, LevelManager.Instance.RequiredExperience);
    }

    private void OnDestroy()
    {
        // ¡SIEMPRE desuscribirse de eventos estáticos para evitar fugas de memoria!
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnExperienceChanged -= UpdateBar;
        }
    }

    private void UpdateBar(int currentXp, int requiredXp)
    {
        // Convertir a valor 0.0 - 1.0
        float fillAmount = (float)currentXp / requiredXp;
        slider.value = fillAmount;
    }
}