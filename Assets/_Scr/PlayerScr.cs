using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScr : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private Vector2 direction;
    public GameObject mouse;
    public GameObject vis;
    public float speed;
    private Camera cam;
    public int bulletNum;
    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }
    private void Update()
    {
        PlayerInput();
        PlayerLook();
        PlayerMove();
    }
    private void PlayerInput()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
    }
    private void PlayerLook()
    {
        mouse.transform.position = cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        //cam.transform.position = Vector3.Lerp(transform.position, mouse.transform.position, 0.25f) + new Vector3(0, 0 - 10);
    }
    private void PlayerMove()
    {
        rigidBody2D.velocity = direction * speed;
    }
    private void Shoot()
    {
        bulletNum--;
        Debug.DrawRay(transform.position, mouse.transform.position, Color.red, 0.5f);
    }

}