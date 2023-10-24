using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFmodEvent : MonoBehaviour
{
    [SerializeField] EventReference eventToPlay;
    [SerializeField] int parameterInt;

    EventInstance eventInstance;

    void Start()
    {
        eventInstance = RuntimeManager.CreateInstance(eventToPlay);
        eventInstance.start();
    }

    void Update()
    {
        eventInstance.setParameterByName("Gameplay_Music_Intensity", parameterInt);
    }
}
