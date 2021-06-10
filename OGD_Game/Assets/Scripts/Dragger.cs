using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    Camera mainCamera;
    float zAxis = 0;
    Vector3 clickOffset = new Vector3(0, -0.4f, 0);

    private Vector3 oldPosition;
    public LayerMask releaseMask;
    private Tile previousTile = null;

    private BaseEntity thisEntity;
    private List<Tile> eligibleTiles = new List<Tile>();

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera.GetComponent<Physics2DRaycaster>() == null)
            mainCamera.gameObject.AddComponent<Physics2DRaycaster>();

        //light eligible nodes
        thisEntity = GetComponent<BaseEntity>();
        foreach (Node n in thisEntity.GetEligibleNodes())
        {
            Tile t = GridManager.Instance.GetTileForNode(n);
            eligibleTiles.Add(t);

        }

        foreach (Tile t in eligibleTiles)
        {
            t.SetEligibleHighlight(true);
        }
    }

    private void Update()
    {


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (TurnManager.Instance.gameState != GameState.Placing)
            return;

        oldPosition = this.transform.position;
        zAxis = transform.position.z;
        clickOffset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zAxis)) + new Vector3(0, 3, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, zAxis);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Use Offset To Prevent Sprite from Jumping to where the finger is
        Vector3 tempVec = mainCamera.ScreenToWorldPoint(eventData.position);
        tempVec.z = zAxis; //Make sure that the z zxis never change


        transform.position = tempVec;

        Tile tileUnder = GetTileUnder(transform.position);
        if (tileUnder != null)
        {
            if (eligibleTiles.Contains(tileUnder))
            {
                if(!thisEntity.isBuilding)
                    tileUnder.SetHighlight(true, !GridManager.Instance.GetNodeForTile(tileUnder).IsOccupied);
                else
                    tileUnder.SetHighlight(true, true);
            }
            else
            {
                tileUnder.SetHighlight(true, false);
            }

            if (previousTile != null && tileUnder != previousTile)
            {
                //We are over a different tile.
                previousTile.SetHighlight(false, false);
            }

            previousTile = tileUnder;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        if (!TryRelease(transform.position))
        {
            //Nothing was found, return to original position.
            this.transform.position = oldPosition;
        }

        if (previousTile != null)
        {
            previousTile.SetHighlight(false, false);
            previousTile = null;
        }
    }

    private bool TryRelease(Vector3 position)
    {
        //Released over something!
        Tile t = GetTileUnder(position);
        if (t != null)
        {
            //It's a tile!
            thisEntity = GetComponent<BaseEntity>();
            Node candidateNode = GridManager.Instance.GetNodeForTile(t);
            if (candidateNode != null && thisEntity != null)
            {
                if (!thisEntity.CheckDrop(candidateNode))
                    return false;

                if (thisEntity.isBuilding)
                {
                    if (candidateNode.IsOccupied)
                    {
                        foreach(BaseEntity entity in GameManager.Instance.GetMyEntities(GameManager.Instance.myTeam))
                        {
                            if(entity.CurrentNode == candidateNode)
                            {
                                entity.TakeDamage(entity.baseHealth);
                                break;
                            }
                        }
                    }
                    thisEntity.SetCurrentNode(candidateNode);
                    thisEntity.SetStartingNode(candidateNode);

                    thisEntity.transform.position = candidateNode.worldPosition;
                    TurnManager.Instance.SetGameState(GameState.Buying);
                    foreach (Tile _t in eligibleTiles)
                    {
                        _t.SetEligibleHighlight(false);
                    }
                    this.enabled = false;
                    return true;
                }

                if (!candidateNode.IsOccupied)
                {
                    //Let's move this unity to that node
                    if (thisEntity.CurrentNode != null)
                    {
                        thisEntity.CurrentNode.SetOccupied(false);

                    }
                    thisEntity.SetCurrentNode(candidateNode);
                    thisEntity.SetStartingNode(candidateNode);
                    candidateNode.SetOccupied(true);

                    thisEntity.transform.position = candidateNode.worldPosition;
                    TurnManager.Instance.SetGameState(GameState.Buying);
                    foreach (Tile _t in eligibleTiles)
                    {
                        _t.SetEligibleHighlight(false);
                    }
                    this.enabled = false;
                    return true;
                }
            }
        }


        return false;
    }

    public Tile GetTileUnder(Vector3 pos)
    {
        RaycastHit2D hit =
            Physics2D.Raycast(pos, Vector2.zero, 100, releaseMask);

        if (hit.collider != null)
        {
            //Released over something!
            //Debug.Log("Found Tile");
            Tile t = hit.collider.GetComponent<Tile>();
            return t;
        }

        return null;
    }

}