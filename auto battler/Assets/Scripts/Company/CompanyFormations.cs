using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using static CompanyFormations;
using Object = System.Object;
using Vector3 = UnityEngine.Vector3;

public static class CompanyFormations
{
    public static List<temp.Formation> tempList = new();
    static Dictionary<string, string> FormationToArrangement = new();
    static Dictionary<string, List<string>> FormationToTargetingMode = new();
    static Dictionary<string, string> FormationToPathfinder = new();
    public static Object[] Formations = Resources.LoadAll("Formations");
    public static Object[] Arrangements = Resources.LoadAll("Arrangements");
    public static Object[] TargetingModes = Resources.LoadAll("TargetingModes");
    public static Object[] CompanyPathfindings = Resources.LoadAll("CompanyPathfinders");
    

    public static void AddArrangementToFormation(string formation, string arrangement)
    {
        if (FormationToArrangement.ContainsKey(formation))
        {
            FormationToArrangement[formation] = arrangement;
        }
        else
        {
            FormationToArrangement.Add(formation,arrangement);
        }
    }

    public static void AddTargetingModeToFormation(string formation, string targetingMode)
    {
        List<string> values;
        if (FormationToTargetingMode.TryGetValue(formation, out values))
        {
            if (values.Contains(targetingMode))
            {
                Debug.Log("CompanyFormations: Arrangement of same name already exists!");
                return;
            }
            values.Add(targetingMode);
        }
        else
        {
            FormationToTargetingMode.Add(formation, new List<string>() { targetingMode });
        }
    }

    public static void AddPathfinderToFormation(string formation, string pathfinder)
    {
        if (FormationToPathfinder.ContainsKey(formation))
        {
            FormationToPathfinder[formation] = pathfinder;
        }
        else
        {
            FormationToPathfinder.Add(formation, pathfinder);
        }
    }

    public static List<string> GetTargetingOptions(string formation, int team)
    {
        List<string> values;
        List<string> options = new();
        if (FormationToTargetingMode.TryGetValue(formation, out values))
            for (int i = 0; i < values.Count; i++)
            {
                options.Add("TargetingModeBehaviour_" + values[i]);
            }
        return options;
    }

    public static string GetArrangement(string formation)
    {
        string value;
        if (FormationToArrangement.TryGetValue(formation, out value))
            return ("ArrangementBehaviour_" + value);
        return null;
    }

    public static string GetCompanyPathfinder(string formation)
    {
        string value;
        if (FormationToPathfinder.TryGetValue(formation, out value))
            return ("CompanyPathfinderBehaviour_" + value);
        return ("CompanyPathfinderBehaviour_ShortestPath");
    }
}
