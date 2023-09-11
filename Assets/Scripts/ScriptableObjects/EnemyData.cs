using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/New Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Prefab")]
    [SerializeField] GameObject enemy;

    [Header("Enemy Description")]
    public EnemyMaterials[] EnemyMaterial;
    public EnemyCategories EnemyCategory;

    [Header("Enemy Movement")]
    public bool RandomMoveInX;
    public bool RandomMoveInY;
    public Vector3Int MovementVector;
    [Space]
    public bool RandomMovementTime;
    public Vector2Int RandomMovementTimeRange;
    public float MovementTime;

    [Header("Enemy Stats")]
    public int MaxHitPoints;
    public int RecyclePointsGiven;

    public void AssignEnemyData()
    {
        enemy.GetComponent<Base_Enemy>().EnemyData = this;
    }

    public enum EnemyMaterials
    {
        None,
        Papel,
        Metal,
        Petroleo,
        Plastico
    }

    public enum EnemyCategories
    {
        None,
        Viviente,
        Andante,
        Consiente,
        Autoconsciente
    }
}
