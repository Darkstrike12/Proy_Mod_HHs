using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Variables")]
    [SerializeField] int initialRecyclePoints;
    public int TotalEnemiesOnLevel;

    [Header("External References")]
    [SerializeField] TextMeshProUGUI RecyclePointsCounter;

    [Header("Internal Variables")]
    //Internal Variables
    public int RemainingEnemies;
    public int CurrentRecyclePoints;
    [field: SerializeField] public LevelState CurrentLevelState { get; private set; }

    //Intanse
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
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
        CurrentRecyclePoints = initialRecyclePoints;
        RemainingEnemies = TotalEnemiesOnLevel;
    }

    void Update()
    {
        RecyclePointsCounter.text = CurrentRecyclePoints.ToString();
    }

    public void UpdateStatsOnEnemyDefeated(Base_Enemy enemy)
    {
        CurrentRecyclePoints += enemy.EnemyData.RecyclePointsGiven;
    }

    public void UpdateStatsOnEnemySpawned()
    {
        RemainingEnemies--;
    }

    public void UpdateLevelState()
    {
        float intensityIndicator = ((float)RemainingEnemies / (float)TotalEnemiesOnLevel) * 100f;
        intensityIndicator = math.abs(100 - intensityIndicator);

        switch (intensityIndicator)
        {
            case float i when i > 0f && i < 30f:
                CurrentLevelState = LevelState.Soft;
                break;
            case float i when i > 30f && i < 80f:
                CurrentLevelState = LevelState.Medium;
                break;
            case float i when i > 80f && i < 100f:
                CurrentLevelState = LevelState.Hard;
                break;
            case float i when i <= 0f:
                CurrentLevelState = LevelState.Finish;
                break;
        }

        print($"Intensity {intensityIndicator}, level state {CurrentLevelState}");
    }

    public enum LevelState
    {
        Soft,
        Medium,
        Hard,
        Finish
    }
}
