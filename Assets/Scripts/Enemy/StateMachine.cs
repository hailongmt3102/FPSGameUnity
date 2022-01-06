using UnityEngine;
using System.Collections;

class StateMachine : MonoBehaviour
{
    AIState state;

    public string type = "Warrior";
    private Enemy enemy;

    private bool isattack = false;

    // Fix a range how early u want your enemy detect the obstacle.
    public int range = 5;
    private float speed;
    private bool isThereAnyThing = false;
    // Specify the target for the enemy.
    public GameObject target;
    private float rotationSpeed;
    private RaycastHit hit;
    // Use this for initialization

    public Transform leftPos;
    public Transform rightPos;
    public Transform tailPos;
    public Transform firePos;

    public GameObject bullet;


    private float width;

    public void Start()
    {
        switch (type)
        {
            case "Tank":
                enemy = new Tank();
                break;
            default:
                enemy = new Tank();
                break;
        }
        speed = enemy.GetMovingSpeed();
        rotationSpeed = 10;
        if (target == null)
        {
            Debug.LogError("Enemy movement: No character reference");
        }
        width = (rightPos.position - leftPos.position).x;
    }

    public void Update()
    {
        if ((target.transform.position - transform.position).magnitude < enemy.GetAttackRange())
        {
            RotateFllowPlayer();
            Attack();
        }
        else {
            SetState(AIState.Walk);
        }
    }

    private void Attack() {
        if (isattack) return;
        // enemy can attack player

        Instantiate(bullet, firePos.position, Quaternion.identity).GetComponent<Bullet>().Setup(target.transform.position - transform.position, 5, enemy.GetDamage());

        isattack = true;
        StartCoroutine(SetAttackCooldown());
    }

    IEnumerator SetAttackCooldown() {
        yield return new WaitForSeconds(enemy.GetAttackSpeed());
        isattack = false;
    }

    public void SetState(AIState newState) {
        switch (newState) {
            case AIState.Walk:
                Walk();
                break;
            default:
                break;
        }
    }

    private void RotateFllowPlayer() {
        Vector3 relativePos = target.transform.position - transform.position;
        relativePos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
    }

    private void Walk() {
        //Look At Somthly Towards the Target if there is nothing in front.
        if (!isThereAnyThing)
        {
            RotateFllowPlayer();
        }
        // Enemy translate in forward direction.
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        //Checking for any Obstacle in front.
        // Two rays left and right to the object to detect the obstacle.
        //Use Phyics.RayCast to detect the obstacle
        if (Physics.Raycast(leftPos.position, transform.forward, out hit, range) || Physics.Raycast(rightPos.position, transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Obstacles"))
            {
                isThereAnyThing = true;
                transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
                return;
            }
        }
        // Now Two More RayCast At The End of Object to detect that object has already pass the obsatacle.
        // Just making this boolean variable false it means there is nothing in front of object.
        if (Physics.Raycast(tailPos.position, transform.right, out hit, range) || Physics.Raycast(tailPos.position, -transform.right, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Obstacles"))
            {
                isThereAnyThing = false;
            }
        }
    }

}

public enum AIState
{
    Walk,
    Attack,
    Dead
}
