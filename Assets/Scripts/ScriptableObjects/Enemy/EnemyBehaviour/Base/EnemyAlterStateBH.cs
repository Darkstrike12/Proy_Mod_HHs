using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlterStateBH : ScriptableObject
{
    [SerializeField] protected Base_Enemy Enemy;
    [TextArea]
    [SerializeField] string Description;

    public float stateDuration;

    public virtual void Initialize(Base_Enemy enemy)
    {
        Enemy = enemy;
    }

    public virtual void OnAlterStateEnter() { }
    public virtual void OnAlterStateExit() { }
    public virtual void OnAlterStateUpdate() { }
}
