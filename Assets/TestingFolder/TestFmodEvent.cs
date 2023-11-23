using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFmodEvent : MonoBehaviour
{
    [SerializeField] EventReference eventToPlay;
    [SerializeField] int parameterInt;
    [SerializeField] GameManager.LevelState levelState;

    EventInstance eventInstance;

    void Start()
    {
        eventInstance = RuntimeManager.CreateInstance(eventToPlay);
        eventInstance.start();
    }

    void Update()
    {
        //eventInstance.setParameterByName("Gameplay_Music_Intensity", parameterInt);
        MusicParameter(levelState);
    }

    void MusicParameter(GameManager.LevelState levelState)
    {
        int numLevelState = 0;
        switch (levelState)
        {
            case GameManager.LevelState.Soft:
                numLevelState = 0;
                break;
            case GameManager.LevelState.Medium:
                numLevelState = 1;
                break;
            case GameManager.LevelState.Hard:
                numLevelState = 2;
                break;
            case GameManager.LevelState.Finish:
                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                return;
        }
        eventInstance.setParameterByName("Gameplay_Music_Intensity", numLevelState);
    }
}
