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
    [SerializeField] List<Base_Enemy> aviableEnemiesToSpawn;

    [Header("External References")]
    [SerializeField] GridManager gridManager;

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
        GridSize = gridManager.GetGridSize();

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
        Base_Enemy selectedEnemy;

        switch (GameManager.Instance.CurrentLevelState)
        {
            case GameManager.LevelState.Soft:
                SpawnDelay = (float)(Random.Range(5, 7));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 5;
                SelectEnemyBasedOnPorcentage(60, 30, 10, 0);
                break;
            case GameManager.LevelState.Medium:
                SpawnDelay = (float)(Random.Range(3, 5));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 3;
                SelectEnemyBasedOnPorcentage(10, 40, 40, 10);
                break;
            case GameManager.LevelState.Hard:
                SpawnDelay = (float)(Random.Range(1, 3));
                MaxEnemyCount = GameManager.Instance.TotalEnemiesOnLevel / 2;
                SelectEnemyBasedOnPorcentage(10, 20, 40, 30);
                break;
            case GameManager.LevelState.Finish:
                break;
        }

        return aviableEnemiesToSpawn[0];

        void SelectEnemyBasedOnPorcentage(float LivingChance, float WalkingChance, float ConsientChance, float SelfconscientChance)
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
                }
                else
                {
                    numberForAdding += Chances[i] / totalChances;
                }
            }

            //float[] wheights = { LivingChance, WalkingChance, ConsientChance, SelfconscientChance };
            //float accumulatedWheights = 0;

            //foreach (float w in wheights)
            //{
            //    accumulatedWheights += w;
            //}

            //float randNumber = Random.Range(0, accumulatedWheights);

            //float runningTotal = 0;
            //for (int i = 0; i < wheights.Length; i++)
            //{
            //    runningTotal += wheights[i];
            //    if (randNumber < runningTotal)
            //    {
            //        selectedCategory = catogoriesForSpawn[i];
            //        print($"Cateogry to spawn {selectedCategory}");
            //    }
            //}

            //switch (randNumber)
            //{
            //    case float val when val > 0 && val < LivingChance:
            //        selectedCategory = EnemyData.EnemyCategories.Viviente;
            //        break;
            //    case float val when val > LivingChance && val < WalkingChance:
            //        selectedCategory = EnemyData.EnemyCategories.Andante;
            //        break;
            //    case float val when val > WalkingChance && val < ConsientChance:
            //        selectedCategory = EnemyData.EnemyCategories.Andante;
            //        break;
            //    case float val when val > ConsientChance && val < SelfconscientChance:
            //        selectedCategory = EnemyData.EnemyCategories.Andante;
            //        break;
            //    default:
            //        selectedCategory = EnemyData.EnemyCategories.None;
            //        break;
            //}

            IEnumerable<Base_Enemy> enemySpawnPool = aviableEnemiesToSpawn.Where(e => e.EnemyData.EnemyCategory == selectedCategory);

            //return enemySpawnPool[Random.Range(0, enemySpawnPool.Count)];
        }
    }

    public void SpawnEnemy()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(0, GridSize.y) + gridManager.GridCellCenter().y, 0f);
        while (Physics2D.OverlapCircle(transform.position + Vector3.left, 0.25f, LayerMask.GetMask("Enemy")) || Physics2D.OverlapCircle(transform.position + (Vector3.left * 2), 0.25f, LayerMask.GetMask("Enemy")))
        {
            transform.position = new Vector3(transform.position.x, Random.Range(0, GridSize.y) + gridManager.GridCellCenter().y, 0f);
        }
        Base_Enemy EnemySpawned = Instantiate(SelectEnemyToSpawn(), transform.position + Vector3.left, Quaternion.identity);
        //EnemySpawned.transform.parent = transform;
        EnemySpawned.InitEnemy(gridManager.Grid);
        CurrentEnemyCount++;
    }

    #endregion
}
