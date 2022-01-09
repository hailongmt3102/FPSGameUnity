using UnityEngine;
using PlayerCore;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[System.Obsolete]
public class PlayerMovement : MonoBehaviour
{
    [System.Obsolete]
    private PlayerShoot playerShootNetwork;

    private PlayerInformation playerInformation;

    [SerializeField]
    private float speed = 4f;
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
    private bool isLanding = false;
    private bool falling = false;

    // fire event
    // gun of player 
    private bool reloading = false;
    private bool firing = false;
    private float reloadingTime = 1.7f;
    private float firingTime = 0.2f;

    // pistol prefab
    public GameObject reloadPistol;
    
    // pistol logic
    private Pistol pistol;
    
    public float fireSpeed = 5f;

    // sound variable
    public AudioSource fireAudio;
    public AudioSource reloadAudio;

    // variable for AI mode
    // don't care in 2 player mode
    [SerializeField]
    private bool twoPlayerMode = true;
    [SerializeField]
    private PlayerInAIMode aiMode;

    [System.Obsolete]
    void Start()
    {
        // get some component
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        animatorController = new CharacterAnimatorController(characterAnimator);
        pistol = new Pistol();
        if (twoPlayerMode) {
            playerShootNetwork = GetComponent<PlayerShoot>();
            playerInformation = GetComponent<PlayerInformation>();
            SetPosition(playerInformation.startPos);
        }
        // try to get start pos
        try
        {
            SetPosition(playerInformation.startPos);
        }
        catch {
            Debug.Log("can't get start pos");
        }
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
        animatorController.move(Mathf.Abs(_zMove) + Mathf.Abs(_xMove), _xMove, velocity.y, _zMove, isGrounding);
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
                animatorController.Landing();
                StartCoroutine("LandingComplete");
            }
        }

        if (isGrounding && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    private void CounterEvent() {
        if (reloading) {
            reloadingTime -= Time.fixedDeltaTime;
            if (reloadingTime <= 0) {
                // reloading complete 
                pistol.Reload();
                reloading = false;
                disableReloadPrefab();
                if (twoPlayerMode)
                    playerInformation.UpdateCurrentBullet(pistol.currentBullet);
                else
                    aiMode.UpdateBullet(pistol.currentBullet);
            }
        }
        if (firing) {
            firingTime -= Time.fixedDeltaTime;
            if (firingTime <= 0) {
                firing = false;
                animatorController.StopFiring();
            }
        }
    }

    // detect some input from the player to a pistol
    private void GunEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // fire event
            if (firing)
            {
                return;
            }
            if (pistol.Fire())
            {
                // can fire
                // stop reload gun if reloading
                if (reloading) { 
                    reloading = false;
                    disableReloadPrefab();
                }
                // set firing bool and time
                firing = true;
                firingTime = 0.2f;
                Fire();
                animatorController.Fire();
                if (twoPlayerMode)
                    playerInformation.UpdateCurrentBullet(pistol.currentBullet);
                else
                    aiMode.UpdateBullet(pistol.currentBullet);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R)) {
            // reload gun
            if (!reloading) {
                reloading = true;
                reloadingTime = 1.7f;
                animatorController.Reload();
                enableReloadPrefab();
                reloadAudio.Play();
            }
        }
        
    }

    private void enableReloadPrefab() {
        reloadPistol.SetActive(true);
    }

    private void disableReloadPrefab() {
        reloadPistol.SetActive(false);
        reloadAudio.Stop();
    }

    [System.Obsolete]
    private void Fire() {
        if (twoPlayerMode)
            playerShootNetwork.Shoot(pistol.getDamage(), pistol.getRange());
        else
            aiMode.Shoot(pistol.getDamage(), pistol.getRange());    
        // call audio
        fireAudio.Play();
    }

    // some callback functions
    // callback after landing state complete
    IEnumerator LandingComplete()
    {
        yield return new WaitForSeconds(1f);
        isLanding = false;
        animatorController.DisableLanding();
    }

    public void Respawn(Vector3 pos) {
        pistol.Reload();
        SetPosition(pos);
    }

    public void SetPosition(Vector3 pos) {
        Debug.Log("move to " + pos.ToString());
        characterController.Move(pos - transform.position);
    }
}
