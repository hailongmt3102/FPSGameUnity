using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform PlayerBody;

    public float mouseSensitivity = 100f;

    public float xRotation = 0f;
    private float yRotatio = 0f;

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 80f);
            
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        yRotatio += mouseX;
        PlayerBody.localRotation = Quaternion.Euler(0f, yRotatio, 0f);
    }
}
