using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyState : MonoBehaviour
{
    private EnemyStatus _enemyStatus;
    private EnemyPathfinding _pathfinding;
    private bool playerFound;
    private bool attacked;
    public float DetectRange;
    public GameObject player;
    private enum State
    { 
        Idle,
        Move,
        Aim,
        Shoot,
        Attacked
    };

    private State _state;
    // Start is called before the first frame update
    void Start()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
        _pathfinding = GetComponent<EnemyPathfinding>();
        _state = State.Idle;
    }

    void CheckState()
    {
        if (_state == State.Move)
        {
            if(DetectRange/2 > Vector2.Distance(transform.position, player.transform.position))
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
    
    void CheckEnemyRange()
    {
        if (DetectRange > Vector2.Distance(transform.position, player.transform.position))
        {
            playerFound = true;
        }
        else if(2 * DetectRange < Vector2.Distance(transform.position, player.transform.position))
        {
            playerFound = false;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckEnemyRange();
        CheckState();
        switch (_state)
        {
            case State.Idle:
                    //무작위 방향 이동
                break;
            case State.Move:
                _pathfinding.DoMove();
                break;
            case State.Aim:
                if ((Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.y - transform.position.y,
                        player.transform.position.x - transform.position.x) + 90)
                    > 5)
                {
                    transform.rotation = Quaternion.Euler(0,0, transform.rotation.z - 1);
                }
                else if((Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.y - transform.position.y,
                            player.transform.position.x - transform.position.x) + 90)
                        < -5)
                {
                    transform.rotation = Quaternion.Euler(0,0, transform.rotation.z + 1);
                }
                else
                {
                    _state = State.Shoot;
                }
            
                break;
            case State.Shoot:
                    _enemyStatus.Attack(player.transform);
                break;
            case State.Attacked:
                    //플레이어로부터 멀어지는 스크립트
                break;
        }
    }
}
