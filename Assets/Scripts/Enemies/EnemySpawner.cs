using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 12f;
    [SerializeField] private float spawnRate = 1f;

    [Header("Manual de Monstruos")]
    [SerializeField] private EnemyData[] availableEnemies;

    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void SpawnRandomEnemy()
    {
        if (availableEnemies.Length == 0) return;

        // 1. Calculamos posición
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + (Vector3)randomCircle;

        // 2. Obtenemos el componente SimpleEnemy directamente del Pool
        // --- CAMBIO PRINCIPAL AQUÍ ---
        SimpleEnemy enemyScript = EnemyPool.Instance.Get();

        // 3. Configuramos posición y rotación
        enemyScript.transform.position = spawnPos;
        enemyScript.transform.rotation = Quaternion.identity;

        // 4. Configuración de datos (Data)
        EnemyData randomMonster = availableEnemies[Random.Range(0, availableEnemies.Length)];
        enemyScript.Configure(randomMonster);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
}