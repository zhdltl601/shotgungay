using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyState : MonoBehaviour
{
    private EnemyStatus _enemyStatus;
    private EnemyMove _enemyMove;
    private bool playerFound;
    private bool attacked;
    public float PlayerDetectRange;
    public float AimRange;
    private bool isAiming = false;
    [FormerlySerializedAs("player")] public GameObject target;
    private enum State
    { 
        Idle,
        Move,
        Aim,
        Shoot,
        Attacked
    };

    [SerializeField] private State _state;
    // Start is called before the first frame update
    void Start()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
        _enemyMove = GetComponent<EnemyMove>();
        _state = State.Move;
    }

    
    void CheckIsPlayerInRange()
    {
        if (PlayerDetectRange > Vector2.Distance(transform.position, target.transform.position))
        {
            if(CheckIsPlayerInSight())
            playerFound = true;
        }
        else if(PlayerDetectRange < Vector2.Distance(transform.position, target.transform.position))
        {
            playerFound = false;
        }
    }
    bool CheckIsPlayerInSight()
    {
        Vector3 targetpos = transform.position - target.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetpos, PlayerDetectRange);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void CheckState()
    {
        CheckIsPlayerInRange();
        if (_state == State.Move)
        {
            if((playerFound) && (AimRange > Vector2.Distance(transform.position, target.transform.position)))
            {
                _state = State.Aim;
            }
        }
        if (attacked)
        {
            _state = State.Attacked;
            attacked = false;
        }
    }
    
    public float checkTime;
    private float currentTime = 0f;
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > checkTime)
        {
            CheckState();
            currentTime = 0f;
        }
        switch (_state)
        {
            case State.Idle:
                    //무작위 방향 이동
                break;
            case State.Move:
                _enemyMove.SetMoveAble(true);
                break;
            case State.Aim:
                if (!isAiming)
                {
                    isAiming = true;
                }
            
                break;
            case State.Shoot:
                _enemyMove.SetMoveAble(false);
                _enemyStatus.Attack(target.transform);
                break;
            case State.Attacked:
                    //플레이어로부터 멀어지는 스크립트
                break;
        }
    }
}
