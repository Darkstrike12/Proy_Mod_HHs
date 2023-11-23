using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColadorDePlastico : Base_Weapon
{
    [Header("Unique Variables")]
    [SerializeField] float movementDuration = 1;
    [SerializeField] AnimationCurve movementCurve;

    Vector3 posChecker;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(HitPosition, new Vector3(weaponDataSO.AttackRange.x, weaponDataSO.AttackRange.y));
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(posChecker, 0.25f);
    }

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

        movementDuration = (finalPos.x - initialPos.x) / 8f;

        return finalPos;
    }

    IEnumerator MoveForward(Vector3 hitPoint)
    {
        float timeElapsed = 0;
        Vector3 initalPos = transform.position;
        Vector3 targetPos = LimitChecker();
        //print($"from {transform.position} to {targetPos}");
        animator.SetTrigger("Hit");

        while (timeElapsed < movementDuration)
        {
            transform.position = Vector3.Lerp(initalPos, targetPos, movementCurve.Evaluate(timeElapsed / movementDuration));
            timeElapsed += Time.deltaTime;
            DamageOnce(transform.position);
            posChecker = targetPos;
            yield return null;
        }
        transform.position = targetPos;
        movementDuration = 0f;
        Invoke("DisableWeapon", EffectDuration);
        //Destroy(gameObject, destroyDelay);
    }

    #endregion
}
