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
    public Grid Grid { set => grid = value; }

    [Header("Behaviour Variables")]
    [SerializeField] public bool AllowDamage;
    [SerializeField] bool RandomMoveInX;
    [SerializeField] bool RandomMoveInY;
    [SerializeField] float MovementDuration;
    [SerializeField] bool JitterY;
    [SerializeField] AnimationCurve MovementAnimationCurve;

    [Header("States Behaviour")]
    [SerializeField] EnemyIdleBH idleBH;
    public EnemyIdleBH IdleBH { get => idleBH; }

    [SerializeField] EnemyMovingBH movingBH;
    public EnemyMovingBH MovingBH { get => movingBH;}

    [SerializeField] EnemyAlterStateBH alterStateBH;
    public EnemyAlterStateBH AlterStateBH { get => alterStateBH; }

    //Internal Variables
    int CurrentHitPoints;
    Vector3Int MovementLimit;

    //Internal References
    Animator animator;

    private void Start()
    {
        idleBH.Initialize(this);
        movingBH.Initialize(this);
        alterStateBH.Initialize(this);

        animator = GetComponent<Animator>();

        CurrentHitPoints = enemyData.MaxHitPoints;
        transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2);
    }

    private void Update()
    {

    }

    //Constructor
    //public Base_Enemy(Grid gridRef, EnemyData enemyDataRef)
    //{
    //    grid = gridRef;
    //    enemyData = enemyDataRef;
    //}

    //Init
    public void InitEnemy(Grid gridRef, EnemyData enemyDataRef)
    {
        grid = gridRef;
        enemyData = enemyDataRef;
    }

    #region Enemy Damage Calculation

    public void TakeDamage(Base_Weapon weapon)
    {
        animator.SetTrigger("TookDamage");
        CurrentHitPoints -= weapon.WeaponDataSO.BaseDamage;
        if (CurrentHitPoints <= 0 || weapon.isInstaKill) EnemyDefeated();
        print(CurrentHitPoints.ToString());

        //var damage = weapon.weaponDataSO.AffectedEnemyMaterials.Intersect(EnemyMaterial);
    }

    public virtual void EnemyDefeated()
    {
        animator.SetTrigger("IsDefeated");
        StopAllCoroutines();
        Destroy(gameObject);
    }

    #endregion

    #region Enemy Movement

    #region JitterAxys

    Vector3Int JitterYAxis(Vector3Int MovementVector, out int JitterIndex)
    {
        int JitterAxis;
        Vector3Int JitterVector;

        JitterAxis = Random.Range(1, 11) % 2;
        switch (JitterAxis)
        {
            case 0:
                JitterVector = new Vector3Int(0, Mathf.Abs(MovementVector.y), 0);
                break;
            case 1:
                JitterVector = new Vector3Int(0, Mathf.Abs(MovementVector.y) * -1, 0);
                break;
            default:
                JitterVector = MovementVector;
                break;
        }
        JitterIndex = JitterAxis;

        return JitterVector;
    }

    Vector3Int JitterXAxis(Vector3Int MovementVector, out int JitterIndex)
    {
        int JitterAxis;
        Vector3Int JitterVector;

        JitterAxis = Random.Range(1, 11) % 2;
        switch (JitterAxis)
        {
            case 0:
                JitterVector = new Vector3Int(Mathf.Abs(MovementVector.y), 0, 0);
                break;
            case 1:
                JitterVector = new Vector3Int(Mathf.Abs(MovementVector.y) * -1, 0, 0);
                break;
            default:
                JitterVector = MovementVector;
                break;
        }
        JitterIndex = JitterAxis;

        return JitterVector;
    }

    #endregion

    int GetAxisLimitIndex(Vector3 Vector)
    {
        int AxysLimiterIndex;
        if (Vector.y > 0)
        {
            AxysLimiterIndex = 0;
        }
        else
        {
            AxysLimiterIndex = 1;
        }
        return AxysLimiterIndex;
    }

    bool IsTileAviable(Vector2 TargetPos)
    {
        //return Physics2D.OverlapCircle(TargetPos, 0.15f);

        if (Physics2D.OverlapCircle(TargetPos, 0.15f).TryGetComponent(out GridTile tile))
        {
            Base_Enemy enem = tile.enemy;
            if (enem == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public virtual IEnumerator MoveEnemy(float MovementTime)
    {
        yield return new WaitForSeconds(MovementTime);
        animator.SetBool("IsMoving", true);

        Vector3 InitialPosition;
        Vector3 TargetPosition;
        float TimeElapsed;
        int JitterIndex = 0;

        var UseRandomMoveInX = RandomMoveInX ? MovementLimit.x = Random.Range(0, Mathf.Abs(enemyData.MovementVector.x) + 1) : MovementLimit.x = enemyData.MovementVector.x;

        InitialPosition = transform.position;
        TargetPosition = InitialPosition  + new Vector3Int(-MovementLimit.x, 0, 0);

        //print(IsTileAviable(TargetPosition));
        while (!IsTileAviable(TargetPosition))
        {
            TargetPosition += Vector3.right;
            //print(IsTileAviable(TargetPosition));
        }

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementAnimationCurve.Evaluate(TimeElapsed / MovementDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        var UseRandomMoveInY = RandomMoveInY ? MovementLimit.y = Random.Range(0, Mathf.Abs(enemyData.MovementVector.y) + 1) : MovementLimit.y = enemyData.MovementVector.y;

        InitialPosition = transform.position;
        if (JitterY)
        {
            TargetPosition = InitialPosition + JitterYAxis(MovementLimit, out JitterIndex);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(0, MovementLimit.y, 0);
            JitterIndex = GetAxisLimitIndex(MovementLimit);
        }
        //var UseJitterY = JitterY ? TargetPosition = InitialPosition + JitterYAxis(MovementLimit, out JitterIndex) : TargetPosition = InitialPosition + new Vector3Int(0, MovementLimit.y, 0); //JitterIndex = GetYAxisLimitIndex(MovementLimit);
        
        //print(IsTileAviable(TargetPosition));
        while (!IsTileAviable(TargetPosition))
        {
            switch (JitterIndex)
            {
                case 0:
                    TargetPosition -= Vector3.up;
                    break;
                case 1:
                    TargetPosition += Vector3.up;
                    break;
                default:
                    TargetPosition -= Vector3.up;
                    break;
            }
            //print(IsTileAviable(TargetPosition));
        }

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementAnimationCurve.Evaluate(TimeElapsed / MovementDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        animator.SetBool("IsMoving", false);
    }

    bool ChechkForEnemy(Vector2 TargetPos)
    {
        if (Physics2D.OverlapCircle(TargetPos, 0.15f).TryGetComponent(out Base_Enemy enem))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    protected virtual void EnemyHabbility()
    {

    }
}
