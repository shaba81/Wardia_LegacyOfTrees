using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeSpawn : MonoBehaviour
{
    public GameObject terrainGrid;
    public TreeEntity treePrefab;
    private List<Tile> tiles = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        tiles = terrainGrid.GetComponentsInChildren<Tile>().ToList();

        foreach (Tile t in tiles)
        {
            if (t.hasTree)
            {
                TreeEntity newEntity = Instantiate(treePrefab, t.transform);
                newEntity.parent = t;
                GameManager.Instance.trees.Add(newEntity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
