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
    [SerializeField] Transform EnemySpawner;

    //Internal
    Grid gridCompnent;

    private void Start()
    {
        gridCompnent = GetComponent<Grid>();
        GenerateGrid();

        Player.position = new Vector3(transform.position.x - 1, height/2);
        EnemySpawner.position = new Vector3(width, height - 1) + (gridCompnent.cellSize / 2);
    }

    public Vector2Int GetGridSize()
    {
        return new Vector2Int(width, height);
    }

    public Vector3 GridCellCenter()
    {
        return Vector3.zero + (gridCompnent.cellSize / 2);
    }

    public Grid GetGridCompnent()
    {
        return gridCompnent;
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
        var SpawnedTile = Instantiate(TilePrefab, gridCompnent.transform.position + (gridCompnent.cellSize / 2) + SpawnPosition, Quaternion.identity);
        SpawnedTile.transform.parent = transform;
        SpawnedTile.name = $"{SpawnedTile.name} : {SpawnPosition.x}, {SpawnPosition.y}";
    }
}
