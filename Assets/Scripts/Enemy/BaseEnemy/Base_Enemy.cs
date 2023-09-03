using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] EnemyData enemyData;
    public EnemyData EnemyData {  get { return enemyData; } }

    [Header("External References")]
    [SerializeField] Grid grid;

    [Header("Behaviour Variables")]
    [SerializeField] bool AllowDamage;
    [SerializeField] float TimeBetweenCellMove;

    //Internal Variables
    int CurrentHitPoints;
    public float CurrentMovementTime;
    Vector3Int MovementLimit;

    //Internal References
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        CurrentHitPoints = enemyData.MaxHitPoints;
        CurrentMovementTime = enemyData.MovementTime;

        transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2);
    }

    private void Update()
    {

    }

    public void TakeDamage(Base_Weapon weapon)
    {
        if(CurrentHitPoints <= 0 || weapon.isInstaKill) EnemyDefeated();
        CurrentHitPoints -= weapon.weaponDataSO.BaseDamage;
        
        //var damage = weapon.weaponDataSO.AffectedEnemyMaterials.Intersect(EnemyMaterial);
    }

    void EnemyDefeated()
    {
        animator.SetTrigger("IsDefeated");
    }

    public virtual IEnumerator MoveEnemy()
    {
        yield return new WaitForSeconds(enemyData.MovementTime);
        animator.SetBool("IsMoving", true);

        //if (enemyData.RandomMoveInX) MovementLimit.x = Random.Range(0, enemyData.MovementVector.x);
        //else MovementLimit.x = enemyData.MovementVector.x;
        var RandomMoveInX = enemyData.RandomMoveInX ? MovementLimit.x = Random.Range(0, enemyData.MovementVector.x) : MovementLimit.x = enemyData.MovementVector.x;
        for (int i = 0; i < MovementLimit.x; i++)
        {
            transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2) + new Vector3Int(-1, 0, 0);
            yield return new WaitForSeconds(TimeBetweenCellMove);
        }

        //if (enemyData.RandomMoveInY) MovementLimit.y = Random.Range(0, enemyData.MovementVector.y);
        //else MovementLimit.y = enemyData.MovementVector.y;
        var UseRandomMoveInY = enemyData.RandomMoveInY ? MovementLimit.y = Random.Range(0, enemyData.MovementVector.y) : MovementLimit.y = enemyData.MovementVector.y;
        for (int j = 0; j < MovementLimit.y; j++)
        {
            transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2) + new Vector3Int(0, 1, 0);
            yield return new WaitForSeconds(TimeBetweenCellMove);
        }

        //transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2) + enemyData.Movement;

        animator.SetBool("IsMoving", false);
        CurrentMovementTime = enemyData.MovementTime;
    }
}
