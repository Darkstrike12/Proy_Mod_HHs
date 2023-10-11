using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Base_Weapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] WeaponData weaponDataSO;
    public WeaponData WeaponDataSO { get { return weaponDataSO; } set { weaponDataSO = value; } }

    [Header("Weapon Behaviour Variables")]
    [SerializeField] bool IsInstantKill;
    //public bool isInstaKill { get { return IsInstantKill; } }
    [SerializeField] bool UseSpecialEffect;
    [SerializeField] float DestroyDelay;

    //Events
    [SerializeField] UnityEvent Hit;

    //Internal Referemces
    public Rigidbody2D RigidBody { get; protected set; }

    //Internal Variables
    Vector3 HitPosition;

    #region Unity Functions

    protected virtual void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.TryGetComponent(out Base_Enemy Enem))
        //{
        //    Enem.TakeDamage(WeaponDataSO.BaseDamage, IsInstantKill);
        //    if (UseSpecialEffect) WeaponSpecialEffect(Enem);
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject, DestroyDelay);
        //}
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(HitPosition, new Vector3(weaponDataSO.AtackRange.x, weaponDataSO.AtackRange.y));
    }

    #endregion

    public void SetHitPoint(Vector3 Point)
    {
        HitPosition = Point;
    }

    public virtual void WeaponHitWhTl(Vector3 hitPoint)
    {
        RigidBody.bodyType = RigidbodyType2D.Static;
        Collider2D[] Colliders = Physics2D.OverlapBoxAll(hitPoint, weaponDataSO.AtackRange, 0f);
        HitPosition = hitPoint;
        foreach (Collider2D col in Colliders)
        {
            if (col.gameObject.TryGetComponent(out Base_Enemy Enem))
            {
                Enem.TakeDamage(WeaponDataSO.BaseDamage, IsInstantKill);
                if (UseSpecialEffect) WeaponSpecialEffect(Enem);
                //Destroy(gameObject);
            }
        }
        Destroy(gameObject, DestroyDelay);
    }

    protected virtual void WeaponSpecialEffect(Base_Enemy enemy)
    {
        if (IsWeaponEffective(enemy))
        {
            print("Special Effect");
        }
    }

    protected bool IsWeaponEffective(Base_Enemy enemy)
    {
        if(!weaponDataSO.AffectAllMaterials || !weaponDataSO.AffectAllCategories)
        {
            var AffectedByMaterial = weaponDataSO.AffectedEnemyMaterials.Intersect(enemy.EnemyData.EnemyMaterial);
            bool AffectedByCategory = weaponDataSO.AffectedEnemyCategories.Contains(enemy.EnemyData.EnemyCategory);
            if ((AffectedByMaterial.Count() > 0 || weaponDataSO.AffectAllMaterials) && (AffectedByCategory || weaponDataSO.AffectAllCategories)) return true;
            else return false;
        }
        else
        {
            return true;
        }
    }
}