using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public GameObject bullet;

    public float damage=1f;
    public int bulletNum=1;
    public float shotSpreadDegree = 1f;
    public float shootRange = 1f;
    
    public void Shoot(Vector2 shootDir)
    {
        shootDir = shootDir.normalized;
        for (int i = 0; i < bulletNum; i++)
        {
            print("created");
            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
            
            //회전행렬을 사용하여 벡터 회전을 하다.
            float randomDegree = Random.Range(-shotSpreadDegree, shotSpreadDegree);
            
            Vector3 spreadDir = new Vector2(
                shootDir.x * Mathf.Cos(randomDegree *Mathf.Deg2Rad) -
                shootDir.x * Mathf.Sin(randomDegree*Mathf.Deg2Rad),
                shootDir.y * Mathf.Cos(randomDegree*Mathf.Deg2Rad) +
                shootDir.y * Mathf.Sin(randomDegree*Mathf.Deg2Rad)
            );
            Bullet ab = b.GetComponent<Bullet>();
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, spreadDir, shootRange);
            if (hit.collider != null)
            {
                    ab.SetPosition(transform.position, hit.point);
                //if ()
                {
                //    var hitcheck = hit.transform.GetComponent<Hittable>();
                    //데미지 주는 스크립트
                }
            }
            else
            {
                ab.SetPosition(transform.position, transform.position + shootRange * spreadDir.normalized);
            }
        }
    }

}
