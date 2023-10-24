using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Fmod Events/New Event")]
public class FmodEvent : ScriptableObject
{
    public string Name;
    public int Priority;
    public EventReference Event;
    public EventCategory Category;

    void InitializeData()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    //EventReference GetEventReferenceByName(string name)
    //{
    //    if (name == Name)
    //    {
    //        return Event;
    //    }
    //    else
    //    {
    //        Debug.Log($"Event {name} not found");
    //        return 
    //    }
    //}

    public enum EventCategory
    {
        None,
        BackgroundMusic,
        SFX
    }
}
