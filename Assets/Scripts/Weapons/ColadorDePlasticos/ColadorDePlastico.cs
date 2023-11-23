using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColadorDePlastico : Base_Weapon
{
    [Header("Unique Variables")]
    [SerializeField] float movementDuration = 1;
    [SerializeField] AnimationCurve movementCurve;

    #region WeaponHit

    protected override void SpecialEffect(Base_Enemy enemy)
    {
        if(IsWeaponEffective(enemy))
        {
            enemy.TakeDamage(0, true);
        }
    }

    public override void HitOnPosition(Vector3 hitPoint)
    {
        RigidBody.velocity = Vector3.Lerp(RigidBody.velocity, Vector3.zero, 5f);
        transform.position = Vector3.Lerp(transform.position, hitPoint, 5f);
        StartCoroutine(MoveForward(hitPoint));
    }

    Vector3 LimitChecker()
    {
        Vector3 initialPos = transform.position;
        Vector3 finalPos = initialPos;

        while (!Physics2D.OverlapCircle(finalPos + Vector3.right, 0.25f, LayerMask.GetMask("BarrierGrid")))
        {
            finalPos += Vector3.right;
        }

        movementDuration = (finalPos.x - initialPos.x) * 5.5f;

        return finalPos;
    }

    IEnumerator MoveForward(Vector3 hitPoint)
    {
        float timeElapsed = 0;
        Vector3 targetPos = LimitChecker();
        //print($"from {transform.position} to {targetPos}");

        while (timeElapsed < movementDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, movementCurve.Evaluate(timeElapsed / movementDuration));
            timeElapsed += Time.deltaTime;
            DamageOnce(transform.position);
            yield return null;
        }
        transform.position = targetPos;
        movementDuration = 0f;
        //Destroy(gameObject, destroyDelay);
    }

    #endregion
}
