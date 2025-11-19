using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "DnD/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Stats Básicos")]
    public string enemyName;
    public float moveSpeed = 3f;
    public int maxHealth = 10;
    public int damage = 1;

    [Header("Visuales")]
    public Color color = Color.white;
    public float scale = 1f;
    public Sprite sprite;
}