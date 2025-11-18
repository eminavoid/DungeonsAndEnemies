using System.Collections.Generic;
using UnityEngine;

// <T> significa "Cualquier tipo de Componente" (Enemy, Projectile, etc.)
public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    public static GenericObjectPool<T> Instance { get; private set; }

    [Header("Configuración del Pool")]
    [SerializeField] private T prefab;
    [SerializeField] private int initialSize = 20;

    private Queue<T> pool = new Queue<T>();

    protected virtual void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        T newObj = Instantiate(prefab);
        newObj.gameObject.SetActive(false);
        // Opcional: Si quieres organizar la jerarquía, hazlos hijos del pool
        newObj.transform.SetParent(transform);
        pool.Enqueue(newObj);
        return newObj;
    }

    public T Get()
    {
        if (pool.Count == 0)
        {
            CreateNewObject();
        }

        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}