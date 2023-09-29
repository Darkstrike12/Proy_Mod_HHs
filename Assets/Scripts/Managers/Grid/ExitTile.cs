using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTile : GridTile
{
    [SerializeField] float DestryDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsEnemyOnTile(collision, out Base_Enemy enemy))
        {
            Destroy(collision.gameObject, DestryDelay);
        }
    }
}
