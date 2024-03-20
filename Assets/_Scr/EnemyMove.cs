using System.Collections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class EnemyMove : MonoBehaviour
{
    private Queue<Vector2> wayQueue = new Queue<Vector2>();
    public float moveTime;

    public void SetQueue(Queue<Vector2> q)
    {
        wayQueue = q;
        StopAllCoroutines();
        StartCoroutine(IMove());
    }

    IEnumerator IMove()
    {
        while (wayQueue.Count != 0)
        {
            Vector2 startPoint = transform.position;
            Vector2 endPoint = wayQueue.First();
            wayQueue.Dequeue();
            
            float time = 0f;
            while (time < moveTime)
            {
            transform.position = Vector3.Lerp(startPoint, endPoint, time / moveTime);
            yield return null;
            time += Time.deltaTime;
            }
        }
    }
}
