using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float SpawnDelay;
    [SerializeField] int InitialRecyclePoints;
    [SerializeField] int MaxEnemyCount;

    [Header("Enemies To Spawn")]
    [SerializeField] Base_Enemy enemy;

    [Header("External References")]
    [SerializeField] GridManager grid;
    [SerializeField] TextMeshProUGUI RecyclePointsCounter;

    [Header("Events")]
    public UnityEvent OnEnemySpawned;

    //Internal Variables
    Vector2Int GridSize;
    float CurrentSpawnDelay;
    public int CurrentEnemyCount;
    public int CurrentRecyclePoints;

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
        CurrentRecyclePoints = InitialRecyclePoints;
        CurrentEnemyCount = 0;
    }

    void Update()
    {
        RecyclePointsCounter.text = CurrentRecyclePoints.ToString();

        CurrentSpawnDelay += Time.deltaTime;
        if (CurrentSpawnDelay > SpawnDelay)
        {
            if (CurrentEnemyCount < MaxEnemyCount)
            {
                OnEnemySpawned.Invoke();
            }
            CurrentSpawnDelay = 0f;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnEnemySpawned.Invoke();
        }
    }

    #endregion

    public void SpawnEnemy()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(0, GridSize.y) + grid.GridCellCenter().y, 0f);
        Base_Enemy EnemySpawned = Instantiate(enemy, transform.position + Vector3.left, Quaternion.identity);
        //EnemySpawned.transform.parent = transform;
        EnemySpawned.InitEnemy(grid.GetGridCompnent());
        CurrentEnemyCount++;
    }

}
