using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

public class Base_Weapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] WeaponData weaponDataSO;
    public WeaponData WeaponDataSO { get { return weaponDataSO; } set { weaponDataSO = value; } }

    [Header("Weapon Behaviour Variables")]
    [SerializeField] bool IsInstantKill;
    public bool isInstaKill { get { return IsInstantKill; } }
    [SerializeField] bool UseSpecialEffect;

    private void Start()
    {
        //eventManager = new EventManager();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Base_Enemy>().TakeDamage(this);
            if (UseSpecialEffect) WeaponSpecialEffect(collision.gameObject.GetComponent<Base_Enemy>());
        }
        Destroy(gameObject);
    }

    public virtual void WeaponSpecialEffect(Base_Enemy enemy)
    {
        if (IsWeaponEffective(enemy))
        {
            print("Special Effect");
        }
    }

    private bool IsWeaponEffective(Base_Enemy enemy)
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
