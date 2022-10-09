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
    public static Object[] Formations = Resources.LoadAll("Formations");
    public static Object[] Arrangements = Resources.LoadAll("Arrangements");
    public static Object[] TargetingModes = Resources.LoadAll("TargetingModes");
    static Dictionary<string, string> FormationToArrangement = new();
    static Dictionary<string, List<string>> FormationToTargetingMode = new();

    public enum Formation
    {
        Square,
        Wedge,
        Saw,
        RangedSquare,
        Dispersed
    }

    //ARRANGEMENT VARIABLES
    //WARNING:: Bad variables might break shit
    //Line and skirmish arrangement
    public static readonly int Columns = 5;
    //Wedge arrangement
    private static readonly int InnerWedge = 4;
    private static readonly int WedgeArmWidth = 2;
    //Skirmish arrangement
    private static readonly float ColumnSkirmishDispersion = 2f;
    private static readonly float RowSkirmishDispersion = 1.5f;

    public static void AddArrangementToFormation(string formation, string arrangement)
    {
        string value;
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

    public static System.Type GetCompanyPathfinder(Formation formation)
    {
        switch (formation)
        {
            case Formation.Square:
                return typeof(CompanyPathfinding_ShortestPath);
                //TODO
        }
        return typeof(CompanyPathfinding_ShortestPath);
    }
}
