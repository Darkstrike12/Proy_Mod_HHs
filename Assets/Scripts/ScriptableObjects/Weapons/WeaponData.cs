using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapon/Data/New Weapon Data")]
public class WeaponData : ScriptableObject
{
    //[Header("WeaponPrefab")]
    //[SerializeField] GameObject weapon;

    [Header("Internal Data")]
    public int WeaponID;

    [Header("Display Data")]
    public string DisplayName;
    public Sprite DisplayImage;
    [TextArea]
    public string DisplayDescription;

    [Header("Weapon Stats")]
    public int BaseDamage;
    public int BaseUseCost;
    public float BaseReloadTime;
    [Space(20)]
    [SerializeField] Vector2 attackRange;
    public Vector2 AttackRange
    {
        get
        {
            if (attackRange.x <= 0)
            {
                attackRange.x = 1;
            }
            if (attackRange.y <= 0)
            {
                attackRange.y = 1;
            }
            return attackRange;
        }
    }

    [Header("Weapon Efectiveness")]
    public bool AffectAllMaterials;
    public EnemyData.EnemyMaterials[] AffectedEnemyMaterials;
    [Space]
    public bool AffectAllCategories;
    public EnemyData.EnemyCategories[] AffectedEnemyCategories;

    [Header("Weapon Sounds")]
    public FmodEvent HitSound;
    public FmodEvent EffectSound;

    //public void AssignWeaponData()
    //{
    //    weapon.GetComponent<Base_Weapon>().WeaponDataSO = this;
    //}
}
