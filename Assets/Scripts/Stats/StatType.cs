public enum StatType
{
    // --- Atributos Primarios ---
    Strength,
    Dexterity,
    Constitution,
    Intelligence,
    Wisdom,
    Charisma,

    // --- Derivadas Ofensivas ---
    MeleeDamageMult,    // Multiplicador de daño melee
    KnockbackForce,
    ArmorPenetration,
    CritChance,
    AttackSpeedMult,    // Multiplicador de velocidad de ataque
    RangedDamageMult,
    MagicDamageMult,
    ElementalMastery,
    CooldownReduction,  // CDR (Wisdom)

    // --- Derivadas Defensivas / Utilidad ---
    MaxHealth,
    Armor,              // Defensa física
    EvasionChance,      // Tu regla #2 (RNG)
    StatusResistance,
    ManaRegeneration,
    HealingPower,
    MagicResistance,

    // --- Meta / Otros ---
    Luck,
    ExperienceMultiplier,
    MoveSpeed           // Tu regla #1 (Independiente)
}