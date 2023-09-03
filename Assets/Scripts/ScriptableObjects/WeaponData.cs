using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Base_Enemy;

[CreateAssetMenu(menuName = "Scriptable Objects/New Weapon")]
public class WeaponData : ScriptableObject
{
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
    public int BaseAtackRangeInX;
    public int BaseAtackRangeInY;

    [Header("Weapon Efectiveness")]
    public bool AffectAllMaterials;
    public EnemyData.EnemyMaterials[] AffectedEnemyMaterials;
    [Space]
    public bool AffectAllCategories;
    public EnemyData.EnemyCategories[] AffectedEnemyCategories;
}
