using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class ModifiedAI : AIPath
{
    protected override void OnPathComplete(Path newPath)
    {
        var regionatedPath = RegionatePath(newPath);
        base.OnPathComplete(regionatedPath);
    }

    private Path RegionatePath(Path p)
    {
        int regionID;
        int currentRegionID =1;
        List<GraphNode> nodesToKeep = new();
        List<Vector3> vectorPathToKeep = new();
        Debug.Log("Regionating path, nodes: " + p.path.Count);
        for(int i = 0; i < p.path.Count - 1; i++)
        {
            if (GraphRegionator.instance.IsNodeInARegion(p.path[i] as GridNode, out regionID))
            {
                if (regionID != currentRegionID)
                {
                    nodesToKeep.Add(p.path[i]);
                    vectorPathToKeep.Add((Vector3)p.path[i].position);
                    currentRegionID = regionID;
                }
            }
        }
        nodesToKeep.Add(p.path.Last());
        vectorPathToKeep.Add((Vector3)p.path.Last().position);
        p.path = nodesToKeep;
        p.vectorPath = vectorPathToKeep;
        Debug.Log("Path regionated, nodes: " + p.path.Count);
        return p;
    }
}
