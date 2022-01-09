using UnityEngine;
using System.Collections;
using UnityEngine.AI;

class StateMachine : MonoBehaviour
{
    AIState state;

    public string type = "Warrior";
    private Enemy enemy;

    private bool isattack = false;

    // Fix a range how early u want your enemy detect the obstacle.
    public int range = 5;
    // Specify the target for the enemy.
    private GameObject target;
    // Use this for initialization

    public Transform leftPos;
    public Transform rightPos;
    public Transform tailPos;
    public Transform firePos;
    public GameObject bullet;

    private AIManager aiManager;

    public AudioSource Fire;
    public AudioSource TankMoving;
    private bool ismoving = false;

    private NavMeshAgent nav;


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

        target = GameObject.Find("centerPos");
        aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
        if (target == null)
        {
            Debug.LogError("State machine: No character reference");
        }
        if (aiManager == null) { 
            Debug.LogError("State machine: No AI manager reference");
        }

        nav = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        // following player
        Walk();
        ismoving = !nav.isStopped;
        if ((target.transform.position - transform.position).magnitude < enemy.GetAttackRange())
        {
            RotateFllowPlayer();
            SetState(AIState.Attack);
        }
    }


    private void Attack() {
        if (isattack) return;
        // enemy can attack player
        Instantiate(bullet, firePos.position, Quaternion.identity).GetComponent<Bullet>().Setup(target.transform.position - transform.position, 5, enemy.GetDamage());

        isattack = true;
        StartCoroutine(SetAttackCooldown());

        Fire.Play();
        TankMoving.Pause();
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
            case AIState.Dead:
                Dead();
                break;
            case AIState.Attack:
                Attack();
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
        nav.SetDestination(target.transform.position);
        if (!ismoving) {
            TankMoving.Play();
            ismoving = true;
        }
    }

    public void Dead() {
        if (aiManager != null)
            aiManager.Remove(gameObject);
    }

    public void Damaged(int amount) {
        enemy.ReceiveDamage(amount);
        if (enemy.currentHeath <= 0) {
            SetState(AIState.Dead);
        }
    }
}

public enum AIState
{
    Walk,
    Attack,
    Dead
}
