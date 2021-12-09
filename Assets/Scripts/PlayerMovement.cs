using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float lookSensitivity = 15f;

    public float gravity = -9.81f;

    private CharacterController characterController;

    private Vector3 velocity;

    public Transform grounding;

    public float groundRadius = 0.5f;

    public LayerMask groundMask;

    public float JumpHeight = 0.3f;

    private bool isGrounding;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounding = Physics.CheckSphere(grounding.position, groundRadius, groundMask);

        if (isGrounding && velocity.y < 0f) {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounding) {
            velocity.y = Mathf.Abs(JumpHeight * -2f * gravity);
        }

        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");

        Vector3 direction = transform.right * _xMove + transform.forward * _zMove;

        characterController.Move(direction * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }
}
