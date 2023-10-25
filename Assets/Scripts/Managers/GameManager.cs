using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Variables")]
    [Range(1, 100)]
    [SerializeField] int initialRecyclePoints;
    [field: SerializeField] public int TotalEnemiesOnLevel { get; private set; }

    [Header("External References")]
    [SerializeField] TextMeshProUGUI RecyclePointsCounter;

    [Header("Internal Variables")]
    //Internal Variables
    [Space(4)]
    int testInt;
    [field: SerializeField]  public int RemainingEnemies {  get; private set; }
    [field: SerializeField] public int CurrentRecyclePoints { get; private set; }
    [field: SerializeField] public LevelState CurrentLevelState { get; private set; }

    //Intanse
    public static GameManager Instance;

    #region Unity Functions

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

    #endregion

    #region Update Variables

    public void UpdateCurrentRecyclePoints(int RecyclePoints)
    {
        CurrentRecyclePoints += RecyclePoints;
    }

    public void UpdateStatsOnEnemyDefeated(Base_Enemy enemy)
    {
        CurrentRecyclePoints += enemy.EnemyData.RecyclePointsGiven;
    }

    public void UpdateStatsOnEnemySpawned()
    {
        RemainingEnemies--;
    }

    public void UpdateStatsOnEnemyOutOfGameArea()
    {
        RemainingEnemies++;
    }

    public void UpdateLevelState()
    {
        float intensityIndicator = ((float)RemainingEnemies / (float)TotalEnemiesOnLevel) * 100f;
        intensityIndicator = math.abs(100 - intensityIndicator);

        switch (intensityIndicator)
        {
            case float i when i > 0f && i < 20f:
                CurrentLevelState = LevelState.Soft;
                AudioManager.Instance.ChangeBGMIntensity(CurrentLevelState);
                break;
            case float i when i > 20f && i < 70f:
                CurrentLevelState = LevelState.Medium;
                AudioManager.Instance.ChangeBGMIntensity(CurrentLevelState);
                break;
            case float i when i > 70f && i < 100f:
                CurrentLevelState = LevelState.Hard;
                AudioManager.Instance.ChangeBGMIntensity(CurrentLevelState);
                break;
            case float i when i >= 100f:
                CurrentLevelState = LevelState.Finish;
                break;
        }

        //print($"Intensity {intensityIndicator}, level state {CurrentLevelState}");
    }

    #endregion

    public enum LevelState
    {
        Start = -1,
        Soft = 0,
        Medium = 1,
        Hard = 2,
        Finish = 3
    }
}
