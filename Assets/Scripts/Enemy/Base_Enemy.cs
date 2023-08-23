using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Description")]
    [SerializeField] string[] EnemyType;
    [SerializeField] string[] EnemyCategory;

    [Header("Enemy Movement")]
    [SerializeField] int MovementInX;
    [SerializeField] int MovementInY;
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
}
