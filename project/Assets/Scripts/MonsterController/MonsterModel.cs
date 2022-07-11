using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class MonsterModel : MonoBehaviour
{
    [Header("Object Info")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Transform body;
    [SerializeField] private Transform aim;
    [SerializeField] private GameObject bulletPrefab;


    [Header("Monster info")]
    [SerializeField] private string monsterName = "monster";
    [SerializeField] public float chaseRadius = 3f;
    [SerializeField] public float moveRadius = 5f;
    [SerializeField] public float attackRadius = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] private float attackSpeed = 4f;
    private string carryItem = null;
    public float currentHealth;
    public float currentSpeed;
    public Vector2 spawnPosition;
    private float nextAttack;

    [Header("Monster UI")]
    [SerializeField] private Text monsterNameText;
    [SerializeField] private GaugeBar healthBar;

    private void Start()
    {
        inti();
    }
    public void inti()
    {
        transform.rotation = Quaternion.identity;
        //Monster info
        currentHealth = maxHealth;
        currentSpeed = moveSpeed;
        spawnPosition = transform.position;

        //NavMesh
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = currentSpeed;

        //set UI
        healthBar.SetMaxValue(maxHealth);
        monsterNameText.text = monsterName;
    }
    [PunRPC]
    public void setItem(string item)
    {
        carryItem = item;
    }
    public string dropItem()
    {
        return carryItem;
    }

    //basic action
    public virtual void moveTo(Vector2 target)
    {
        agent.SetDestination(target);
    }
    public virtual void rotation(Vector2 target)
    {
        Vector2 lookDir = target - (Vector2)body.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
        body.eulerAngles = Vector3.forward * angle;
    }
    public virtual void stop()
    {
        agent.SetDestination(transform.position);
    }
    public void attack()
    {
        if (Time.time >= nextAttack)
        {
            nextAttack = Time.time + attackSpeed;
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, aim.position, aim.rotation);
        }
    }

    [PunRPC]
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        healthBar.SetValue(currentHealth);
    }
    public virtual void respawn()
    {
        currentHealth = maxHealth;
        currentSpeed = moveSpeed;
        transform.position = spawnPosition;
        healthBar.SetValue(currentHealth);
    }
    //action stage
    public virtual void chasing(Vector2 target)
    {
        moveTo(target);
        rotation(target);
    }
    public virtual void goToSpawnPoint()
    {
        moveTo(spawnPosition);
        rotation(spawnPosition);
    }
    public virtual void patrol()
    {
        Debug.Log("movearound");
    }
    //check
    public virtual bool Isdead()
    {
        return currentHealth <= 0;
    }

    public bool isInMoveRange()
    {
        return Vector2.Distance(spawnPosition, transform.position) <= moveRadius;
    }

    public bool isInAttackRange(Vector2 target)
    {
        return Vector2.Distance(transform.position, target) <= attackRadius;
    }
    public bool isInSpawnPoint()
    {
        return Vector2.Distance(spawnPosition, transform.position) <= 0.5;
    }
    public GameObject FindClosestPlayer()
    {
        float distanceToClosestTarget = Mathf.Infinity;
        GameObject closestTarget = null;
        GameObject[] allTarget = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject target in allTarget)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            if (distance < chaseRadius && distance < distanceToClosestTarget && !target.GetComponent<PlayerModel>().isDead())
            {
                distanceToClosestTarget = distance;
                closestTarget = target;
            }
        }
        return closestTarget;
    }
}
