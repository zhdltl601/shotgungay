using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyMove : MonoBehaviour
{
    private EnemyState _enemyState;
    private Queue<Vector2> wayQueue = new Queue<Vector2>();
    public float moveSpeed;
    public float rotateSpeed;
    private bool doMove = false;

    private void Start()
    {
        _enemyState = GetComponent<EnemyState>();
    }

    public void SetMoveAble(bool domovement)
    {
        doMove = domovement;
    }
    public void SetQueue(Queue<Vector2> q)
    {
        wayQueue = q;
        StopAllCoroutines();
        StartCoroutine(IMove());
    }

    public void StartAiming(Vector3 targetPos)
    {
        StartCoroutine(DoAim(targetPos));
    }
    IEnumerator IMove()
    {
        while (wayQueue.Count != 0 && doMove)
        {
            Vector2 startPoint = transform.position;
            Vector2 endPoint = wayQueue.First();
            wayQueue.Dequeue();

            Vector2 lookdir = endPoint - startPoint;
            float time = 0f;
            while (time * moveSpeed < 10f)
            {
                float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                transform.position = Vector3.Lerp(startPoint, endPoint, time * moveSpeed / 10f);
                yield return null;
                time += Time.deltaTime;
            }
        }
    }

    IEnumerator DoAim(Vector3 targetPosition)
    {
        Vector3 v = (targetPosition - transform.position);
        float rotation = Mathf.Acos(Vector3.Dot(transform.up, v.normalized)) * Mathf.Rad2Deg;
        bool isclockwise = Vector3.Cross(transform.up, v).z < 0;
        Debug.DrawRay(transform.position, transform.up * 10, Color.blue);
        Debug.DrawRay(transform.position, v, Color.red);

        
        if (isclockwise)
        {
            print(rotation);
        }
        else
        {
            print(-rotation);
        }

        float taketime = 0.1f * rotation / rotateSpeed;
        float currenttime = 0;
        float currentRotation = transform.rotation.eulerAngles.z;
        while (currenttime < taketime)
        {
            yield return null;
            currenttime += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0,0, Mathf.LerpAngle(currentRotation, rotation, currenttime / taketime));
        }
        _enemyState.PlayerAimed();
    }

    
    
}
