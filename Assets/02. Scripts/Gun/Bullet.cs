using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 dir;
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 _startPos;
    private Vector3 _endPos;

    private float progression = 0f;
    
    private float _speed = 10f;
    
    // Update is called once per frame
    void Update()
    {
        progression += Time.deltaTime * _speed;
        transform.position = Vector3.Lerp(_startPos, _endPos, progression);
        
        if(progression > 1)
        Destroy(gameObject,0.1f);    
    }

    public void SetPosition(Vector3 _startPos, Vector3 _endPos)
    {
        print("bullet position setted");
        this._startPos = _startPos;
        this._endPos = _endPos;
    }
}
