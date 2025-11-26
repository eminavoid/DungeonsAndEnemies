using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Class", menuName = "DnD/Class Data")]
public class ClassData : ScriptableObject
{
    [Header("Información")]
    public string className;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Equipo Inicial")]
    public List<ItemData> startingEquipment;

    [Header("Bonificadores Base (Stats)")]
    public List<StatBonus> baseStatBonuses;
}