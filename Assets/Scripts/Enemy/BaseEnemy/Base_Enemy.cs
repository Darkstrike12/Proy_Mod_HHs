using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] EnemyData enemyData;
    public EnemyData EnemyData { get => enemyData; set => enemyData = value; }

    [Header("External References")]
    [SerializeField] Grid grid;
    public Grid Grid { get => grid; set => grid = value; }
    [SerializeField] LayerMask DetectedLayers;

    #region Behavior Variables

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

    #endregion

    #region States Behaviour

    [Header("States Behaviour")]
    [SerializeField] EnemyIdleBH idleBH;
    public EnemyIdleBH IdleBH { get => idleBH; }

    [SerializeField] EnemyMovingBH movingBH;
    public EnemyMovingBH MovingBH { get => movingBH; }

    [SerializeField] EnemyAlterStateBH alterStateBH;
    public EnemyAlterStateBH AlterStateBH { get => alterStateBH; }

    [SerializeField] EnemyTakeDamageBH takeDamageBH;
    public EnemyTakeDamageBH TakeDamageBH { get => takeDamageBH; }

    //[SerializeField] EnemyDeathBH deathBH;
    //public EnemyDeathBH DeathBH { get => deathBH;}

    #endregion

    //Internal Variables
    int CurrentHitPoints;
    Vector3Int MovementLimit;

    //Internal References
    Animator enemAnimator;
    public Animator EnemAnimator { get => enemAnimator; }

    private void Awake()
    {
        idleBH.Initialize(this);
        movingBH.Initialize(this);
        alterStateBH.Initialize(this);
        takeDamageBH.Initialize(this);

    }

    private void Start()
    {
        //idleBH.Initialize(this);
        //movingBH.Initialize(this);
        //alterStateBH.Initialize(this);
        //takeDamageBH.Initialize(this);

        enemAnimator = GetComponent<Animator>();

        CurrentHitPoints = enemyData.MaxHitPoints;
        transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2);
    }

    private void Update()
    {

    }

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
            CurrentHitPoints -= DamageTaken;
            if (CurrentHitPoints <= 0 || IsInstantKill) EnemyDefeated();
        }
        
        //var damage = weapon.weaponDataSO.AffectedEnemyMaterials.Intersect(EnemyMaterial);
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

    public virtual IEnumerator MoveEnemy(float MovementTime)
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
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementAnimationCurveX.Evaluate(TimeElapsed / MovementDuration.x));
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
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementAnimationCurveY.Evaluate(TimeElapsed / MovementDuration.y));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

        enemAnimator.SetBool("IsMoving", false);
    }

    //public virtual IEnumerator MoveEnemy()
    //{
    //    Vector3 InitialPosition;
    //    Vector3 TargetPosition;
    //    Vector3 MovementDirection;
    //    Vector3 CurrentPos;
    //    float TimeElapsed;

    //    #region Movement In X

    //    var UseRandomMoveInX = RandomMoveInX ? MovementLimit.x = Random.Range(0, Mathf.Abs(enemyData.MovementVector.x)) : MovementLimit.x = enemyData.MovementVector.x;

    //    InitialPosition = transform.position;
    //    if (JitterX)
    //    {
    //        TargetPosition = InitialPosition + JitterXAxis(MovementLimit);
    //    }
    //    else
    //    {
    //        TargetPosition = InitialPosition + new Vector3Int(-MovementLimit.x, 0, 0);
    //    }

    //    MovementDirection = (TargetPosition - InitialPosition).normalized;
    //    CurrentPos = InitialPosition + MovementDirection;
    //    switch (MovementDirection.x)
    //    {
    //        case 1:
    //            while (IsTileAviable(CurrentPos) && CurrentPos.x < TargetPosition.x)
    //            {
    //                CurrentPos += MovementDirection;
    //            }
    //            while (!IsTileAviable(CurrentPos))
    //            {
    //                CurrentPos -= MovementDirection;
    //            }
    //            break;

    //        case -1:
    //            while (IsTileAviable(CurrentPos) && CurrentPos.x > TargetPosition.x)
    //            {
    //                CurrentPos += MovementDirection;
    //            }
    //            while (!IsTileAviable(CurrentPos))
    //            {
    //                CurrentPos -= MovementDirection;
    //            }
    //            break;

    //        default:
    //            while (IsTileAviable(CurrentPos) && CurrentPos.x < TargetPosition.x)
    //            {
    //                CurrentPos += MovementDirection;
    //            }
    //            while (!IsTileAviable(CurrentPos))
    //            {
    //                CurrentPos -= MovementDirection;
    //            }
    //            break;
    //    }
    //    TargetPosition = CurrentPos;

    //    TimeElapsed = 0f;
    //    while (TimeElapsed < MovementDuration.x)
    //    {
    //        transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementAnimationCurveX.Evaluate(TimeElapsed / MovementDuration.x));
    //        TimeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    transform.position = TargetPosition;

    //    #endregion

    //    #region Movement In Y

    //    var UseRandomMoveInY = RandomMoveInY ? MovementLimit.y = Random.Range(0, Mathf.Abs(enemyData.MovementVector.y)) : MovementLimit.y = enemyData.MovementVector.y;

    //    InitialPosition = transform.position;
    //    if (JitterY)
    //    {
    //        TargetPosition = InitialPosition + JitterYAxis(MovementLimit);
    //    }
    //    else
    //    {
    //        TargetPosition = InitialPosition + new Vector3Int(0, MovementLimit.y, 0);
    //    }

    //    MovementDirection = (TargetPosition - InitialPosition).normalized;
    //    CurrentPos = InitialPosition + MovementDirection;
    //    switch (MovementDirection.y)
    //    {
    //        case 1:
    //            while (IsTileAviable(CurrentPos) && CurrentPos.y < TargetPosition.y)
    //            {
    //                CurrentPos += MovementDirection;
    //            }
    //            while (!IsTileAviable(CurrentPos))
    //            {
    //                CurrentPos -= MovementDirection;
    //            }
    //            break;

    //        case -1:
    //            while (IsTileAviable(CurrentPos) && CurrentPos.y > TargetPosition.y)
    //            {
    //                CurrentPos += MovementDirection;
    //            }
    //            while (!IsTileAviable(CurrentPos))
    //            {
    //                CurrentPos -= MovementDirection;
    //            }
    //            break;

    //        default:
    //            while (IsTileAviable(CurrentPos) && CurrentPos.y < TargetPosition.y)
    //            {
    //                CurrentPos += MovementDirection;
    //            }
    //            while (!IsTileAviable(CurrentPos))
    //            {
    //                CurrentPos -= MovementDirection;
    //            }
    //            break;
    //    }
    //    TargetPosition = CurrentPos;

    //    TimeElapsed = 0f;
    //    while (TimeElapsed < MovementDuration.y)
    //    {
    //        transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementAnimationCurveY.Evaluate(TimeElapsed / MovementDuration.y));
    //        TimeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    transform.position = TargetPosition;

    //    #endregion

    //    enemAnimator.SetBool("IsMoving", false);
    //}

    public virtual IEnumerator OverrideMoveEnemy()
    {
        yield return null;
    }

    #endregion

    protected virtual void EnemyHabbility()
    {

    }
}
