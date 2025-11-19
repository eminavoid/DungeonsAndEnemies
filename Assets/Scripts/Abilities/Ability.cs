using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public float cooldownTime;
    public int manaCost; // Si usas maná

    // Método abstracto: Cada habilidad definirá qué hace
    public abstract void Activate(GameObject parent);
}