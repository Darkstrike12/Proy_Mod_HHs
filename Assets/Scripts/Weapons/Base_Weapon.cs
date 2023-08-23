using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] int Damage;
    [SerializeField] int UseCost;
    [SerializeField] float ReloadTime;
    [SerializeField] int AtackRangeInX;
    [SerializeField] int AtackRangeInY;
    //[SerializeField] string[] AffectedEnemyTypes;
    //[SerializeField] string[] AffectedEnemyCategories;
    [Header("Weapon Efectiveness")]
    [SerializeField] Base_Enemy.EnemyMaterials[] AffectedEnemyMaterials;
    [SerializeField] Base_Enemy.EnemyCategories[] AffectedEnemyCategories;
}
