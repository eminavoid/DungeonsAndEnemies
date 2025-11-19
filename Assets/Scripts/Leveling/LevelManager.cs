using System; // Necesario para usar 'Action' (Eventos)
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Configuración")]
    // Curva de nivel: Nivel 1 pide 100, Nivel 2 pide más, etc.
    [SerializeField] private int baseXpRequirement = 100;
    [SerializeField] private float xpMultiplier = 1.2f; // Cada nivel pide 20% más

    // --- Estado ---
    public int CurrentLevel { get; private set; } = 1;
    public int CurrentExperience { get; private set; } = 0;
    public int RequiredExperience { get; private set; }

    // --- Eventos (Patrón Observador) ---
    // La UI se suscribirá a esto para saber cuándo actualizarse.
    // Enviamos dos ints: (XP Actual, XP Requerida)
    public event Action<int, int> OnExperienceChanged;
    public event Action<int> OnLevelUp; // Enviamos el nuevo nivel

    private void Awake()
    {
        Instance = this;
        CalculateNextLevelXp();
    }

    public void AddExperience(int amount)
    {
        CurrentExperience += amount;

        // Chequear si subimos de nivel
        if (CurrentExperience >= RequiredExperience)
        {
            LevelUp();
        }

        // Avisar a la UI (o a quien le interese) que la XP cambió
        OnExperienceChanged?.Invoke(CurrentExperience, RequiredExperience);
    }

    private void LevelUp()
    {
        CurrentLevel++;

        // El excedente de XP se mantiene (rollover)
        CurrentExperience -= RequiredExperience;

        CalculateNextLevelXp();


        // Avisar del Level Up
        OnLevelUp?.Invoke(CurrentLevel);

        // Si nos pasamos de XP y subimos dos niveles de golpe, recursividad:
        if (CurrentExperience >= RequiredExperience)
        {
            LevelUp();
        }
    }

    private void CalculateNextLevelXp()
    {
        // Fórmula simple de crecimiento exponencial
        RequiredExperience = Mathf.RoundToInt(baseXpRequirement * Mathf.Pow(xpMultiplier, CurrentLevel - 1));
    }
}