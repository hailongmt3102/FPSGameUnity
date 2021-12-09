using UnityEngine;
using PlayerCore;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Rigidbody rigidbody;
    private Animator animator;

    private PlayerSetting playerSetting;


    void Start() {
        //rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        // init player properties
        playerSetting = new PlayerSetting(100, 10, 100, 1);
    }

    public void Move(Vector3 velocity){
        //if (velocity.x == 0 && velocity.z == 0)
        //{
        //    animator.SetFloat("speed", 0);
        //}
        //else
        //{
        //    animator.SetFloat("speed", 1);
        //}
        //rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }

    public void Rotate(Vector3 rotate) {
        //transform.Rotate(rotate);
        //rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotate * Time.fixedDeltaTime));
    }

    public void RotateCamera(Vector3 cameraRotation)
    {
        //if (cam != null) {
        //    cam.transform.Rotate(-cameraRotation);
        //}
    }

    public void Jump() { 
        
    }
}
