using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BotellaNoBiodegradable : Base_Enemy
{
    public override IEnumerator MoveCR(float MovementTime)
    {
        yield return new WaitForSeconds(MovementTime);
        EnemAnimator.SetBool("IsMoving", true);

        Vector3 targetPosition = Vector3.zero;
        Vector3 initialPosition = transform.position;

        var UseRandomMoveInX = RandomMoveInX ? MovementLimit.x = -Random.Range(1, Mathf.Abs(enemyData.MovementVector.x + 1)) : MovementLimit.x = -enemyData.MovementVector.x;
        var UseRandomMoveInY = RandomMoveInY ? MovementLimit.y = Random.Range(1, Mathf.Abs(enemyData.MovementVector.y + 1)) : MovementLimit.y = enemyData.MovementVector.y;

        do
        {
            if (JitterY)
            {
                targetPosition = initialPosition + JitterAxys(Axis.Y, MovementLimit);
                targetPosition = initialPosition + new Vector3(MovementLimit.x, JitterAxys(Axis.Y, MovementLimit).y);
            }
            else
            {
                targetPosition = initialPosition + MovementLimit;
            }
        } while (!IsTileAviable(targetPosition));

        float TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.x)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, MovementCurves[0].Evaluate(TimeElapsed / MovementDuration.x));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        EnemAnimator.SetBool("IsMoving", false);
    }
}
