using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Scriptable Objects/Ending/New Ending")]
public class EndingData : ScriptableObject
{
    [Header("Internal Data")]
    public int EndingID;

    [Header("Display Data")]
    public string EngingName;
    public EndingType endingType;
    public Sprite EndingSprite;
    public GameObject EndingObject;
    [TextArea] public string EndingText;

    public enum EndingType
    {
        None = -1,
        Horrible = 0,
        Malo = 1,
        Regular = 2,
        Bueno = 3
    }
}
