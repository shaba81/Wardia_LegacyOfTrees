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
    private int originalDamage;
    private int originalHealth;
    public int movement = 1;
    public bool isBuilding = false;
    [Range(1, 5)]
    public int range = 1;
    public float attackSpeed = 1f; //Attacks per second
    public float movementSpeed = 1f; //Attacks per second

    protected Team myTeam;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;
    protected Node startingNode;
    public bool isFirstTurn = true;

    private List<Node> eligibleNodes = new List<Node>();
    private List<int> positions = new List<int>();

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

        healthbar = Instantiate(barPrefab, this.transform);
        healthbar.Setup(this.transform, baseHealth);
    }

    protected void Start()
    {
        GameManager.Instance.OnRoundStart += OnRoundStart;
        GameManager.Instance.OnRoundEnd += OnRoundEnd;
        GameManager.Instance.OnUnitDied += OnUnitDied;

        eligibleNodes = GridManager.Instance.GetFirstRow();

        //Here i add every tree conquered into the eligible nodes for placing
        foreach(TreeEntity t in GameManager.Instance.trees)
        {
            if (t.GetConquerer() == GameManager.Instance.myTeam)
            {
                Node tempNode = GridManager.Instance.GetNodeForTile(t.parent);
                eligibleNodes.Add(tempNode);

                //Here i compute the tree neighbors.
                /*
                foreach(Node n in GridManager.Instance.Neighbors(tempNode))
                {
                    if(!eligibleNodes.Contains(n))
                        eligibleNodes.Add(n);
                }
                */

            }
        }

        originalDamage = baseDamage;
        originalHealth = baseHealth;

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

    public void GetPositions()
    {
        if (startingNode.index < 5 && startingNode.index >= 0)
        {
            positions = Enumerable.Range(0, 5).ToList();
        }
        else if (startingNode.index < 11 && startingNode.index >= 5)
        {
            positions = Enumerable.Range(5, 6).ToList();
        }
        else if (startingNode.index < 16 && startingNode.index >= 11)
        {
            positions = Enumerable.Range(11, 5).ToList();
        }
        else if (startingNode.index < 22 && startingNode.index >= 16)
        {
            positions = Enumerable.Range(16, 6).ToList();
        }
        else if (startingNode.index < 27 && startingNode.index >= 22)
        {
            positions = Enumerable.Range(22, 5).ToList();
        }
    }

    public int GetNextIndex()
    {
        int currentIndex = positions.IndexOf(currentNode.index);
        int indexOfDestination = 0;
        if(myTeam == Team.Team1)
        {
            indexOfDestination = currentIndex + movement;

        } else
        {
            indexOfDestination = currentIndex - movement;
        }
        indexOfDestination = indexOfDestination % positions.Count;

        return indexOfDestination;
    }

    public bool IsDamageBuffed()
    {
        if (originalDamage == baseDamage)
            return false;

        return true;
    }

    public bool IsHealthBuffed()
    {
        if (originalHealth == baseHealth)
            return false;

        return true;
    }

    protected bool Move()
    {

        //to get a node at a given index GridManager.Instance.graph.Nodes[index];      

        Node destination = GridManager.Instance.GetNodeAtIndex(positions[GetNextIndex()]);
        if (destination.IsOccupied)
        {
            return false;
        }
        transform.position = destination.worldPosition;

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
        GetPositions();


    }

    public List<Node> GetEligibleNodes()
    {
        return eligibleNodes;
    }

    public bool CheckDrop(Node node)
    {
        if (eligibleNodes.Contains(node))
        {
            return true;
        }

        return false;
    }

    public void ReceiveAttackBuff(int amount)
    {
        baseDamage += amount;
    }

    public void ReceiveDefenseBuff(int amount)
    {
        baseHealth += amount;
    }

    public void ResetBuffs()
    {
        baseDamage = originalDamage;
        baseHealth = originalHealth;
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

    public void Unsubscribe()
    {
        GameManager.Instance.OnRoundStart -= OnRoundStart;
        GameManager.Instance.OnRoundEnd -= OnRoundEnd;
        GameManager.Instance.OnUnitDied -= OnUnitDied;
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
