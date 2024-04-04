using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScr : MonoBehaviour
{
    public Shotgun shotgun;
    
    private Rigidbody2D rigidBody2D;
    private Vector2 direction;
    private Vector3 mousePosition;

    public GameObject mouse;
    public GameObject vis;
    private Camera cam;
    
    public float speed;
    public int bulletNum;
    
    private void Awake()
    {
        shotgun = GetComponentInChildren<Shotgun>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }
    private void Update()
    {
        GetMousePosition();
        PlayerInput();
        PlayerLook();
        PlayerMove();
    }

    private void GetMousePosition()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    private void PlayerInput()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
    }
    private void PlayerLook()
    {
        mouse.transform.position = mousePosition + new Vector3(0, 0, 10);
        vis.transform.localRotation = Quaternion.Euler(0,0, Mathf.Rad2Deg * Mathf.Atan2(vis.transform.position.y - mousePosition.y,
            vis.transform.position.x - mousePosition.x) + 90);
        //print(Mathf.Rad2Deg * Mathf.Atan2(vis.transform.position.y - mousePosition.y,
        //    vis.transform.position.x - mousePosition.x));
        //cam.transform.position = Vector3.Lerp(transform.position, mouse.transform.position, 0.25f) + new Vector3(0, 0 - 10);
    }
    private void PlayerMove()
    {
        rigidBody2D.velocity = direction * speed;
    }
    private void Shoot()
    {
        bulletNum--;
        //Uiwadawa.ins.UpdateUi(bulletNum);
        Debug.DrawRay(transform.position, vis.transform.up * 5, Color.red, 0.5f);
        Debug.DrawRay(transform.position, (vis.transform.up - -vis.transform.right * 0.25f).normalized * 5, Color.red, 0.5f);
        Debug.DrawRay(transform.position, (vis.transform.up -  vis.transform.right * 0.25f).normalized * 5, Color.red, 0.5f);
        shotgun.Shoot(vis.transform.up);
        CameraFuck();
        
        //Debug.DrawRay(transform.position, mouse.transform.position, Color.red, 0.5f);
    }
    private void CameraFuck()
    {
        StopAllCoroutines();
        cam.transform.localPosition = new Vector3(0, 0, -10f);
        StartCoroutine(Fuck());

    }
    private IEnumerator Fuck()
    {
        Vector3 cameraPos = cam.transform.position;
        float shakeAmount = 0.15f;
        float shakeTime = 0.15f;
        float t = 0;
        while (t < shakeTime)
        {
            yield return null;
            t += Time.deltaTime;
            Vector3 shakedPosition = Random.insideUnitCircle * shakeAmount;
            shakedPosition.z = cam.transform.position.z;
            cam.transform.position += shakedPosition;

        }
        //cam.transform.position = cameraPos;
        cam.transform.localPosition = new Vector3(0, 0, -10f);
        
    }

}