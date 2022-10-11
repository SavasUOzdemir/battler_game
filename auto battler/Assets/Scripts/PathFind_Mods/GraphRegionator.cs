using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GraphRegionator : MonoBehaviour
{
    public class NavRegion
    {
        private int regionID;
        public Vector3 regionStart { get; private set; }
        public Vector3 regionEnd { get; private set; }

        public NavRegion(int regionID, Vector3 regionStart, Vector3 regionEnd)
        {
            this.regionID = regionID;
            this.regionStart = regionStart;
            this.regionEnd = regionEnd;
        }
    }

    private GridGraph gridGraph = AstarPath.active.data.gridGraph;
    public List<NavRegion> navRegions = new();


}
