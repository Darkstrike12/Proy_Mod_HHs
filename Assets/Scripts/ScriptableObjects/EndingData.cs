using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Scriptable Objects/New Ending")]
public class EndingData : ScriptableObject
{
    public string EngingName;
    public EndingType endingType;
    public Image EndingImage;
    public Sprite EndingSprite;

    public enum EndingType
    {
        None,
    }
}
