using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Base_Enemy enemy;

    void Start()
    {
        enemy = null;
    }

    void Update()
    {
        
    }

    protected void GetEnemy(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Base_Enemy enem))
        {
            enemy = enem;
            //print("Enter Trigeer");
        }
    }

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

    protected void ClearEnemy()
    {
        enemy = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetEnemy(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ClearEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Base_Enemy enem))
        {
            enemy = enem;
            print("Enter Coliision");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        enemy = null;
    }
}
