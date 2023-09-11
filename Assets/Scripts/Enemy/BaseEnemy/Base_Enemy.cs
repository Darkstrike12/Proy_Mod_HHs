using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] EnemyData enemyData;
    public EnemyData EnemyData { get => enemyData; set => enemyData = value; }

    [Header("External References")]
    [SerializeField] Grid grid;

    [Header("Behaviour Variables")]
    [SerializeField] public bool AllowDamage;
    [SerializeField] float MovementDuration;
    [SerializeField] bool JitterY;
    [SerializeField] AnimationCurve MovementCurve;

    //Internal Variables
    int CurrentHitPoints;
    Vector3Int MovementLimit;

    //Internal References
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        CurrentHitPoints = enemyData.MaxHitPoints;
        transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2);
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GridTile")
        {
            StopCoroutine(MoveEnemy());
            print("Tile");
        }
    }

    public void TakeDamage(Base_Weapon weapon)
    {
        CurrentHitPoints -= weapon.WeaponDataSO.BaseDamage;
        if (CurrentHitPoints <= 0 || weapon.isInstaKill) EnemyDefeated();
        print(CurrentHitPoints.ToString());

        //var damage = weapon.weaponDataSO.AffectedEnemyMaterials.Intersect(EnemyMaterial);
    }

    void EnemyDefeated()
    {
        animator.SetTrigger("IsDefeated");
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public virtual async void MoveEnemyAs()
    {
        await Task.Delay((int)(enemyData.MovementTime * 1000));
        animator.SetBool("IsMoving", true);
        Vector3 InitialPosition;
        Vector3 TargetPosition;
        float TimeElapsed;

        var RandomMoveInX = enemyData.RandomMoveInX ? MovementLimit.x = Random.Range(0, enemyData.MovementVector.x + 1) : MovementLimit.x = enemyData.MovementVector.x;

        InitialPosition = transform.position;
        TargetPosition = grid.WorldToCell(InitialPosition) + (grid.cellSize / 2) + new Vector3Int(-MovementLimit.x, 0, 0);

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurve.Evaluate(TimeElapsed / MovementDuration));
            TimeElapsed += Time.deltaTime;
            await Task.Yield();
        }
        transform.position = TargetPosition;
        
        var UseRandomMoveInY = enemyData.RandomMoveInY ? MovementLimit.y = Random.Range(0, enemyData.MovementVector.y + 1) : MovementLimit.y = enemyData.MovementVector.y;
        InitialPosition = transform.position;
        var UseJitterY = JitterY ? TargetPosition = grid.WorldToCell(InitialPosition) + (grid.cellSize / 2) + JitterAxis(MovementLimit) : TargetPosition = grid.WorldToCell(InitialPosition) + (grid.cellSize / 2) + new Vector3Int(0, MovementLimit.y, 0);

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurve.Evaluate(TimeElapsed / MovementDuration));
            TimeElapsed += Time.deltaTime;
            await Task.Yield();
        }
        transform.position = TargetPosition;
        animator.SetBool("IsMoving", false);
    }

    Vector3Int JitterAxis(Vector3Int MovementVector)
    {
        int JitterAxis;
        Vector3Int JitterVector;

        JitterAxis = Random.Range(1, 11) % 2;
        switch (JitterAxis)
        {
            case 0:
                JitterVector = new Vector3Int(0, MovementVector.y, 0);
                break;
            case 1:
                JitterVector = new Vector3Int(0, -MovementVector.y, 0);
                break;
            default:
                JitterVector = MovementVector;
                break;
        }

        return JitterVector;
    }

    public virtual IEnumerator MoveEnemy()
    {
        yield return new WaitForSeconds(enemyData.MovementTime);
        animator.SetBool("IsMoving", true);
        Vector3 InitialPosition;
        Vector3 TargetPosition;
        float TimeElapsed;

        var RandomMoveInX = enemyData.RandomMoveInX ? MovementLimit.x = Random.Range(0, enemyData.MovementVector.x + 1) : MovementLimit.x = enemyData.MovementVector.x;

        InitialPosition = transform.position;
        TargetPosition = grid.WorldToCell(InitialPosition) + (grid.cellSize / 2) + new Vector3Int(-MovementLimit.x, 0, 0);
        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurve.Evaluate(TimeElapsed / MovementDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        var UseRandomMoveInY = enemyData.RandomMoveInY ? MovementLimit.y = Random.Range(0, enemyData.MovementVector.y + 1) : MovementLimit.y = enemyData.MovementVector.y;

        InitialPosition = transform.position;
        var UseJitterY = JitterY ? TargetPosition = grid.WorldToCell(InitialPosition) + (grid.cellSize / 2) + JitterAxis(MovementLimit) : TargetPosition = grid.WorldToCell(InitialPosition) + (grid.cellSize / 2) + new Vector3Int(0, MovementLimit.y, 0);

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurve.Evaluate(TimeElapsed / MovementDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        animator.SetBool("IsMoving", false);
    }

    bool IsAviableTile()
    {
        return true;
    }

    protected virtual void EnemyHabbility()
    {

    }
}
