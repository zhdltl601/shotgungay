using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Vector3 pos;
    public float duration;

    private void Awake()
    {
        pos = pos - transform.position;
        StartCoroutine(Move());
        Invoke("Die", duration);
    }

    public void Die()
    {
        StopAllCoroutines();
        Destroy(this);
    }

    IEnumerator Move()
    {
        transform.Translate(pos);
        yield return new WaitForSeconds(0.1f);
    }
}
    
    
