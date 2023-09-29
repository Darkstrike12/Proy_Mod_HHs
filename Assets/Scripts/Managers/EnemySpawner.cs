using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float SpawnTime;

    [Header("Enemys To Spawn")]
    [SerializeField] Base_Enemy enemy;

    [Header("External References")]
    [SerializeField] GridManager grid;

    //Internal Variables
    Vector2Int GridSize;
    public float CurrentSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        print("Start");
        GridSize = grid.GetGridSize();

        CurrentSpawnTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentSpawnTime += Time.deltaTime;
        if (CurrentSpawnTime > SpawnTime)
        {
            print(CurrentSpawnTime);
            transform.position = new Vector3(transform.position.x, Random.Range(0, GridSize.y) + grid.GridCellCenter().y, 0f) ;
            CurrentSpawnTime = 0f;
            print(CurrentSpawnTime);
        }
    }

}
