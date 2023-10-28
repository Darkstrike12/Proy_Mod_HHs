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
    [TextArea] public string EndingText;

    public enum EndingType
    {
        None,
        Horrible,
        Malo,
        Regular,
        Bueno
    }
}
