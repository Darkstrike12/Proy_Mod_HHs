using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Description")]
    [SerializeField] EnemyMaterials[] EnemyMaterial;
    public EnemyMaterials[] enemyMaterial { get { return EnemyMaterial; } }
    [SerializeField] EnemyCategories EnemyCategory;
    public EnemyCategories enemyCategory { get { return EnemyCategory; } }

    [Header("Enemy Movement")]
    [SerializeField] bool RandomMoveInX;
    [SerializeField] bool RandomMoveInY;
    [SerializeField] Vector3Int Movement;
    [SerializeField] float MovementTime;

    [Header("Enemy Stats")]
    [SerializeField] int HitPoints;
    [SerializeField] int ReciclePointsGiven;
    [SerializeField] bool AllowDamage;

    //Internal Variables
    int CurrentHitPoints;

    //Internal References
    Animator animator;

    private void Start()
    {
        CurrentHitPoints = HitPoints;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(Base_Weapon weapon)
    {
        if(CurrentHitPoints <= 0 || weapon.isInstaKill) EnemyDefeated();
        CurrentHitPoints -= weapon.weaponDataSO.BaseDamage;
        
        //var damage = weapon.weaponDataSO.AffectedEnemyMaterials.Intersect(EnemyMaterial);
    }

    void EnemyDefeated()
    {
        animator.SetTrigger("IsDefeated");
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
