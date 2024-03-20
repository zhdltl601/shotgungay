using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float duration;
    public GameObject bulletPrefab;
    public void Attack(Transform target)
    {
        GameObject g = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        EnemyAttack a = g.GetComponent<EnemyAttack>();
        a.pos = target.position;
        a.duration = duration;
    }
}
