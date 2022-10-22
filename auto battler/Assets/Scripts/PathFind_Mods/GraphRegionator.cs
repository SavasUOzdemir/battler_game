using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.PlasticSCM.Editor.WebApi;
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
        public Color regionColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1), Random.Range(0f, 1));


        public NavRegion(Vector3 regionStart, Vector3 regionEnd, List<GridNode> nodes)
        {
            regionID = idCounter;
            this.regionStart = regionStart;
            this.regionEnd = regionEnd;
            this.nodes = new(nodes);
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
        foreach (NavRegion region in navRegions)
        {
            Debug.Log(region.regionID + "||" + region.nodes.Count);
        }

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
        List<GridNode> remainingNodes = new(graph.nodes);
        foreach (GridNode node in unwalkables)
        {
            remainingNodes.Remove(node);
        }

        List<GridNode> currentNodes = new();
        List<GridNode> nodesInRow = new();
        GridNode currentNode;

        int iteration = 0;

        while (remainingNodes.Count > 0)
        {


            int startIndex = 0;
            for (int i = 0; i < remainingNodes.Count; i++)
            {
                if (remainingNodes[i].Walkable)
                {
                    currentRegionStart = (Vector3)remainingNodes[i].position;
                    regionStartNode = remainingNodes[i];
                    startIndex = remainingNodes[i].NodeInGridIndex;
                    break;
                }
            }

            if (regionStartNode == null || currentRegionStart == null)
                return;

            float maxX = graph.Width - 1;
            int currentIndex = startIndex;
            currentNodes.Clear();
            nodesInRow.Clear();
            bool firstRow = true;
            bool running = true;
            toolStart.transform.position = (Vector3)graph.nodes[startIndex].position;
            while (running && currentIndex < graph.nodes.Length)
            {
                currentNodes.AddRange(nodesInRow);
                nodesInRow.Clear();
                for (int i = currentIndex; true; i++)
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
                            maxX = currentNode.XCoordinateInGrid;
                            firstRow = false;
                            toolEnd.transform.position = (Vector3)graph.nodes[i].position;
                            break;
                        }

                        currentRegionEnd = (Vector3)currentNodes.Last().position;
                        running = false;
                        break;
                    }

                    nodesInRow.Add(currentNode);
                }

                currentIndex += graph.width;
            }

            navRegions.Add(new NavRegion(currentRegionStart, currentRegionEnd, currentNodes));
            Debug.Log("New Region: " + currentRegionStart + " to " + currentRegionEnd + " node count: " + navRegions.Last().nodes.Count);
            foreach (GridNode node in currentNodes)
            {
                remainingNodes.Remove(node);
            }

            iteration++;
            if (iteration > 50000)
            {
                Debug.Log("Potential infinite loop! Regionator abondoned!");
                break;
            }

            if (remainingNodes.Count == 0)
            {
                Debug.Log("Regionator successfully exiting, number of regions created: " + navRegions.Count);
            }
        }
        
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

        foreach (NavRegion region in navRegions)
        {
            Gizmos.color = region.regionColor;
            foreach (GridNode node in region.nodes)
            {
                Gizmos.DrawCube((Vector3)node.position,Vector3.one * gridGraph.nodeSize);
            }
        }
    }
}
