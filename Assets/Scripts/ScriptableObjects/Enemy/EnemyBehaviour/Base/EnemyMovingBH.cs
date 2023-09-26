using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingBH : ScriptableObject
{
    [SerializeField] protected Base_Enemy Enemy;

    public virtual void Initialize(Base_Enemy enemy)
    {
        Enemy = enemy;
    }

    public virtual void OnMovingEnter() { }
    public virtual void OnMovingExit() { }
    public virtual void OnMovingUpdate() { }
}
