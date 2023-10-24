using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Fmod Events")]
    [SerializeField] List<FmodEvent> MusicEvents;

    //Event Instances
    EventInstance MainBGM;

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
        //InitializeBMG(MusicEvents.Find(e => e.Name == "BGM").Event);
    }

    void Update()
    {
        
    }

    #endregion

    EventInstance CreateEventInstance(EventReference fmodEventReference)
    {
        EventInstance instance = RuntimeManager.CreateInstance(fmodEventReference);
        return instance;
    }

    void InitializeBMG(EventReference BGMEventRef)
    {
        MainBGM = CreateEventInstance(BGMEventRef);
        MainBGM.start();
    }

    public void ChangeBGMIntensity(GameManager.LevelState levelState)
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
        }
        MainBGM.setParameterByName("Gameplay_Music_Intensity", numLevelState);
    }

    public void ChangeBGMIntensity(int musicIntensity)
    {
        MainBGM.setParameterByName("Gameplay_Music_Intensity", musicIntensity);
    }
}
