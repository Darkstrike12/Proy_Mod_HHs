using FMODUnity;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] EnemyData enemyData;
    public EnemyData EnemyData { get => enemyData; set => enemyData = value; }

    [Header("External References")]
    [SerializeField] Grid grid;
    public Grid Grid { get => grid; set => grid = value; }
    [SerializeField] LayerMask DetectedLayers;
    [SerializeField] EventReference TestOneSshot;

    #region Behavior Variables

    [Header("Behaviour Variables")]
    [SerializeField] public bool AllowDamage;
    [Space]
    [SerializeField] bool RandomMoveInX;
    [SerializeField] bool RandomMoveInY;
    [Space]
    public Vector2 MovementDuration;
    [Space]
    [SerializeField] bool JitterX;
    [SerializeField] bool JitterY;
    [Space]
    public AnimationCurve MovementCurveX;
    public AnimationCurve MovementCurveY;

    #endregion

    #region States Behaviour

    [Header("States Behaviour")]
    [SerializeField] EnemyIdleBH idleBH;
    public EnemyIdleBH IdleBH { get; private set; }

    [SerializeField] EnemyMovingBH movingBH;
    public EnemyMovingBH MovingBH { get; private set; }

    [SerializeField] EnemyAlterStateBH alterStateBH;
    public EnemyAlterStateBH AlterStateBH { get; private set; }

    [SerializeField] EnemyTakeDamageBH takeDamageBH;
    public EnemyTakeDamageBH TakeDamageBH { get; private set; }

    //[SerializeField] EnemyDeathBH deathBH;
    //public EnemyDeathBH DeathBH { get => deathBH;}

    #endregion

    //Internal Variables
    int CurrentHitPoints;
    Vector3Int MovementLimit;
    public Coroutine MoveCoroutine;

    //Internal References
    Animator enemAnimator;
    public Animator EnemAnimator { get => enemAnimator; }

    #region Unity Functions

    protected virtual void Awake()
    {
        IdleBH = Instantiate(idleBH);
        MovingBH = Instantiate(movingBH);
        AlterStateBH = Instantiate(alterStateBH);
        TakeDamageBH = Instantiate(takeDamageBH);
    }

    protected virtual void Start()
    {
        IdleBH.Initialize(this);
        MovingBH.Initialize(this);
        AlterStateBH.Initialize(this);
        TakeDamageBH.Initialize(this);

        enemAnimator = GetComponent<Animator>();

        CurrentHitPoints = enemyData.MaxHitPoints;
        transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2);
    }

    private void Update()
    {

    }

    protected virtual void OnDestroy()
    {
        EnemySpawner.Instance.CurrentEnemyCount--;
        EnemySpawner.Instance.CurrentRecyclePoints += enemyData.RecyclePointsGiven;
    }

    #endregion

    #region Intialize

    //Constructor
    public Base_Enemy(Grid gridRef, EnemyData enemyDataRef)
    {
        grid = gridRef;
        enemyData = enemyDataRef;
    }

    public Base_Enemy(Grid gridRef)
    {
        grid = gridRef;
    }

    //Init
    public void InitEnemy(Grid gridRef, EnemyData enemyDataRef)
    {
        grid = gridRef;
        enemyData = enemyDataRef;
    }

    public void InitEnemy(Grid gridRef)
    {
        grid = gridRef;
    }

    #endregion

    #region Enemy Damage Calculation

    public void TakeDamage(int DamageTaken, bool IsInstantKill)
    {
        if (AllowDamage)
        {
            enemAnimator.SetTrigger("TookDamage");
            RuntimeManager.PlayOneShot(TestOneSshot, transform.position);
            CurrentHitPoints -= DamageTaken;
            if (CurrentHitPoints <= 0 || IsInstantKill) EnemyDefeated();
        }
    }

    public virtual void EnemyDefeated()
    {
        StopAllCoroutines();
        enemAnimator.SetTrigger("IsDefeated");
        Destroy(gameObject);
    }

    #endregion

    #region Enemy Movement

    #region JitterAxys

    protected virtual Vector3Int JitterYAxis(Vector3Int MovementVector)
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

        return JitterVector;
    }

    protected virtual Vector3Int JitterXAxis(Vector3Int MovementVector)
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

        return JitterVector;
    }

    #endregion

    //protected int GetAxisLimitIndex(float VectorAxys)
    //{
    //    int AxysLimiterIndex;
    //    if (VectorAxys > 0)
    //    {
    //        AxysLimiterIndex = 0;
    //    }
    //    else
    //    {
    //        AxysLimiterIndex = 1;
    //    }
    //    return AxysLimiterIndex;
    //}

    protected virtual bool IsTileAviable(Vector3 TargetPos)
    {
        return !Physics2D.OverlapCircle(TargetPos, 0.15f, DetectedLayers);
    }

    public virtual IEnumerator MoveEnemyCR(float MovementTime)
    {
        yield return new WaitForSeconds(MovementTime);
        enemAnimator.SetBool("IsMoving", true);

        Vector3 InitialPosition;
        Vector3 TargetPosition;
        Vector3 MovementDirection;
        Vector3 CurrentPos;
        float TimeElapsed;

        #region Movement In X

        var UseRandomMoveInX = RandomMoveInX ? MovementLimit.x = Random.Range(0, Mathf.Abs(enemyData.MovementVector.x)) : MovementLimit.x = enemyData.MovementVector.x;

        InitialPosition = transform.position;
        if (JitterX)
        {
            TargetPosition = InitialPosition + JitterXAxis(MovementLimit);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(-MovementLimit.x, 0, 0);
        }

        MovementDirection = (TargetPosition - InitialPosition).normalized;
        CurrentPos = InitialPosition + MovementDirection;
        switch (MovementDirection.x)
        {
            case 1:
                while (IsTileAviable(CurrentPos) && CurrentPos.x < TargetPosition.x)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;

            case -1:
                while (IsTileAviable(CurrentPos) && CurrentPos.x > TargetPosition.x)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;

            default:
                while (IsTileAviable(CurrentPos) && CurrentPos.x < TargetPosition.x)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;
        }
        TargetPosition = CurrentPos;

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.x)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurveX.Evaluate(TimeElapsed / MovementDuration.x));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

        #region Movement In Y

        var UseRandomMoveInY = RandomMoveInY ? MovementLimit.y = Random.Range(0, Mathf.Abs(enemyData.MovementVector.y)) : MovementLimit.y = enemyData.MovementVector.y;

        InitialPosition = transform.position;
        if (JitterY)
        {
            TargetPosition = InitialPosition + JitterYAxis(MovementLimit);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(0, MovementLimit.y, 0);
        }

        MovementDirection = (TargetPosition - InitialPosition).normalized;
        CurrentPos = InitialPosition + MovementDirection;
        switch (MovementDirection.y)
        {
            case 1:
                while (IsTileAviable(CurrentPos) && CurrentPos.y < TargetPosition.y)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;

            case -1:
                while (IsTileAviable(CurrentPos) && CurrentPos.y > TargetPosition.y)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;

            default:
                while (IsTileAviable(CurrentPos) && CurrentPos.y < TargetPosition.y)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;
        }
        TargetPosition = CurrentPos;

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.y)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurveY.Evaluate(TimeElapsed / MovementDuration.y));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

        enemAnimator.SetBool("IsMoving", false);
    }

    public virtual IEnumerator MoveEnemyCR()
    {
        Vector3 InitialPosition;
        Vector3 TargetPosition;
        Vector3 MovementDirection;
        Vector3 CurrentPos;
        float TimeElapsed;

        #region Movement In X

        var UseRandomMoveInX = RandomMoveInX ? MovementLimit.x = Random.Range(0, Mathf.Abs(enemyData.MovementVector.x)) : MovementLimit.x = enemyData.MovementVector.x;

        InitialPosition = transform.position;
        if (JitterX)
        {
            TargetPosition = InitialPosition + JitterXAxis(MovementLimit);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(-MovementLimit.x, 0, 0);
        }

        MovementDirection = (TargetPosition - InitialPosition).normalized;
        CurrentPos = InitialPosition + MovementDirection;
        switch (MovementDirection.x)
        {
            case 1:
                while (IsTileAviable(CurrentPos) && CurrentPos.x < TargetPosition.x)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;

            case -1:
                while (IsTileAviable(CurrentPos) && CurrentPos.x > TargetPosition.x)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;

            default:
                while (IsTileAviable(CurrentPos) && CurrentPos.x < TargetPosition.x)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;
        }
        TargetPosition = CurrentPos;

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.x)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurveX.Evaluate(TimeElapsed / MovementDuration.x));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

        #region Movement In Y

        var UseRandomMoveInY = RandomMoveInY ? MovementLimit.y = Random.Range(0, Mathf.Abs(enemyData.MovementVector.y)) : MovementLimit.y = enemyData.MovementVector.y;

        InitialPosition = transform.position;
        if (JitterY)
        {
            TargetPosition = InitialPosition + JitterYAxis(MovementLimit);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(0, MovementLimit.y, 0);
        }

        MovementDirection = (TargetPosition - InitialPosition).normalized;
        CurrentPos = InitialPosition + MovementDirection;
        switch (MovementDirection.y)
        {
            case 1:
                while (IsTileAviable(CurrentPos) && CurrentPos.y < TargetPosition.y)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;

            case -1:
                while (IsTileAviable(CurrentPos) && CurrentPos.y > TargetPosition.y)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;

            default:
                while (IsTileAviable(CurrentPos) && CurrentPos.y < TargetPosition.y)
                {
                    CurrentPos += MovementDirection;
                }
                while (!IsTileAviable(CurrentPos))
                {
                    CurrentPos -= MovementDirection;
                }
                break;
        }
        TargetPosition = CurrentPos;

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.y)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurveY.Evaluate(TimeElapsed / MovementDuration.y));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

        //enemAnimator.SetBool("IsMoving", false);
    }

    public virtual IEnumerator LerpEmenyToTarget(Vector3 InitialPos, Vector3 TargetPos, float LerpDuration, AnimationCurve animationCurve)
    {
        float TimeElapsed;
        TimeElapsed = 0f;
        while (TimeElapsed < LerpDuration)
        {
            transform.position = Vector3.Lerp(InitialPos, TargetPos, animationCurve.Evaluate(TimeElapsed / LerpDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPos;
    }

    public virtual IEnumerator OverrideMoveEnemyCR()
    {
        yield return null;
    }

    #endregion

    protected virtual void EnemyAbility()
    {

    }
}
