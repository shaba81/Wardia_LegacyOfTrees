using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{

    public HealthBar barPrefab;
    public SpriteRenderer spriteRender;
    public Animator animator;

    public int baseDamage = 1;
    public int baseHealth = 3;
    [Range(1, 5)]
    public int range = 1;
    public float attackSpeed = 1f; //Attacks per second
    public float movementSpeed = 1f; //Attacks per second

    protected Team myTeam;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;
    protected Node startingNode;

    public Node CurrentNode => currentNode;
    public Node StartingtNode => startingNode;

    protected bool HasEnemy => currentTarget != null;
    protected bool IsInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;
    protected bool moving;
    protected Node destination;
    protected HealthBar healthbar;

    protected bool dead = false;
    protected bool canAttack = true;
    protected float waitBetweenAttack;

    public void Setup(Team team, /*Node currentNode*/ Vector3 pos)
    {
        myTeam = team;
        if (myTeam == Team.Team2)
        {
            spriteRender.flipY = true;
        }

        //this.currentNode = currentNode;
        transform.position = pos;

        //currentNode.SetOccupied(true);

        //healthbar = Instantiate(barPrefab, this.transform);
        //healthbar.Setup(this.transform, baseHealth);
    }

    protected void Start()
    {
        GameManager.Instance.OnRoundStart += OnRoundStart;
        GameManager.Instance.OnRoundEnd += OnRoundEnd;
        GameManager.Instance.OnUnitDied += OnUnitDied;
    }

    protected virtual void OnRoundStart() { }
    protected virtual void OnRoundEnd() { }
    protected virtual void OnUnitDied(BaseEntity diedUnity) { }

    protected void FindTarget()
    {
        var allEnemies = GameManager.Instance.GetEntitiesAgainst(myTeam);
        float minDistance = Mathf.Infinity;
        BaseEntity entity = null;
        foreach (BaseEntity e in allEnemies)
        {
            if (Vector3.Distance(e.transform.position, this.transform.position) <= minDistance)
            {
                minDistance = Vector3.Distance(e.transform.position, this.transform.position);
                entity = e;
            }
        }

        currentTarget = entity;
    }

    protected void MoveTowards(Node nextNode)
    {
        if(nextNode == startingNode)
        {
            transform.position = nextNode.worldPosition;
            return;
        }

        /*
        Vector3 direction = (nextNode.worldPosition - this.transform.position);
        while (transform.position != nextNode.worldPosition)
        {
            //animator.SetBool("walking", false);
            this.transform.position += direction.normalized * movementSpeed * Time.deltaTime;
        }
        //animator.SetBool("walking", true);
        */

        transform.position = nextNode.worldPosition;


    }

    protected bool Move()
    {
        destination = GridManager.Instance.GetNextNode(currentNode);
        if(destination.IsOccupied)
        {
            return false;
        }
        if (destination == null)
        {
            destination = startingNode;
        }
        int indexOfDestination = destination.index;
        Debug.Log(indexOfDestination);
        switch (indexOfDestination)
        {
            case 5:
                Debug.Log("First Column Traversed");
                destination = startingNode;
                break;
            case 11:
                Debug.Log("Second Column Traversed");
                destination = startingNode;
                break;
            case 16:
                Debug.Log("Third Column Traversed");
                destination = startingNode;
                break;
            case 22:
                Debug.Log("Fourth Column Traversed");
                destination = startingNode;
                break;
            default:
                Debug.Log("Eligible");
                break;

        }

        MoveTowards(destination);

        //Free previous node
        currentNode.SetOccupied(false);
        SetCurrentNode(destination);
        currentNode.SetOccupied(true);

        return true;

    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;


    }

    public void SetStartingNode(Node node)
    {
        startingNode = node;


    }

    public void TakeDamage(int amount)
    {
        baseHealth -= amount;
        healthbar.UpdateBar(baseHealth);

        if (baseHealth <= 0 && !dead)
        {
            dead = true;
            currentNode.SetOccupied(false);
            GameManager.Instance.UnitDead(this);
        }
    }

    protected virtual void Attack()
    {
        if (!canAttack)
            return;

        animator.SetTrigger("attack");

        waitBetweenAttack = 1 / attackSpeed;
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        canAttack = false;
        yield return null;
        animator.ResetTrigger("attack");
        yield return new WaitForSeconds(waitBetweenAttack);
        canAttack = true;
    }
}
