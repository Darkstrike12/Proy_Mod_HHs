using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBH : ScriptableObject
{
    [SerializeField] protected Base_Enemy Enemy;
    [TextArea]
    [SerializeField] string Description;

    public virtual void Initialize(Base_Enemy enemy)
    {
        Enemy = enemy;
    }

    public virtual void OnDeathEnter() { }
    public virtual void OnDeathExit() { }
    public virtual void OnDeathUpdate() { }
}
