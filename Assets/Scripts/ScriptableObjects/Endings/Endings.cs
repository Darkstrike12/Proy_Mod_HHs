using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Ending/New Ending List")]
public class Endings : ScriptableObject
{
    [Header("Endings In Level")]
    [SerializeField] List<EndingData> endings;

    [Header("EndingThreshold")]
    [SerializeField] float horribleThreshold;
    [SerializeField] float badThreshold;
    [SerializeField] float regularThreshold;
    //[SerializeField] float goodThreshold;

    public EndingData SelectEnding(int toalEnemies, int defeatedEnemies)
    {
        float defeatedRate = ((float)defeatedEnemies / (float)toalEnemies) * 100f;
        List<EndingData> selectedEndingPool = new List<EndingData>();

        switch (defeatedRate)
        {
            case float i when i >= 0 && i <= horribleThreshold:
                selectedEndingPool = endings.Where(e  => e.endingType == EndingData.EndingType.Horrible).ToList();
                break;
            case float i when i > horribleThreshold && i <= badThreshold:
                selectedEndingPool = endings.Where(e => e.endingType == EndingData.EndingType.Malo).ToList();
                break;
            case float i when i > badThreshold && i <= regularThreshold:
                selectedEndingPool = endings.Where(e => e.endingType == EndingData.EndingType.Regular).ToList();
                break;
            case float i when i >= regularThreshold:
                selectedEndingPool = endings.Where(e => e.endingType == EndingData.EndingType.Bueno).ToList();
                break;
        }

        return selectedEndingPool[Random.Range(0, selectedEndingPool.Count())];
    }

    public EndingData SelectEnding(int toalEnemies, int defeatedEnemies, float horribleThres, float badThres, float regularThres)
    {
        float defeatedRate = ((float)defeatedEnemies / (float)toalEnemies) * 100f;
        List<EndingData> selectedEndingPool = new List<EndingData>();

        switch (defeatedRate)
        {
            case float i when i >= 0 && i < horribleThres:
                selectedEndingPool = endings.Where(e => e.endingType == EndingData.EndingType.Horrible).ToList();
                break;
            case float i when i > horribleThres && i < badThres:
                selectedEndingPool = endings.Where(e => e.endingType == EndingData.EndingType.Malo).ToList();
                break;
            case float i when i > badThres && i < regularThres:
                selectedEndingPool = endings.Where(e => e.endingType == EndingData.EndingType.Regular).ToList();
                break;
            case float i when i >= regularThres:
                selectedEndingPool = endings.Where(e => e.endingType == EndingData.EndingType.Bueno).ToList();
                break;
        }

        return selectedEndingPool[Random.Range(0, selectedEndingPool.Count())];
    }

    public EndingData SelectEnding(EndingData.EndingType endingType)
    {
        List<EndingData> selectedEndingPool = selectedEndingPool = endings.Where(e => e.endingType == endingType).ToList();
        return selectedEndingPool[Random.Range(0, selectedEndingPool.Count())];
    }
}
