using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Variables")]
    [SerializeField] float SpawnTime;
    [SerializeField] int MaxEnemyCount;

    [Header("Enemys To Spawn")]
    [SerializeField] Base_Enemy enemy;

    [Header("External References")]
    [SerializeField] GridManager grid;

    [Header("Events")]
    public UnityEvent OnEnemySpawned;

    //Internal Variables
    Vector2Int GridSize;
    float CurrentSpawnTime;
    public int CurrentEnemyCount;

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

        CurrentSpawnTime = 0f;
        CurrentEnemyCount = 0;
    }

    void Update()
    {
        CurrentSpawnTime += Time.deltaTime;
        if (CurrentSpawnTime > SpawnTime)
        {
            if (CurrentEnemyCount < MaxEnemyCount)
            {
                OnEnemySpawned.Invoke();
            }
            CurrentSpawnTime = 0f;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnEnemySpawned.Invoke();
        }
    }

    public void SpawnEnemy()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(0, GridSize.y) + grid.GridCellCenter().y, 0f);
        Base_Enemy EnemySpawned = Instantiate(enemy, transform.position + Vector3.left, Quaternion.identity);
        //EnemySpawned.transform.parent = transform;
        EnemySpawned.InitEnemy(grid.GetGridCompnent());
        CurrentEnemyCount++;
    }

}
