// ItemData.cs
using UnityEngine;

public enum EquipmentSlot { Helm, Armor, Boots, Necklace, Ring, MainHand, OffHand }

[CreateAssetMenu(fileName = "New Item", menuName = "D&D Roguelike/Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public Sprite icon;
    public EquipmentSlot slot;

    [Header("Stats (Dejar en 0 si no aplica)")]
    public int strengthBonus;
    public int dexterityBonus;
    public int constitutionBonus;
    public int intelligenceBonus;
    public int wisdomBonus;
    public int charismaBonus;

    [Header("Otros Bonus")]
    public int defenseBonus; // Armadura plana
    public int moveSpeedBonus;

    [Header("Especial")]
    // Si es arma, aquí va el prefab del proyectil
    public GameObject projectilePrefab;
}