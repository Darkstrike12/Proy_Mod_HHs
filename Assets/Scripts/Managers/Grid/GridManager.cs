using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [SerializeField] int width, height;

    [Header("Tiles")]
    [SerializeField] GameObject GridTilePF;
    [SerializeField] GameObject FinishTilePF;
    [SerializeField] GameObject EnemySpawnTilePF;
    [SerializeField] GameObject BarrierTilePF;

    [Header("External")]
    [SerializeField] Transform Player;

    Grid grid;

    private void Start()
    {
        grid = GetComponent<Grid>();
        GenerateGrid();
        Player.position = new Vector3(transform.position.x - 1, height/2);
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            SpawnTile(BarrierTilePF, new Vector3(x, height, 0));

            SpawnTile(BarrierTilePF, new Vector3(x, -1, 0));

            for (int y = 0; y < height; y++)
            {
                SpawnTile(GridTilePF, new Vector3(x, y, 0));

                SpawnTile(FinishTilePF, new Vector3(-1, y, 0));

                SpawnTile(EnemySpawnTilePF, new Vector3(width, y, 0));
            }

        }
    }

    void SpawnTile(GameObject TilePrefab, Vector3 SpawnPosition)
    {
        var SpawnedTile = Instantiate(TilePrefab, grid.transform.position + (grid.cellSize / 2) + SpawnPosition, Quaternion.identity);
        SpawnedTile.transform.parent = transform;
        SpawnedTile.name = $"{SpawnedTile.name} : {SpawnPosition.x}, {SpawnPosition.y}";
    }
}
