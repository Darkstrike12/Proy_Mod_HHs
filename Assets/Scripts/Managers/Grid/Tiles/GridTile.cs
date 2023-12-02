using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    //public Base_Enemy enemy { get; protected set; }
    //bool IsTileSelected;

    void Start()
    {

    }

    void Update()
    {
        
    }

    //private void OnMouseDown()
    //{
    //    IsTileSelected = true;
    //    print(IsTileSelected);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent(out Base_Weapon weapon) && IsTileSelected)
    //    {
    //        weapon.WeaponHit(transform.position);
    //        IsTileSelected = false;
    //        print(IsTileSelected);
    //    }
    //}

    protected bool IsEnemyOnTile(Collider2D collision, out Base_Enemy Enem)
    {
        if(collision.TryGetComponent(out Base_Enemy enemy))
        {
            Enem = enemy;
            return true;
        }
        else
        {
            Enem = null;  
            return false;
        }
    }

    //protected bool IsWeaponOnTile(Collider2D collision)
    //{
    //    if (collision.TryGetComponent(out Base_Weapon weapon))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
