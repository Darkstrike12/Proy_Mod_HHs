using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitTile : GridTile
{
    public float finishOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsEnemyOnTile(collision, out Base_Enemy enemy))
        {
            enemy.EnemAnimator.SetFloat("ExitYOffset", finishOffset);
            enemy.EnemAnimator.SetTrigger("IsExitMap");
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (IsEnemyOnTile(collision, out Base_Enemy enemy))
    //    {
    //        enemy.EnemAnimator.SetTrigger("IsExitMap");
    //    }
    //}
}

