using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{

    public HealthBar barPrefab;
    public SpriteRenderer spriteRender;
    private Animator anim;
    public SwitchAnimator switchAnim;

    public Sprite opponentSprite;

    public int baseDamage = 1;
    public int baseHealth = 3;
    private int originalDamage;
    private int originalHealth;
    public int movement = 1;
    public bool isBuilding = false;
    protected bool isBuilder = false;
    

    public Team myTeam;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;
    protected Node startingNode;
    public bool isFirstTurn = true;

    protected List<Node> eligibleNodes = new List<Node>();
    protected List<int> positions = new List<int>();

    public Node CurrentNode => currentNode;
    public Node StartingtNode => startingNode;

    
    protected bool moving;
    protected Node destination;
    protected HealthBar healthbar;

    protected bool dead = false;
    protected bool canAttack = true;
    protected float waitBetweenAttack;

    public void Setup(Team team, /*Node currentNode*/ Vector3 pos)
    {
        myTeam = team;
        if (!isBuilding && myTeam == GameManager.Instance.GetOpposingTeam())
        {
            spriteRender.sprite = opponentSprite;
            switchAnim.SwitchFront();
        }

        //this.currentNode = currentNode;
        transform.position = pos;

        //currentNode.SetOccupied(true);

        healthbar = Instantiate(barPrefab, this.transform);
        healthbar.Setup(this.transform, baseHealth);
    }

    public void Spawn(Team team)
    {

        myTeam = team;
        if (!isBuilding)
        {
            spriteRender.sprite = opponentSprite;
        }

        //this.currentNode = GridManager.Instance.GetNodeAtIndex(nodeIndex);
        transform.position = currentNode.worldPosition;

        //currentNode.SetOccupied(true);
        //startingNode = currentNode;

        healthbar = Instantiate(barPrefab, this.transform);
        healthbar.Setup(this.transform, baseHealth);
    }

    protected void Start()
    {

        anim = GetComponent<Animator>();

        GameManager.Instance.OnRoundStart += OnRoundStart;
        //GameManager.Instance.OnRoundEnd += OnRoundEnd;
        GameManager.Instance.OnUnitDied += OnUnitDied;

        eligibleNodes = GridManager.Instance.GetFirstRow();

        AddPlacingConditions();

        if(isBuilding)
        {
            foreach(BaseEntity entity in GameManager.Instance.GetMyEntities(myTeam)) 
            {
                if (entity.isBuilder)
                {
                    eligibleNodes.Add(entity.currentNode);
                }
            }
        }

        //Here i add every tree conquered into the eligible nodes for placing
        foreach(TreeEntity t in GameManager.Instance.trees)
        {
            if (t.GetConquerer() == GameManager.Instance.myTeam)
            {
                Node tempNode = GridManager.Instance.GetNodeForTile(t.parent);
                eligibleNodes.Add(tempNode);

            }
        }

        if (!isBuilding)
        {
            foreach (BaseEntity building in GameManager.Instance.GetMyEntities(myTeam))
            {
                if(building.isBuilding)
                {
                    eligibleNodes.Add(building.currentNode);
                }
            }
        }

        originalDamage = baseDamage;
        originalHealth = baseHealth;

    }

    protected virtual void OnRoundStart() { }
    public virtual void OnRoundEnd() { }
    protected virtual void OnUnitDied(BaseEntity diedUnity) { }
    protected virtual void AddPlacingConditions() { }

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


    public int GetNextIndex(int from, int amount)
    {
        //int currentIndex = positions.IndexOf(currentNode.index);
        int indexOfDestination = 0;
        if(myTeam == Team.Team1)
        {
            indexOfDestination = from + amount;

        } else if (myTeam == Team.Team2)
        {
            indexOfDestination = from - amount;
        }
        indexOfDestination = indexOfDestination % positions.Count;
        if (indexOfDestination < 0)
            indexOfDestination = indexOfDestination + positions.Count;

        return indexOfDestination;
    }

    public int GetIndexBefore(int from, int amount)
    {
        int indexOfDestination = 0;
        if (myTeam == Team.Team1)
        {
            indexOfDestination = from - amount;

        }
        else if (myTeam == Team.Team2)
        {
            indexOfDestination = from + amount;
        }
        indexOfDestination = indexOfDestination % positions.Count;
        if (indexOfDestination < 0)
            indexOfDestination = indexOfDestination + positions.Count;

        return indexOfDestination;
    }

    public Team GetMyTeam()
    {
        return myTeam;
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

    protected bool Move(int amount)
    {

        //to get a node at a given index GridManager.Instance.graph.Nodes[index];
        int index = GetNextIndex(positions.IndexOf(currentNode.index), amount);

        Node destination = GridManager.Instance.GetNodeAtIndex(positions[index]);

        //If its occupied by a unit from enemy team
        if(GameManager.Instance.GetTroopForNode(destination) == GameManager.Instance.GetOpposingTeam())
        {
            Debug.Log("Enemy Blocking");
            return false;
        }
        //If its occupied by a unit from my team
        else if (destination.IsOccupied  && GameManager.Instance.GetTroopForNode(destination) == GameManager.Instance.myTeam)
        {
            if(amount == 1)
            {
                    destination = GridManager.Instance.GetNodeAtIndex(positions[GetIndexBefore(index, amount)]);
            }
            else if(amount > 1)
            {
                    destination = GridManager.Instance.GetNodeAtIndex(positions[GetNextIndex(index, 1)]);
            }

        }
        StartCoroutine(MoveFunction(destination.worldPosition));
        //transform.position = destination.worldPosition;

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

    //RETURNS TRUE IF DEAD
    public bool TakeDamage(int amount)
    {
        baseHealth -= amount;
        healthbar.UpdateBar(baseHealth);

        if (baseHealth <= 0 && !dead)
        {
            dead = true;
            currentNode.SetOccupied(false);
            GameManager.Instance.UnitDead(this);
            return true;
        }
        // HERE WE HAVE TO NOTIFY THE NETWORK MANAGER

        return false;
    }

    IEnumerator MoveFunction(Vector3 newPos)
    {
        float timeSinceStarted = 0f;
        anim.SetBool("Walking", true);
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, newPos, timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if (this.transform.position == newPos)
            {
                anim.SetBool("Walking", false);
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
    }

    public void Unsubscribe()
    {
        GameManager.Instance.OnRoundStart -= OnRoundStart;
        //GameManager.Instance.OnRoundEnd -= OnRoundEnd;
        GameManager.Instance.OnUnitDied -= OnUnitDied;
    }

}
