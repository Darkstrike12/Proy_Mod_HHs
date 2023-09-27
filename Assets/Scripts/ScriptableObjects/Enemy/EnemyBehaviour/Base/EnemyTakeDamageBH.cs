using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamageBH : ScriptableObject
{
    [SerializeField] protected Base_Enemy Enemy;

    public virtual void Initialize(Base_Enemy enemy)
    {
        Enemy = enemy;
    }

    public virtual void OnTakeDamageEnter() { }
    public virtual void OnTakeDamageExit() { }
    public virtual void OnTakeDamageUpdate() { }
}
