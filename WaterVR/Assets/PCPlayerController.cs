using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerController : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float speedMovement = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Start()
    {
        
    }

    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, speedMovement * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -speedMovement * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(new Vector3(0, speedMovement * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(new Vector3(0, -speedMovement * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speedMovement * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speedMovement * Time.deltaTime, 0, 0));
        }
    }
}
