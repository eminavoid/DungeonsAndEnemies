using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    [SerializeField] private int xpAmount = 10;

    // Ya no necesitamos "Actions" ni callbacks complicados.
    // Usamos el poder del Singleton Genérico.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Dar XP
            // Asegúrate de que LevelManager existe, si no daría error
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.AddExperience(xpAmount);
            }

            // 2. Devolverse al Pool
            // IMPORTANTE: Llamamos al Release pasando 'this' (el componente), no el GameObject.
            ExperienceGemPool.Instance.Release(this);
        }
    }
}