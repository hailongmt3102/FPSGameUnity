using UnityEngine;
using PlayerCore;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    public float gravity = -9.81f;

    // grounding transfrom of the character.
    public Transform grounding;
    public float groundRadius = 0.5f;
    public LayerMask groundMask;

    // jumping height.
    public float JumpHeight = 0.3f;

    // character controller component
    private CharacterController characterController;
    // falling velocity
    private Vector3 velocity;

    // bool check when player stand on the ground.
    private bool isGrounding;

    // character animator
    private Animator characterAnimator;
    // animator controller
    private AnimatorController animatorController;

    // riging animation
    private List<RigLayer> rigLayer;

    // fire event
    private float firingTime = 0;
    public GameObject bullet;
    public Transform firePoint;
    public float fireSpeed = 5f;

    void Start()
    {
        // get some component
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponentInChildren<Animator>();
        rigLayer = GetComponentInChildren<RigBuilder>().layers;
        animatorController = new AnimatorController(characterAnimator, false, false, rigLayer[0]);
    }

    void FixedUpdate()
    {
        // check the layer of ground to detect grounding status
        if (velocity.y < 0)
        {
            // check grounding
            isGrounding = Physics.CheckSphere(grounding.position, groundRadius, groundMask);
        }

        if (isGrounding && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // calculate direction from input of player
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");
        Vector3 direction = transform.right * _xMove + transform.forward * _zMove;

        if (isGrounding)
        {
            if (_xMove != 0 || _zMove != 0)
            {
                animatorController.Walk();
            }
            else
            {
                animatorController.Idle();
            }
        }

        // jumping input
        if (Input.GetButtonDown("Jump") && isGrounding)
        {
            velocity.y = Mathf.Abs(JumpHeight * -2f * gravity);
            animatorController.Jump();
            isGrounding = false;
        }

        characterController.Move(direction * speed * Time.fixedDeltaTime);
        velocity.y += gravity * Time.fixedDeltaTime;
        characterController.Move(velocity * Time.fixedDeltaTime * 0.5f);

        // fire event
        if (Input.GetButtonDown("Fire1")) {
            animatorController.EnableFire();
            firingTime = .5f;
            Instantiate(bullet, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>().velocity = Vector3.up * fireSpeed;
        }

        if (firingTime > 0) 
            firingTime -= Time.fixedDeltaTime;
        else
            animatorController.DisableFire();
    }
}
