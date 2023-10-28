using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [SerializeField] int width, height;

    [Header("Tiles")]
    [SerializeField] GameObject GameTilePF;
    [SerializeField] GameObject FinishTilePF;
    [SerializeField] GameObject EnemySpawnTilePF;
    [SerializeField] GameObject BarrierTilePF;

    [Header("External References")]
    [SerializeField] Transform Player;
    [SerializeField] Transform EnemySpawner;

    //Internal
    Grid gridCompnent;
    public Grid Grid { get => gridCompnent; }
    [field: SerializeField] public int EnemyCount { get; private set; }

    #region Unity Functions

    private void Start()
    {
        gridCompnent = GetComponent<Grid>();
        GenerateGrid();

        Player.position = new Vector3(transform.position.x - 2.2f, height/2 + 2f);
        EnemySpawner.position = new Vector3(width, height - 1) + (gridCompnent.cellSize / 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(width, height, 0f), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, height, 0f), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(width, 0f, 0f), 0.2f);
    }

    #endregion

    #region Getters

    public Vector2 GetGridSize()
    {
        return new Vector2(width, height);
    }

    public Vector3 GridCellCenter()
    {
        return Vector3.zero + (gridCompnent.cellSize / 2);
    }

    #endregion

    public bool IsOverGameGrid(Vector3 Position)
    {
        if(Physics2D.OverlapCircle(Position, 0.25f, LayerMask.GetMask("GameGrid")))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public int CountEnemiesOnGrid()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position, transform.position + new Vector3(width, height, 0f));
        if (colliders.Length > 0)
        {
            EnemyCount = 0;
            foreach (Collider2D coll in colliders)
            {
                if (coll.gameObject.TryGetComponent(out Base_Enemy enemy))
                {
                    EnemyCount++;
                }
            }
        }
        return EnemyCount;
    }

    #region Grid Generation

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnTile(GameTilePF, new Vector3(x, y, 0));
            }
        }

        for (int x = 0; x < width; x++)
        {
            SpawnTile(BarrierTilePF, new Vector3(x, height, 0));

            SpawnTile(BarrierTilePF, new Vector3(x, -1, 0));
        }

        for (int y = 0; y < height; y++)
        {
            SpawnTile(FinishTilePF, new Vector3(-1, y, 0));

            SpawnTile(EnemySpawnTilePF, new Vector3(width, y, 0));
        }
    }

    void SpawnTile(GameObject TilePrefab, Vector3 SpawnPosition)
    {
        var SpawnedTile = Instantiate(TilePrefab, gridCompnent.transform.position + (gridCompnent.cellSize / 2) + SpawnPosition, Quaternion.identity);
        SpawnedTile.transform.parent = transform;
        SpawnedTile.name = $"{SpawnedTile.name} : {SpawnPosition.x}, {SpawnPosition.y}";
    }

    #endregion
}
