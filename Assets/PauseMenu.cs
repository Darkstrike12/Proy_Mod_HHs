using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public PauseState state;

    [SerializeField] GameObject menuVisual;
    [SerializeField] TextMeshProUGUI defeatedCount;
    [SerializeField] GameObject confirmVisual;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnPauseEnter()
    {
        Time.timeScale = 0f;
        menuVisual.SetActive(true);
        state = PauseState.OnPause;
        if(EnemySpawner.Instance != null)
        {
            defeatedCount.text = EnemySpawner.Instance.DefeatedEnemyCount.ToString();
        }
            }

    public void OnPauseExit()
    {
        Time.timeScale = 1f;
        menuVisual.SetActive(false);
        state = PauseState.NoPause;
        defeatedCount.text = "0";
    }

    public void OnEnterConfirmWindow()
    {
        confirmVisual.SetActive(true);
    }

    public void OnExitConfirmWindow()
    {
        confirmVisual.SetActive(false);
    }

    public void OnExitLevel()
    {

    }

    public enum PauseState
    {
        NoPause,
        OnPause,
    }
}
