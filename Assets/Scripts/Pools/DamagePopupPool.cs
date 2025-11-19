using UnityEngine;

public class DamagePopupPool : GenericObjectPool<DamagePopup>
{
    // --- HELPER STATIC ---
    // Añadimos este método estático para facilitar la vida.
    // Así, desde el enemigo solo llamamos a DamagePopupPool.Create(...) y listo.
    public static void Create(Vector3 position, int damageAmount)
    {
        // 1. Pedir al pool (que hereda de GenericObjectPool)
        DamagePopup popup = Instance.Get();

        // 2. Configurar posición
        popup.transform.position = position;

        // 3. Configurar texto (Setup)
        popup.Setup(damageAmount);
    }
}