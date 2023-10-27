using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float SpawnDelay;
    [SerializeField] int MaxEnemyCount;

    [Header("Enemies To Spawn")]
    [SerializeField] List<Base_Enemy> aviableEnemies;

    [Header("External References")]
    [SerializeField] GridManager gridManager;

    [Header("Events")]
    public UnityEvent OnEnemySpawned;

    //Internal Variables
    Vector2Int GridSize;
    float CurrentSpawnDelay;
    [field: SerializeField] public int CurrentEnemyCount { get; private set; }
    [field: SerializeField] public int DefeatedEnemyCount { get; private set; }

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
        GridSize = gridManager.GetGridSize();

        DefeatedEnemyCount = 0;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.left, 0.25f);
        Gizmos.DrawWireSphere(transform.position + (Vector3.left * 2), 0.25f);
    }

    #endregion

    #region Update Variables

    public void UpdateStatsOnEnemyDefeated()
    {
        CurrentEnemyCount--;
        DefeatedEnemyCount++;
    }

    public void UpdateStatsOnEnemyDestroyed()
    {
        CurrentEnemyCount--;
    }

    #endregion

    #region Spawn Enemy

    Base_Enemy SelectEnemyToSpawn()
    {
        Base_Enemy selectedEnemy = null;

        switch (GameManager.Instance.CurrentLevelState)
        {
            case GameManager.LevelState.Soft:
                SpawnDelay = (float)(Random.Range(5, 7));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 5;
                selectedEnemy = SelectEnemyBasedOnPorcentage(70, 30, 0, 0);
                break;
            case GameManager.LevelState.Medium:
                SpawnDelay = (float)(Random.Range(3, 5));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 3;
                selectedEnemy = SelectEnemyBasedOnPorcentage(15, 40, 40, 5);
                break;
            case GameManager.LevelState.Hard:
                SpawnDelay = (float)(Random.Range(1, 3));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 2;
                selectedEnemy = SelectEnemyBasedOnPorcentage(10, 20, 40, 30);
                break;
            case GameManager.LevelState.Finish:
                break;
        }

        if (selectedEnemy != null)
        {
            print($"Enemy {selectedEnemy}, enemy category {selectedEnemy.EnemyData.EnemyCategory}");
            return selectedEnemy;
        }
        else
        {
            return aviableEnemies[Random.Range(0, aviableEnemies.Count())];
        }

        Base_Enemy SelectEnemyBasedOnPorcentage(float LivingChance, float WalkingChance, float ConsientChance, float SelfconscientChance)
        {
            EnemyData.EnemyCategories selectedCategory = EnemyData.EnemyCategories.None;

            EnemyData.EnemyCategories[] catogoriesForSpawn = { 
                EnemyData.EnemyCategories.Viviente,
                EnemyData.EnemyCategories.Andante,
                EnemyData.EnemyCategories.Consiente, 
                EnemyData.EnemyCategories.Autoconsciente };

            float[] Chances = { LivingChance, WalkingChance, ConsientChance, SelfconscientChance };
            float totalChances = 0;

            float numberForAdding = 0;
            float randomNumber = Random.Range(0f, 1f);

            foreach(float num in Chances)
            {
                totalChances += num;
            }

            for (int i = 0; i < catogoriesForSpawn.Length; i++)
            {
                if (Chances[i]/ totalChances + numberForAdding >= randomNumber)
                {
                    print($"Spawn {catogoriesForSpawn[i]}");
                    selectedCategory = catogoriesForSpawn[i];
                }
                else
                {
                    numberForAdding += Chances[i] / totalChances;
                }
            }

            List<Base_Enemy> enemySpawnPool = aviableEnemies.Where(e => e.EnemyData.EnemyCategory == selectedCategory).ToList();

            //return enemySpawnPool[Random.Range(0, enemySpawnPool.Count())];

            if (enemySpawnPool.Count() > 0) return enemySpawnPool[Random.Range(0, enemySpawnPool.Count())];
            else return null;
        }
    }

    public void SpawnEnemy()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(0, GridSize.y) + gridManager.GridCellCenter().y, 0f);
        while (Physics2D.OverlapCircle(transform.position + Vector3.left, 0.25f, LayerMask.GetMask("Enemy")) || Physics2D.OverlapCircle(transform.position + (Vector3.left * 2), 0.25f, LayerMask.GetMask("Enemy")))
        {
            transform.position = new Vector3(transform.position.x, Random.Range(0, GridSize.y) + gridManager.GridCellCenter().y, 0f);
        }

        //Base_Enemy selectedEnemy = SelectEnemyToSpawn();
        //while (selectedEnemy == null)
        //{
        //    selectedEnemy = SelectEnemyToSpawn();
        //}

        //if (selectedEnemy != null)
        //{
        //    Base_Enemy EnemySpawned = Instantiate(selectedEnemy, transform.position + Vector3.left, Quaternion.identity);
        //    EnemySpawned.InitEnemy(gridManager.Grid);
        //    CurrentEnemyCount++;
        //}

        Base_Enemy EnemySpawned = Instantiate(SelectEnemyToSpawn(), transform.position + Vector3.left, Quaternion.identity);
        EnemySpawned.InitEnemy(gridManager.Grid);
        CurrentEnemyCount++;
    }

    #endregion
}
