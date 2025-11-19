// PlayerStats.cs
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Aquí guardamos todos los stats
    private Dictionary<StatType, CharacterStat> stats;

    [Header("Configuración Base (Nivel 1)")]
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float baseMoveSpeed = 6f; // Fijo

    // Evento: La UI se suscribirá a esto para actualizarse cuando cambie algo
    public event System.Action OnStatsChanged;

    // Propiedades públicas para lectura rápida
    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        InitializeStats();
        CurrentHealth = GetTotalValue(StatType.MaxHealth);
    }

    private void InitializeStats()
    {
        stats = new Dictionary<StatType, CharacterStat>();

        // Creamos una entrada para CADA tipo de stat definido en el Enum
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            stats.Add(type, new CharacterStat(0));
        }

        // Valores iniciales de atributos (luego vendrán de la Clase elegida)
        stats[StatType.Strength].BaseValue = 10;
        stats[StatType.Dexterity].BaseValue = 10;
        stats[StatType.Constitution].BaseValue = 10;
        stats[StatType.Intelligence].BaseValue = 10;
        stats[StatType.Wisdom].BaseValue = 10;
        stats[StatType.Charisma].BaseValue = 10;

        // Move Speed base (Stat Neutral)
        stats[StatType.MoveSpeed].BaseValue = baseMoveSpeed;
    }

    // --- EL NÚCLEO MATEMÁTICO ---
    public float GetTotalValue(StatType type)
    {
        // 1. Empezamos con el valor base + modificadores directos (Items)
        float val = stats[type].Value;

        // 2. Sumamos los bonus derivados de tus Atributos (STR, DEX, etc.)
        switch (type)
        {
            // STR (Fuerza)
            case StatType.MeleeDamageMult: val += stats[StatType.Strength].Value * 0.05f; break; // +5% daño
            case StatType.KnockbackForce: val += stats[StatType.Strength].Value * 0.5f; break;
            case StatType.ArmorPenetration: val += stats[StatType.Strength].Value * 1f; break;

            // DEX (Destreza)
            case StatType.CritChance: val += stats[StatType.Dexterity].Value * 0.5f; break; // 10 DEX = 5% crit
            case StatType.EvasionChance: val += stats[StatType.Dexterity].Value * 0.2f; break; // Evasión escala lento
            case StatType.AttackSpeedMult: val += stats[StatType.Dexterity].Value * 0.02f; break;
            case StatType.RangedDamageMult: val += stats[StatType.Dexterity].Value * 0.05f; break;

            // CON (Constitución)
            case StatType.MaxHealth: val += baseHealth + (stats[StatType.Constitution].Value * 5f); break;
            case StatType.Armor: val += stats[StatType.Constitution].Value * 0.5f; break;
            case StatType.StatusResistance: val += stats[StatType.Constitution].Value * 1f; break;

            // INT (Inteligencia)
            case StatType.MagicDamageMult: val += stats[StatType.Intelligence].Value * 0.05f; break;
            case StatType.ManaRegeneration: val += stats[StatType.Intelligence].Value * 0.2f; break;

            // WIS (Sabiduría)
            case StatType.CooldownReduction: val += stats[StatType.Wisdom].Value * 0.5f; break; // Tu regla de CDR
            case StatType.HealingPower: val += stats[StatType.Wisdom].Value * 2f; break;
            case StatType.MagicResistance: val += stats[StatType.Wisdom].Value * 1f; break;

            // CHA (Carisma)
            case StatType.ExperienceMultiplier: val += stats[StatType.Charisma].Value * 1f; break; // 10 CHA = +10% XP
            case StatType.Luck: val += stats[StatType.Charisma].Value * 0.5f; break;
        }

        return val;
    }

    // Método para recibir daño (incluye tu lógica de Evasión)
    public void TakeDamage(int rawDamage)
    {
        // 1. Evasión (RNG Clásico)
        float evasionChance = GetTotalValue(StatType.EvasionChance);
        if (Random.Range(0f, 100f) < evasionChance)
        {
            // ¡Esquivado!
            DamagePopupPool.Create(transform.position, 0); // 0 o texto "MISS" si lo implementamos
            Debug.Log("¡MISS! Ataque esquivado.");
            return;
        }

        // 2. Mitigación por Armadura (CON)
        float armor = GetTotalValue(StatType.Armor);
        int finalDamage = Mathf.Max(Mathf.RoundToInt(rawDamage - armor), 1); // Mínimo 1 daño

        CurrentHealth -= finalDamage;

        // Feedback Visual
        DamagePopupPool.Create(transform.position, finalDamage);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // Métodos para items/buffs
    public void AddModifier(StatType type, float amount)
    {
        if (stats.ContainsKey(type))
        {
            stats[type].AddModifier(amount);
            OnStatsChanged?.Invoke();
        }
    }

    public void RemoveModifier(StatType type, float amount)
    {
        if (stats.ContainsKey(type))
        {
            stats[type].RemoveModifier(amount);
            OnStatsChanged?.Invoke();
        }
    }

    private void Die()
    {
        Debug.Log("JUGADOR MUERTO");
        // Aquí iría la lógica de Game Over
    }
}