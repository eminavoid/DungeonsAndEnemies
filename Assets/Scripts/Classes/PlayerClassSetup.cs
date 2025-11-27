using UnityEngine;

public class PlayerClassSetup : MonoBehaviour
{
    [Header("Debug (Por si probamos la escena directo sin menú)")]
    [SerializeField] private ClassData debugClass;

    private void Start()
    {
        // 1. Obtener la clase seleccionada del GameManager
        ClassData dataToLoad = null;

        if (GameManager.Instance != null && GameManager.Instance.SelectedClass != null)
        {
            dataToLoad = GameManager.Instance.SelectedClass;
        }
        else
        {
            // Fallback para testing rápido en el editor
            Debug.LogWarning("No se encontró GameManager o Clase Seleccionada. Usando Debug Class.");
            dataToLoad = debugClass;
        }

        if (dataToLoad != null)
        {
            InitializeClass(dataToLoad);
        }
    }

    private void InitializeClass(ClassData data)
    {
        var stats = GetComponent<PlayerStats>();
        var equipment = GetComponent<EquipmentManager>();

        // 1. Aplicar Bonos de Stats Base (Rasgos de clase)
        foreach (var bonus in data.baseStatBonuses)
        {
            stats.AddModifier(bonus.statType, bonus.amount);
        }

        // 2. Equipar Items Iniciales
        foreach (var item in data.startingEquipment)
        {
            equipment.Equip(item);
        }

        Debug.Log($"Personaje inicializado como: {data.className}");
    }
}