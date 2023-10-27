using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InheritanceTt : Base_Enemy
{
    public override IEnumerator MoveCR(float MovementTime)
    {
        //return base.MoveCR(MovementTime);
        yield return new WaitForSeconds(MovementTime);
        enemAnimator.SetBool("IsMoving", true);

        //StartCoroutine(LerpPositionToTarget(this, transform.position, transform.position + Vector3.left, MovementDuration.x, MovementCurves[0]));

        enemAnimator.SetBool("IsMoving", false);
    }
}
