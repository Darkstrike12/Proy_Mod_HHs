using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Base_Enemy;

[CreateAssetMenu(menuName = "Scriptable Objects/New Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("Display")]
    [SerializeField] public string DisplayName;
    [SerializeField] Sprite DisplayImage;
    [SerializeField] string DisplayDescription;

    [Header("Weapon Stats")]
    [SerializeField] int BaseDamage;
    [SerializeField] int BaseUseCost;
    [SerializeField] float BaseReloadTime;
    [SerializeField] int BaseAtackRangeInX;
    [SerializeField] int BaseAtackRangeInY;

    [Header("Weapon Efectiveness")]
    [SerializeField] public EnemyMaterials[] AffectedEnemyMaterials;
    [SerializeField] public EnemyCategories[] AffectedEnemyCategories;
}
