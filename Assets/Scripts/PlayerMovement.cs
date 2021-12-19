using UnityEngine;
using PlayerCore;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    public float gravity = -9.81f;

    // grounding transfrom of the character.
    public Transform grounding;
    public float groundRadius = 0.1f;
    public LayerMask groundMask;

    // jumping height.
    public float JumpHeight = 0.5f;

    // character controller component
    private CharacterController characterController;
    // falling velocity
    private Vector3 velocity;

    // bool check when player stand on the ground.
    private bool isGrounding = false;

    // character animator
    private Animator characterAnimator;
    // animator controller
    private CharacterAnimatorController animatorController;

    // falling variable
    private float landingTime = 1f;
    private bool isLanding = false;
    private bool falling = false;

    // fire event
    // pistol prefab
    public GameObject pistolItem;
    public GameObject pistolHandle;
    // removing gun
    private bool isremovingGun = false;
    private float removingGunTime = 0.8f;

    // gun of player 
    // 0 is nothing, 1 is pistol
    private int guntype = 0;

    private float firingTime = 0;
    public GameObject bullet;
    public Transform firePoint;
    public float fireSpeed = 5f;

    void Start()
    {
        // get some component
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponentInChildren<Animator>();
        animatorController = new CharacterAnimatorController(characterAnimator);
    }

    void FixedUpdate()
    {
        // check the layer of ground to detect grounding status
        CheckGroundingPosition();

        // count cooldown for some envents, some items
        CounterEvent();

        // calculate direction from input of player
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");
        Vector3 direction = transform.right * _xMove + transform.forward * _zMove;

        // detect keyboard event
        // jumping input
        if (Input.GetButtonDown("Jump") && isGrounding)
        {
            Debug.Log("jump");
            velocity.y = Mathf.Abs(JumpHeight * -2f * gravity);
            isGrounding = false;
        }

        // pistol event
        GunEvent();

        // movement update
        if (!isLanding) {
            characterController.Move(direction * speed * Time.fixedDeltaTime);
            velocity.y += gravity * Time.fixedDeltaTime;
            characterController.Move(velocity * Time.fixedDeltaTime * 0.5f);
        }

        // set animator
        animatorController.movement(Mathf.Abs(_zMove) + Mathf.Abs(_xMove), _xMove, velocity.y, _zMove, isGrounding);
    }

    private void CheckGroundingPosition() {
        if (velocity.y < 0)
        {
            falling = !isGrounding;
            // check grounding
            isGrounding = Physics.CheckSphere(grounding.position, groundRadius, groundMask);
            if (falling && isGrounding)
            {
                // on landing
                isLanding = true;
                landingTime = 1f;
                animatorController.Landing();
            }
        }

        if (isGrounding && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    private void CounterEvent() {
        // check landing state
        if (isLanding)
        {
            landingTime -= Time.fixedDeltaTime;
            if (landingTime < 0f)
            {
                // finish landing state
                isLanding = false;
                animatorController.DisableLanding();
            }
        }
        if (isremovingGun) {
            removingGunTime -= Time.fixedDeltaTime;
            if (removingGunTime < 0f) {
                isremovingGun = false;
                // disable hand layer animation
                animatorController.DisableHandLayer();
                removePistol();
            }
        }
    }

    // detect some input from the player to a pistol
    private void GunEvent()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // button 1 was pressed
            // take the pistol if not holding it
            if (guntype != 1)
            {
                drawPistol();
                // set animation for pistol
                animatorController.EnableHandLayer();
                animatorController.fireEvent(1, 0);
                isremovingGun = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // button 1 was pressed
            // take the pistol if not holding it
            if (guntype != 2)
            {
                // set animation for pistol
                animatorController.fireEvent(1, 4);
                // removing time counter
                isremovingGun = true;
                removingGunTime = 0.8f;
            }
        }
        if (Input.GetButtonDown("Fire1")) { 

        }
    }

    private void drawPistol() {
        pistolItem.SetActive(false);
        pistolHandle.SetActive(true);
    }

    private void removePistol() {
        pistolItem.SetActive(true);
        pistolHandle.SetActive(false);
    }
}
