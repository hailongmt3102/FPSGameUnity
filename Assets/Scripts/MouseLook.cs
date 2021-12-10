using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform PlayerBody;
    public float mouseSensitivity = 100f;

    // control the rotation of camera
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Update()
    {
        // get the delta position of the mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotate camera follow X axis
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // rotate player follow Y axis
        yRotation += mouseX;
        PlayerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
