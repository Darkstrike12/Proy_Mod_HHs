using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/New Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("WeaponPrefab")]
    [SerializeField] GameObject weapon;

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
    public Vector2Int AtackRange;

    [Header("Weapon Efectiveness")]
    public bool AffectAllMaterials;
    public EnemyData.EnemyMaterials[] AffectedEnemyMaterials;
    [Space]
    public bool AffectAllCategories;
    public EnemyData.EnemyCategories[] AffectedEnemyCategories;

    public void AssignWeaponData()
    {
        weapon.GetComponent<Base_Weapon>().WeaponDataSO = this;
    }
}
