using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Base_Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Base_Enemy enem))
        {
            enemy = enem;
            //print("Enter Trigeer");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy = null;
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
