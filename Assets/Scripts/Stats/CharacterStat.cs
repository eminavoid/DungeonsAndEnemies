// CharacterStat.cs
using System.Collections.Generic;
using System;

[Serializable] // Importante para verlo en el Inspector de Unity
public class CharacterStat
{
    public float BaseValue; // El valor "desnudo" (ej. 10 de Fuerza)

    // Lista de modificadores (ej. +2 por casco, +1 por Feat)
    private readonly List<float> modifiers = new List<float>();

    // Cache para no recalcular matemáticas en cada frame (Optimización)
    private float _value;
    private bool _isDirty = true; // "Dirty" significa que algo cambió y hay que recalcular

    public CharacterStat(float baseValue)
    {
        BaseValue = baseValue;
    }

    public float Value
    {
        get
        {
            if (_isDirty)
            {
                _value = CalculateFinalValue();
                _isDirty = false;
            }
            return _value;
        }
    }

    public void AddModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
            _isDirty = true;
        }
    }

    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
            _isDirty = true;
        }
    }

    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        foreach (float mod in modifiers)
        {
            finalValue += mod;
        }
        return finalValue;
    }
}