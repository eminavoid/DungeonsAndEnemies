using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    // Singleton local (o referencia pública) para acceder fácil
    public static EquipmentManager Instance { get; private set; }

    // Diccionario para saber qué llevamos puesto en cada slot
    private Dictionary<EquipmentSlot, ItemData> currentEquipment;

    // Referencias
    private PlayerStats playerStats;

    private void Awake()
    {
        Instance = this;
        playerStats = GetComponent<PlayerStats>();
        currentEquipment = new Dictionary<EquipmentSlot, ItemData>();
    }

    public void Equip(ItemData newItem)
    {
        // 1. Si ya hay algo en ese slot, lo quitamos primero (Swap)
        if (currentEquipment.ContainsKey(newItem.slot))
        {
            Unequip(newItem.slot);
        }

        // 2. Guardar el nuevo item en el diccionario
        currentEquipment[newItem.slot] = newItem;

        // 3. Aplicar los stats al jugador
        foreach (StatBonus bonus in newItem.statBonuses)
        {
            playerStats.AddModifier(bonus.statType, bonus.amount);
        }

        Debug.Log($"Equipado: {newItem.itemName} en {newItem.slot}");
    }

    public void Unequip(EquipmentSlot slot)
    {
        if (currentEquipment.ContainsKey(slot))
        {
            ItemData oldItem = currentEquipment[slot];

            // 1. Retirar los stats
            foreach (StatBonus bonus in oldItem.statBonuses)
            {
                playerStats.RemoveModifier(bonus.statType, bonus.amount);
            }

            // 2. Sacar del diccionario
            currentEquipment.Remove(slot);

            Debug.Log($"Desequipado: {oldItem.itemName}");
        }
    }

    // Método de prueba para equipar desde código (temporal)
    public void TestEquip(ItemData item)
    {
        Equip(item);
    }










    [Header("Debug")]
    public ItemData debugItemToEquip;

    [ContextMenu("Equipar Item de Debug")]
    public void EquipDebugItem()
    {
        if (debugItemToEquip != null) Equip(debugItemToEquip);
    }

    [ContextMenu("Desequipar Todo")]
    public void UnequipAll()
    {
        // Copiamos las claves para no romper el loop al borrar
        var slots = new List<EquipmentSlot>(currentEquipment.Keys);
        foreach (var slot in slots) Unequip(slot);
    }
}