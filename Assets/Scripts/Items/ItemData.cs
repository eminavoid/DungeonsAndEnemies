using UnityEngine;
using System.Collections.Generic;

// Estructura auxiliar para definir "Qué stat" y "Cuánto"
[System.Serializable]
public struct StatBonus
{
    public StatType statType;
    public float amount;
}

[CreateAssetMenu(fileName = "Item_", menuName = "DnD/Item")]
public class ItemData : ScriptableObject
{
    [Header("Visuales")]
    public string itemName;
    public Sprite icon;
    public EquipmentSlot slot;

    [Header("Bonificaciones")]
    // Una lista es mucho más limpia que tener 20 variables public int str, dex, con...
    // Puedes agregar tantos bonos como quieras en el inspector.
    public List<StatBonus> statBonuses;

    [Header("Especial (Armas/Activos)")]
    public GameObject projectilePrefab;
    // public Ability abilityToGrant; // Para el futuro
}