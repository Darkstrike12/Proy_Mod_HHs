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
    [Header("Level State Threshold")]
    [SerializeField] float SoftThreshold;
    [SerializeField] float MediumThreshold;
    [SerializeField] float HardThreshold;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI RecyclePointsCounter;

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
        if(RemainingEnemies == 0 && EnemySpawner.Instance.CurrentEnemyCount <= 0)
        {
            CurrentLevelState = LevelState.Finish;
        }
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
            case float i when i >= 0f && i < SoftThreshold:
                CurrentLevelState = LevelState.Soft;
                AudioManager.Instance.ChangeBGMIntensity(CurrentLevelState);
                break;
            case float i when i > SoftThreshold && i < MediumThreshold:
                CurrentLevelState = LevelState.Medium;
                AudioManager.Instance.ChangeBGMIntensity(CurrentLevelState);
                break;
            case float i when i > MediumThreshold && i < HardThreshold:
                CurrentLevelState = LevelState.Hard;
                AudioManager.Instance.ChangeBGMIntensity(CurrentLevelState);
                break;
            //case float i when i >= HardThreshold:
            //    CurrentLevelState = LevelState.Finish;
            //    break;
        }
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
