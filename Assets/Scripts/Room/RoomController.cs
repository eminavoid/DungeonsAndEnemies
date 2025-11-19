using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("Configuración de la Sala")]
    [SerializeField] private Transform[] spawnPoints; // Arrastra aquí tus puntos vacíos
    [SerializeField] private EnemyData[] roomEnemies; // Qué monstruos salen aquí
    [SerializeField] private GameObject[] doors; // Puertas para bloquear
    [SerializeField] private int roomXpReward = 100; // XP por limpiar la sala

    private List<SimpleEnemy> activeEnemies = new List<SimpleEnemy>();
    private bool isRoomActive = false;
    private bool isRoomCleared = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // DEBUG 1: ¿Detecta algo?

        if (other.CompareTag("Player") && !isRoomActive && !isRoomCleared)
        {
            StartRoomBattle();
        }
    }

    private void StartRoomBattle()
    {
        isRoomActive = true;
        CloseDoors(true);
        SpawnEnemies();
    }

    private void CloseDoors(bool close)
    {
        foreach (var door in doors)
        {
            door.SetActive(close); // Si close=true, activa el muro (bloquea)
        }
    }

    private void SpawnEnemies()
    {
        // Spawnear enemigos en los puntos definidos
        foreach (Transform sp in spawnPoints)
        {
            if (roomEnemies.Length == 0)
            {
                return;
            }
            // Elegimos un monstruo al azar de la lista de esta sala
            EnemyData monsterData = roomEnemies[Random.Range(0, roomEnemies.Length)];

            // Usamos nuestro Pool existente
            SimpleEnemy enemy = EnemyPool.Instance.Get();

            // Lo colocamos y configuramos
            enemy.transform.position = sp.position;
            enemy.Configure(monsterData);

            // !!! IMPORTANTE !!!
            // Necesitamos saber cuándo muere este enemigo específico para chequear la sala.
            // Nos suscribimos a un evento (tendremos que añadir esto a SimpleEnemy)
            enemy.OnEnemyDeath += HandleEnemyDeath;

            activeEnemies.Add(enemy);
        }
    }

    private void HandleEnemyDeath(SimpleEnemy enemy)
    {
        // Quitar de la lista
        activeEnemies.Remove(enemy);
        enemy.OnEnemyDeath -= HandleEnemyDeath; // Desuscribirse para evitar errores

        // Si no quedan enemigos... ¡Victoria!
        if (activeEnemies.Count == 0)
        {
            RoomCleared();
        }
    }

    private void RoomCleared()
    {
        isRoomActive = false;
        isRoomCleared = true;
        CloseDoors(false); // Abrir puertas

        // --- RECOMPENSA ---
        // Damos la XP directa al manager, sin gemas físicas
        LevelManager.Instance.AddExperience(roomXpReward);


        // Opcional: Spawnear un cofre o item aquí
    }
}