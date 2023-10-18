using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float SpawnDelay;
    [SerializeField] int MaxEnemyCount;

    [Header("Enemies To Spawn")]
    [SerializeField] List<Base_Enemy> aviableEnemiesToSpawn;

    [Header("External References")]
    [SerializeField] GridManager grid;

    [Header("Events")]
    public UnityEvent OnEnemySpawned;

    [Header("Internal Variables")]
    //Internal Variables
    Vector2Int GridSize;
    float CurrentSpawnDelay;
    public int CurrentEnemyCount;
    public int DefeatedEnemyCount = 0;

    //SingletonInstance
    public static EnemySpawner Instance;

    #region UnityFunctions

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GridSize = grid.GetGridSize();

        CurrentSpawnDelay = 0f;
        CurrentEnemyCount = 0;
    }

    void Update()
    {
        CurrentSpawnDelay += Time.deltaTime;
        if (CurrentSpawnDelay > SpawnDelay && CurrentEnemyCount < MaxEnemyCount && GameManager.Instance.RemainingEnemies > 0)
        {
            OnEnemySpawned.Invoke();
            CurrentSpawnDelay = 0f;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnEnemySpawned.Invoke();
        }
    }

    #endregion

    public void UpdateStatsOnEnemyDefrated()
    {
        CurrentEnemyCount--;
        DefeatedEnemyCount++;
    }

    public void UpdateStatsOnEnemyDestroyed()
    {
        CurrentEnemyCount--;
    }

    Base_Enemy SelectEnemyToSpawn()
    {
        switch (GameManager.Instance.CurrentLevelState)
        {
            case GameManager.LevelState.Soft:
                SpawnDelay = (float)(Random.Range(5, 7));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 5;
                break;
            case GameManager.LevelState.Medium:
                SpawnDelay = (float)(Random.Range(3, 5));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 3;
                break;
            case GameManager.LevelState.Hard:
                SpawnDelay = (float)(Random.Range(1, 3));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 2;
                break;
            case GameManager.LevelState.Finish:
                break;
        }

        return aviableEnemiesToSpawn[0];
    }

    public void SpawnEnemy()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(0, GridSize.y) + grid.GridCellCenter().y, 0f);
        Base_Enemy EnemySpawned = Instantiate(SelectEnemyToSpawn(), transform.position + Vector3.left, Quaternion.identity);
        //EnemySpawned.transform.parent = transform;
        EnemySpawned.InitEnemy(grid.GetGridCompnent());
        CurrentEnemyCount++;
    }

}
