using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Weapon : MonoBehaviour
{
    [SerializeField] int Damage;
    [SerializeField] int AtackRangeInX;
    [SerializeField] int AtackRangeInY;
    [SerializeField] string[] AffectedEnemyTypes;
    [SerializeField] string[] AffectedEnemyCategories;
}
