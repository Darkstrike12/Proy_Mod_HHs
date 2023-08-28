using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Base_Enemy;

public class Base_Weapon : MonoBehaviour
{
    /*
    [Header("Weapon Stats")]
    [SerializeField] int Damage;
    [SerializeField] int UseCost;
    [SerializeField] float ReloadTime;
    [SerializeField] int AtackRangeInX;
    [SerializeField] int AtackRangeInY;
    [Header("Weapon Efectiveness")]
    [SerializeField] public EnemyMaterials[] AffectedEnemyMaterials;
    //[SerializeField] public List<EnemyMaterials> AffectedMaterials;
    [SerializeField] public EnemyCategories[] AffectedEnemyCategories;
    */

    [SerializeField] public WeaponData weaponData;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Base_Enemy enemigoColliion = collision.collider.gameObject.GetComponent<Base_Enemy>();
        collision.collider.gameObject.GetComponent<Base_Enemy>().TakeDamage(this);
    }
}
