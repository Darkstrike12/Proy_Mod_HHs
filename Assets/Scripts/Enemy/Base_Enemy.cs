using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Description")]
    //[SerializeField] string[] EnemyType;
    //[SerializeField] string[] EnemyCategory;
    [SerializeField] EnemyMaterials[] EnemyMaterial;
    [SerializeField] EnemyCategories EnemyCategory;

    [Header("Enemy Movement")]
    [SerializeField] int MovementInX;
    [SerializeField] bool RandomX;
    [SerializeField] int MovementInY;
    [SerializeField] bool RandomY;
    [SerializeField] float MovementTime;

    [Header("Enemy Stats")]
    [SerializeField] int HitPoints;
    [SerializeField] int PointsGiven;

    bool TakeDamage = false;

    public void GetDamage(Base_Weapon weapon)
    {

    }

    void EnemyDefeated()
    {

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
