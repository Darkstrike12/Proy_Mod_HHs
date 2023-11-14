using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Data/New Enemy Data")]
public class EnemyData : ScriptableObject
{
    //[Header("Enemy Prefab")]
    //[SerializeField] GameObject enemy;

    [Header("Display Data")]
    public string DisplayName;
    public Sprite DisplayImage;
    [TextArea]
    public string DisplayDescription;

    [Header("Enemy Description")]
    public EnemyMaterials[] EnemyMaterial;
    public EnemyCategories EnemyCategory;

    [Header("Enemy Movement")]
    public Vector3Int MovementVector;
    [Space]
    public float MovementTime;
    public float[] MovementTimeRange;

    [Header("Enemy Stats")]
    public int MaxHitPoints;
    public int RecyclePointsGiven;

    //public void AssignEnemyData()
    //{
    //    enemy.GetComponent<Base_Enemy>().EnemyData = this;
    //}

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
