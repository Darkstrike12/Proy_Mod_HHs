using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Fmod Events")]
    [SerializeField] FmodEvent bgmEvent;
    [SerializeField] FmodEvent ambienceEvent;
    [SerializeField] float ambienceIntensity;

    //Event Instances
    EventInstance MainBGM;
    EventInstance AmbienceSound;

    //Singleton Instance
    public static AudioManager Instance;

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
        InitializeBMG(bgmEvent.Event);
        InitializeAmbience(ambienceEvent.Event);

        ChangeAmbienceIntensity(ambienceIntensity);
    }

    private void OnDestroy()
    {
        StopBGM();
        StopAmbienceSound();
    }

    #endregion

    public EventInstance CreateEventInstance(EventReference fmodEventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEventReference);
        return eventInstance;
    }

    #region Ambience Control

    void InitializeAmbience(EventReference AmbienceRef)
    {
        AmbienceSound = CreateEventInstance(AmbienceRef);
        AmbienceSound.start();
    }

    public void ChangeAmbienceIntensity(float intensity)
    {
        AmbienceSound.setParameterByName("Ambience_Intensity", intensity);
    }

    public void StopAmbienceSound()
    {
        AmbienceSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AmbienceSound.release();
    }

    #endregion

    #region BGM Control

    void InitializeBMG(EventReference BGMEventRef)
    {
        MainBGM = CreateEventInstance(BGMEventRef);
        MainBGM.start();
    }

    public void ChangeBGMIntensity(GameManager.LevelState levelState)
    {
        //int numLevelState = 0;
        //switch (levelState)
        //{
        //    case GameManager.LevelState.Soft:
        //        numLevelState = 0;
        //        break;
        //    case GameManager.LevelState.Medium:
        //        numLevelState = 1;
        //        break;
        //    case GameManager.LevelState.Hard:
        //        numLevelState = 2;
        //        break;
        //    case GameManager.LevelState.Finish:
        //        MainBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //        MainBGM.release();
        //        return;
        //}
        MainBGM.setParameterByName("Gameplay_Music_Intensity", ((int)levelState));
    }

    public void ChangeBGMIntensity(int musicIntensity)
    {
        MainBGM.setParameterByName("Gameplay_Music_Intensity", musicIntensity);
    }

    public void ChangeEndingSound(EndingData.EndingType endingType)
    {
        MainBGM.setParameterByName("Ending_Type", ((int)endingType));
    }

    public void StopBGM()
    {
        MainBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        MainBGM.release();
    }

    #endregion

    public void PlaySound(FmodEvent eventToPlay)
    {
        RuntimeManager.PlayOneShot(eventToPlay.Event);
    }

    public void PlaySound(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
    }
}
