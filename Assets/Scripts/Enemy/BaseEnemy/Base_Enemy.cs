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
    public Grid Grid { get => grid ; set => grid = value; }

    [Header("Behaviour Variables")]
    [SerializeField] public bool AllowDamage;
    [Space]
    [SerializeField] bool RandomMoveInX;
    [SerializeField] bool RandomMoveInY;
    [Space]
    [SerializeField] Vector2 MovementDuration;
    [Space]
    [SerializeField] bool JitterY;
    [SerializeField] bool JitterX;
    [Space]
    [SerializeField] AnimationCurve MovementAnimationCurveX;
    [SerializeField] AnimationCurve MovementAnimationCurveY;

    [Header("States Behaviour")]
    [SerializeField] EnemyIdleBH idleBH;
    public EnemyIdleBH IdleBH { get => idleBH; }

    [SerializeField] EnemyMovingBH movingBH;
    public EnemyMovingBH MovingBH { get => movingBH;}

    [SerializeField] EnemyAlterStateBH alterStateBH;
    public EnemyAlterStateBH AlterStateBH { get => alterStateBH; }

    [SerializeField] EnemyTakeDamageBH takeDamageBH;
    public EnemyTakeDamageBH TakeDamageBH { get => takeDamageBH;}

    [SerializeField] EnemyDeathBH deathBH;
    public EnemyDeathBH DeathBH { get => deathBH;}

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
        takeDamageBH.Initialize(this);

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

    public void TakeDamage(int DamageTaken, bool IsInstantKill)
    {
        animator.SetTrigger("TookDamage");
        CurrentHitPoints -= DamageTaken;
        if (CurrentHitPoints <= 0 || IsInstantKill) EnemyDefeated();

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

    protected virtual Vector3Int JitterYAxis(Vector3Int MovementVector, out int JitterIndex)
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

    protected virtual Vector3Int JitterXAxis(Vector3Int MovementVector, out int JitterIndex)
    {
        int JitterAxis;
        Vector3Int JitterVector;

        JitterAxis = Random.Range(1, 11) % 2;
        switch (JitterAxis)
        {
            case 0:
                JitterVector = new Vector3Int(Mathf.Abs(MovementVector.x), 0, 0);
                break;
            case 1:
                JitterVector = new Vector3Int(Mathf.Abs(MovementVector.x) * -1, 0, 0);
                break;
            default:
                JitterVector = MovementVector;
                break;
        }
        JitterIndex = JitterAxis;

        return JitterVector;
    }

    #endregion

    protected int GetAxisLimitIndex(float VectorAxys)
    {
        int AxysLimiterIndex;
        if (VectorAxys > 0)
        {
            AxysLimiterIndex = 0;
        }
        else
        {
            AxysLimiterIndex = 1;
        }
        return AxysLimiterIndex;
    }

    protected virtual bool IsTileAviable(Vector2 TargetPos)
    {
        return !Physics2D.OverlapCircle(TargetPos, 0.15f);

        //if (Physics2D.OverlapCircle(TargetPos, 0.15f).TryGetComponent(out GridTile tile))
        //{
        //    Base_Enemy enem = tile.enemy;
        //    if (enem == null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //else
        //{
        //    return false;
        //}
    }

    public virtual IEnumerator MoveEnemy(float MovementTime)
    {
        yield return new WaitForSeconds(MovementTime);
        animator.SetBool("IsMoving", true);

        Vector3 InitialPosition;
        Vector3 TargetPosition;
        float TimeElapsed;
        int JitterIndex = 0;

        #region Movement In X

        var UseRandomMoveInX = RandomMoveInX ? MovementLimit.x = Random.Range(0, Mathf.Abs(enemyData.MovementVector.x) + 1) : MovementLimit.x = enemyData.MovementVector.x;

        InitialPosition = transform.position;
        if (JitterX)
        {
            TargetPosition = InitialPosition + JitterXAxis(MovementLimit, out JitterIndex);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(-MovementLimit.x, 0, 0);
            //JitterIndex = GetAxisLimitIndex(MovementLimit.x);
        }

        while (!IsTileAviable(TargetPosition))
        {
            
            switch(JitterIndex)
            {
                case 0:
                    TargetPosition -= Vector3.right;
                    break;
                case 1:
                    TargetPosition += Vector3.right;
                    break;
                default:
                    TargetPosition -= Vector3.right;
                    break;
            }
        }

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.x)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementAnimationCurveX.Evaluate(TimeElapsed / MovementDuration.x));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

        #region Movement In Y

        var UseRandomMoveInY = RandomMoveInY ? MovementLimit.y = Random.Range(0, Mathf.Abs(enemyData.MovementVector.y) + 1) : MovementLimit.y = enemyData.MovementVector.y;

        InitialPosition = transform.position;
        if (JitterY)
        {
            TargetPosition = InitialPosition + JitterYAxis(MovementLimit, out JitterIndex);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(0, MovementLimit.y, 0);
            JitterIndex = GetAxisLimitIndex(MovementLimit.y);
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
        while (TimeElapsed < MovementDuration.y)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementAnimationCurveY.Evaluate(TimeElapsed / MovementDuration.y));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

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
