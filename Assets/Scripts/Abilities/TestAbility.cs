using UnityEngine;

[CreateAssetMenu(menuName = "D&D/Abilities/Debug Test")]
public class TestAbility : Ability
{
    [Header("Configuración de Debug")]
    public string message = "¡Habilidad Activada!";
    public Color logColor = Color.green;

    public override void Activate(GameObject parent)
    {
        // Usamos Rich Text de Unity para que se vea claro en la consola
        string hexColor = ColorUtility.ToHtmlStringRGB(logColor);

        Debug.Log($"<color=#{hexColor}><b>[ABILITY] {abilityName}:</b> {message}</color>");
        Debug.Log($"Usada por: {parent.name}");
    }
}