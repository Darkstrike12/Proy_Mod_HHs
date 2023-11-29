using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitTile : GridTile
{
    [SerializeField] float DestroyDelay;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float lerpDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsEnemyOnTile(collision, out Base_Enemy enemy))
        {
            StartCoroutine(LerpToExit(collision.gameObject, collision.gameObject.transform.position, transform.position - targetPosition, lerpDuration));
        }
    }

    IEnumerator LerpToExit(GameObject obj, Vector3 initalPos, Vector3 finalPos, float lerpDuration)
    {
        float timeElapsed = 0;
        while(timeElapsed < lerpDuration)
        {
            obj.transform.position = Vector3.Lerp(initalPos, finalPos, timeElapsed/lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = finalPos;
    }
}

