using System.Collections.Generic;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager Instance { get; private set; }

    [SerializeField] private DamagePopup popupPrefab;
    [SerializeField] private int poolSize = 30; // 30 números a la vez es suficiente

    private Queue<DamagePopup> pool = new Queue<DamagePopup>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewPoolObject();
        }
    }

    private DamagePopup CreateNewPoolObject()
    {
        DamagePopup popup = Instantiate(popupPrefab);
        popup.gameObject.SetActive(false);
        pool.Enqueue(popup);
        return popup;
    }

    public void ShowDamage(int amount, Vector3 position)
    {
        DamagePopup popup;

        if (pool.Count > 0)
        {
            popup = pool.Dequeue();
        }
        else
        {
            // Si nos quedamos sin pool, creamos uno extra (fallback)
            popup = CreateNewPoolObject();
            // Lo sacamos de la cola inmediatamente porque lo vamos a usar
            pool.Dequeue();
        }

        popup.transform.position = position;
        popup.gameObject.SetActive(true);
        popup.Setup(amount);
    }

    public void ReturnToPool(DamagePopup popup)
    {
        popup.gameObject.SetActive(false);
        pool.Enqueue(popup);
    }
}