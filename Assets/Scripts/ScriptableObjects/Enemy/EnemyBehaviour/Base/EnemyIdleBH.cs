using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleBH : ScriptableObject
{
    [SerializeField] protected Base_Enemy Enemy;

    public virtual void Initialize(Base_Enemy enemy)
    {
        Enemy = enemy;
    }

    public virtual void OnIdleEnter() { }
    public virtual void OnIdleExit() { }
    public virtual void OnIdleUpdate() { }
}
