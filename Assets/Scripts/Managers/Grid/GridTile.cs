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
        if (collision.TryGetComponent(out Base_Enemy enem))
        {
            enemy = enem;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy = null;
    }
}
