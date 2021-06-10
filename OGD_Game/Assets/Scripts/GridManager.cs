using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Manager<GridManager>
{
    public GameObject terrainGrid;
    public Transform rotatedTransform;

    protected Graph graph;
    protected Dictionary<Team, int> startPositionPerTeam;

    List<Tile> allTiles = new List<Tile>();
    protected void Awake()
    {
        base.Awake();
        allTiles = terrainGrid.GetComponentsInChildren<Tile>().ToList();

        if (GameManager.Instance.myTeam == Team.Team2)
        {
            terrainGrid.transform.localRotation *= Quaternion.Euler(0, 0, 180);
            Vector3 rotated_pos = new Vector3(-1, 1, 0);
            terrainGrid.transform.position = rotated_pos;
        }

        InitializeGraph();
        startPositionPerTeam = new Dictionary<Team, int>();
        startPositionPerTeam.Add(Team.Team1, 0);
        startPositionPerTeam.Add(Team.Team2, graph.Nodes.Count - 1);

         
    }

    public Node GetFreeNode(Team forTeam)
    {
        int startIndex = startPositionPerTeam[forTeam];
        int currentIndex = startIndex;

        while (graph.Nodes[currentIndex].IsOccupied)
        {
            if (startIndex == 0)
            {
                currentIndex++;
                if (currentIndex == graph.Nodes.Count)
                    return null;
            }
            else
            {
                currentIndex--;
                if (currentIndex == -1)
                    return null;
            }

        }
        return graph.Nodes[currentIndex];
    }

    public Node GetNextNode(Node from, List<int> positions, bool isTwo)
    {
        int _index = from.index;
        int nodeIndex = 0;

        if (GameManager.Instance.myTeam == Team.Team1)
        {
            //_index += 1;
            nodeIndex = positions.SkipWhile(x => x != _index).Skip(1).DefaultIfEmpty(positions[0]).FirstOrDefault();
            if (isTwo)
            {
                _index = nodeIndex;
                _index = positions.SkipWhile(x => x != _index).Skip(1).DefaultIfEmpty(positions[0]).FirstOrDefault();
                nodeIndex = _index;
            }

        }
        else if (GameManager.Instance.myTeam == Team.Team2)
        {
            //_index -= 1;
            nodeIndex = positions.TakeWhile(x => x != _index).DefaultIfEmpty(positions[positions.Count - 1]).LastOrDefault();
            if (isTwo)
            {
                _index = nodeIndex;
                _index = positions.TakeWhile(x => x != _index).DefaultIfEmpty(positions[positions.Count - 1]).LastOrDefault();
                nodeIndex = _index;
            }

        }

        return GetNodeAtIndex(nodeIndex);
    }

    public Node GetNodeAtIndex(int index)
    {
        return graph.Nodes[index];
    }

    public List<Node> GetFirstRow()
    {
        List<Node> row = new List<Node>();
        int startIndex = startPositionPerTeam[GameManager.Instance.myTeam];
        row.Add(graph.Nodes[startIndex]);
        int[] indexesTeam1 = { 0, 5, 11, 16, 22 };
        int[] indexesTeam2 = { 4, 10, 15, 21, 26};



        if (startIndex == 0)
        {
            foreach (int index in indexesTeam1)
            {
                if (!graph.Nodes[index].IsOccupied)
                    row.Add(graph.Nodes[index]);
            }
        }
        else
        {
            foreach (int index in indexesTeam2)
            {
                if (!graph.Nodes[index].IsOccupied)
                    row.Add(graph.Nodes[index]);
            }
        }



        return row;

    }

    public List<Node> Neighbors(Node from)
    {
        return graph.Neighbors(from);
    }

    public List<Node> GetPath(Node from, Node to)
    {
        return graph.GetShortestPath(from, to);
    }

    public List<Node> GetNodesCloseTo(Node to)
    {
        return graph.Neighbors(to);
    }

    public Node GetNodeForTile(Tile t)
    {
        var allNodes = graph.Nodes;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (t.transform.GetSiblingIndex() == allNodes[i].index)
            {
                return allNodes[i];
            }
        }

        return null;
    }

    public Tile GetTileForNode(Node n)
    {
        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i].transform.GetSiblingIndex() == n.index)
            {
                return allTiles[i];
            }
        }

        return null;
    }

    private void InitializeGraph()
    {
        graph = new Graph();

        for (int i = 0; i < allTiles.Count; i++)
        {
            Vector3 place = allTiles[i].transform.position;
            graph.AddNode(place);
        }

        var allNodes = graph.Nodes;
        foreach (Node from in allNodes)
        {
            foreach (Node to in allNodes)
            {
                if (Vector3.Distance(from.worldPosition, to.worldPosition) < 1.5f && from != to)
                {
                    graph.AddEdge(from, to);
                }
            }
        }
    }

    public int fromIndex = 0;
    public int toIndex = 0;

    private void OnDrawGizmos()
    {
        if (graph == null)
            return;

        var allEdges = graph.Edges;
        if (allEdges == null)
            return;

        foreach (Edge e in allEdges)
        {
            Debug.DrawLine(e.from.worldPosition, e.to.worldPosition, Color.black, 100);
        }

        var allNodes = graph.Nodes;
        if (allNodes == null)
            return;

        foreach (Node n in allNodes)
        {
            Gizmos.color = n.IsOccupied ? Color.red : Color.green;
            Gizmos.DrawSphere(n.worldPosition, 0.1f);

        }

        if (fromIndex >= allNodes.Count || toIndex >= allNodes.Count)
            return;

        List<Node> path = graph.GetShortestPath(allNodes[fromIndex], allNodes[toIndex]);
        if (path.Count > 1)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(path[i - 1].worldPosition, path[i].worldPosition, Color.red, 10);
            }
        }
    }
}
