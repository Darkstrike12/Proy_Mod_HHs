using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("External References")]
    [SerializeField] GridManager gridManager;
    [SerializeField] Endings levelEnginds;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI recyclePointsCounter;
    [SerializeField] GameObject levelFinished;
    [SerializeField] TextMeshProUGUI defeatedEnemyCounter;
    [SerializeField] TextMeshProUGUI endingType;

    //Public Variables
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
        recyclePointsCounter.text = CurrentRecyclePoints.ToString();

        //UpdateLevelState();
        if (RemainingEnemies == 0 && EnemySpawner.Instance.CurrentEnemyCount <= 0)
        {
            CurrentLevelState = LevelState.Finish;
            OnLEvelFinished();
        }
    }

    #endregion

    public void OnLEvelFinished()
    {
        if(CurrentLevelState == LevelState.Finish)
        {
            var selectedEnding = levelEnginds.SelectEnding(TotalEnemiesOnLevel, EnemySpawner.Instance.DefeatedEnemyCount);
            AudioManager.Instance.ChangeBGMIntensity(CurrentLevelState);
            AudioManager.Instance.ChangeEndingSound(selectedEnding.endingType);
            AudioManager.Instance.ChangeAmbienceIntensity(0.6f);

            levelFinished.SetActive(true);
            defeatedEnemyCounter.text = EnemySpawner.Instance.DefeatedEnemyCount.ToString();
            endingType.text = levelEnginds.SelectEnding(TotalEnemiesOnLevel, EnemySpawner.Instance.DefeatedEnemyCount).endingType.ToString();
            print(selectedEnding.endingType.ToString());
            print("Completition " + TotalEnemiesOnLevel / EnemySpawner.Instance.DefeatedEnemyCount * 100);
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ReloadLevel()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.StopAmbienceSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.StopAmbienceSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

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
            case float i when i >= 0f && i <= SoftThreshold:
                CurrentLevelState = LevelState.Soft;
                break;
            case float i when i > SoftThreshold && i <= MediumThreshold:
                CurrentLevelState = LevelState.Medium;
                break;
            case float i when i > MediumThreshold && i <= HardThreshold:
                CurrentLevelState = LevelState.Hard;
                break;
            //case float i when i >= HardThreshold && RemainingEnemies == 0 && EnemySpawner.Instance.CurrentEnemyCount <= 0:
            //    CurrentLevelState = LevelState.Finish;
            //    break;
        }
        AudioManager.Instance.ChangeBGMIntensity(CurrentLevelState);
        //AudioManager.Instance.ChangeAmbienceIntensity(intensityIndicator / 100);
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
