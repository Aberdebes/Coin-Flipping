using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity, keyboardSensitivity;
	void Update ()
    {
        if (Input.GetMouseButton(0))
        { 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Translate(Vector3.forward * vertical);
        transform.Translate(Vector3.right * horizontal);
        transform.RotateAround(transform.position, Vector3.up, mouseX * mouseSensitivity);
        transform.RotateAround(transform.position, transform.right, -mouseY * mouseSensitivity);
        }
    }
}
