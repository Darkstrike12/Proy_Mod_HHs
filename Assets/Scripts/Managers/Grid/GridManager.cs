using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GameObject TilePrefab;
    [SerializeField] Grid grid;

    private void Start()
    {
        grid = GetComponent<Grid>();
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var SpawnedTile = Instantiate(TilePrefab, grid.transform.position + (grid.cellSize / 2) +  new Vector3(x,y,0), Quaternion.identity);
                SpawnedTile.name = $"Tile {x} {y}";
                SpawnedTile.transform.parent = transform;
            }
        }
    }
}
