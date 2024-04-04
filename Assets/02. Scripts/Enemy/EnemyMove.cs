using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyMove : MonoBehaviour
{
    private Queue<Vector2> wayQueue = new Queue<Vector2>();
    public float moveSpeed;
    public float rotateSpeed;
    private bool doMove = false;

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

    IEnumerator DoAim()
    {
        while (time < moveTime)
        {
            float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            출처: https://jugung.tistory.com/83 [죽은쥐:티스토리]
            transform.position = Vector3.Lerp(startPoint, endPoint, time / moveTime);
            yield return null;
            time += Time.deltaTime;
        }
    }
}
