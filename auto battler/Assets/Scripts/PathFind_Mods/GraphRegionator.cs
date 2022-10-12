using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GraphRegionator : MonoBehaviour
{
    public class NavRegion
    {
        private static int idCounter = 0;
        public readonly int regionID;
        public Vector3 regionStart { get; private set; }
        public Vector3 regionEnd { get; private set; }
        public List<GridNode> nodes;
        

        public NavRegion(Vector3 regionStart, Vector3 regionEnd, List<GridNode> nodes)
        {
            this.regionID = idCounter;
            this.regionStart = regionStart;
            this.regionEnd = regionEnd;
            this.nodes = nodes;
            idCounter++;
            foreach (GridNode node in nodes)
            {
                GraphRegionator.nodeToRegion.Add(node,regionID);
            }
        }
    }

    public static GraphRegionator instance;
    public static Dictionary<GridNode, int> nodeToRegion = new(); 

    private GridGraph gridGraph;
    public List<NavRegion> navRegions = new();
    public GameObject toolStart;
    public GameObject toolEnd;
    public bool drawGizmos = false;

    void Awake()
    {
        gridGraph = AstarPath.active.data.gridGraph;

        //TODO: Make better singleton
        if (!instance)
            instance = this;
    }

    void Start()
    {
        Regionate(gridGraph);
        
    }

    void Update()
    {
        
    }


    public void Regionate(GridGraph graph)
    {
        Vector3 currentRegionStart = new();
        Vector3 currentRegionEnd = new();
        GridNode regionStartNode = null;
        List<GraphNode> unwalkables = GetUnwalkableNodes(gridGraph);
        int startIndex = 0;
        for (int i = 0; i < graph.nodes.Length; i++)
        {
            if (graph.nodes[i].Walkable)
            {
                currentRegionStart = (Vector3)graph.nodes[i].position;
                regionStartNode = graph.nodes[i];
                startIndex = i;
                break;
            }
        }

        if (regionStartNode == null || currentRegionStart == null)
            return;

        float maxX = graph.Width - 1;
        int currentIndex = startIndex;
        GridNode currentNode = graph.nodes[currentIndex];
        List<GridNode> currentNodes = new();
        List<GridNode> nodesInRow = new();
        bool firstRow = true;
        bool running = true;
        while (running && currentIndex < graph.nodes.Length )
        {
            currentNodes.AddRange(nodesInRow);
            nodesInRow.Clear();
            for(int i = currentIndex; true; i++)
            {
                currentNode = graph.nodes[i];
                if (currentNode.XCoordinateInGrid >= maxX)
                {
                    break;
                }
                if (IsNodeInARegion(currentNode) || !currentNode.Walkable)
                {
                    if (firstRow)
                    {
                        Debug.Log(IsNodeInARegion(currentNode) + " " + currentNode.Walkable);
                        maxX = currentNode.XCoordinateInGrid;
                        firstRow = false;
                        break;
                    }
                    toolStart.transform.position = (Vector3)currentNode.position;
                    currentRegionEnd = (Vector3)currentNodes.Last().position;
                    running = false;
                    break;
                }
                nodesInRow.Add(currentNode);
            }

            currentIndex += graph.width;
        }
        navRegions.Add(new NavRegion(currentRegionStart, currentRegionEnd, currentNodes));
        
        toolEnd.transform.position = currentRegionEnd;
        Debug.Log(navRegions[0].regionStart + " to " + navRegions[0].regionEnd);
    }

    private List<GraphNode> GetUnwalkableNodes(GridGraph graph)
    {
        List<GraphNode> unwalkables = new();
        for (int i = 0; i < graph.nodes.Length; i++)
        {
            if (!graph.nodes[i].Walkable)
            {
                unwalkables.Add(graph.nodes[i]);
            }
        }
        return unwalkables;
    }

    public bool IsNodeInARegion(GridNode node)
    {
        if (nodeToRegion.ContainsKey(node))
            return true;
        return false;
    }

    public bool IsNodeInARegion(GridNode node, out int regionid)
    {
        return nodeToRegion.TryGetValue(node, out regionid);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || !drawGizmos)
            return;
        Gizmos.color = Color.red;
        foreach (GraphNode node in GetUnwalkableNodes(gridGraph))
        {
            Gizmos.DrawCube((Vector3)node.position,Vector3.one * 0.2f);
        }
        Gizmos.color = Color.blue;
        foreach (NavRegion region in navRegions)
        {
            foreach (GridNode node in region.nodes)
            {
                Gizmos.DrawCube((Vector3)node.position,Vector3.one * 0.2f);
            }
        }
    }
}
