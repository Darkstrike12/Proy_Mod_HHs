using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveTest : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Vector3Int Move;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2);   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            //transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2) + Move;
            transform.position += Move;
        }
    }
}
