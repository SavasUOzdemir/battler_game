using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using static CompanyFormations;
using Vector3 = UnityEngine.Vector3;

public static class CompanyFormations
{
    public enum Formation
    {
        Square,
        Wedge,
        Saw,
        RangedSquare,
        Dispersed
    }

    public enum Arrangement
    {
        Line,
        Wedge,
        Skirmish,
    }

    public enum TargetingMode
    {
        ClosestSquad,
        LowestHealth,
        LowestMorale,
        LowestEndurance,
        RangedSquad,
        FlyingSquad,
        HighestHealth,
        Protective
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

    public static List<Formation> GetFormationOptions(bool melee, bool ranged)
    {
        List<Formation> formations = new();
        if (melee)
        {
            formations.Add(Formation.Square);
            formations.Add(Formation.Wedge);
            formations.Add(Formation.Saw);
        }
        if (ranged)
        {
            formations.Add(Formation.RangedSquare);
            formations.Add(Formation.Dispersed);
        }
        return formations;
    }

    public static TargetingMode[] GetTargetingOptions(Formation formation, int team)
    {
        switch (formation)
        {
            case Formation.Square:
                return new TargetingMode[] { TargetingMode.ClosestSquad };
            case Formation.Saw:
                return new TargetingMode[] { TargetingMode.Protective, TargetingMode.HighestHealth };
            case Formation.Wedge:
                return new TargetingMode[] { TargetingMode.RangedSquad, TargetingMode.LowestHealth, TargetingMode.LowestEndurance };
            case Formation.RangedSquare:
                return new TargetingMode[]
                    { TargetingMode.FlyingSquad, TargetingMode.LowestHealth, TargetingMode.LowestMorale, TargetingMode.LowestEndurance };
            case Formation.Dispersed:
                return new TargetingMode[]
                    { TargetingMode.FlyingSquad, TargetingMode.LowestHealth, TargetingMode.LowestMorale, TargetingMode.LowestEndurance };
        }
        return null;
    }

    public static Arrangement GetArrangement(Formation formation)
    {
        switch (formation)
        {
            case CompanyFormations.Formation.Square:
                return Arrangement.Line;
            case CompanyFormations.Formation.Saw:
                return Arrangement.Skirmish;
            case CompanyFormations.Formation.Wedge:
                return Arrangement.Wedge;
            case CompanyFormations.Formation.RangedSquare:
                return Arrangement.Line;
            case CompanyFormations.Formation.Dispersed:
                return Arrangement.Skirmish;
        }
        return Arrangement.Line;
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


    public static void CalcModelPositions(Vector3 companyPos, Vector3 direction, int modelCount, Vector3[] modelPositions, Arrangement arrangement, float modelColliderDia)
    {
        Vector3 localLeft = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 localBack = -(direction.normalized);
        Vector3 firstPosition;
        int rows;
        int currentRow = 0;
        int currentModel;
        int leftOvers = modelCount % Columns;
        switch (arrangement)
        {
            case Arrangement.Line:
                rows = modelCount / Columns;
                firstPosition = ((Columns - 1) / 2f * modelColliderDia) * localLeft + companyPos;
                for (currentModel = 0; currentModel < rows * Columns; currentModel++)
                {
                    modelPositions[currentModel] = firstPosition - (currentModel % Columns) * localLeft * modelColliderDia + currentRow * localBack * modelColliderDia;
                    if (currentModel > 0 && (currentModel + 1) % Columns == 0)
                        currentRow++;
                }
                firstPosition = ((leftOvers - 1) / 2 * modelColliderDia) * localLeft + rows * localBack + companyPos;
                for (int i = 0; i < leftOvers; i++)
                {
                    modelPositions[currentModel + i] = firstPosition - i * localLeft * modelColliderDia;
                }
                break;
            case Arrangement.Wedge:
                firstPosition = companyPos;
                currentModel = 0;
                for (currentRow = 0; currentRow < modelCount; currentRow++)
                {
                    for (int currentColumn = 0; currentColumn <= currentRow && currentModel < modelCount; currentColumn++)
                    {
                        if(currentRow >= InnerWedge && (currentColumn >= WedgeArmWidth && currentColumn - WedgeArmWidth < currentRow - InnerWedge + 1))
                            continue;
                        modelPositions[currentModel] = firstPosition + localLeft * currentRow * modelColliderDia / 2 + currentRow * localBack - localLeft * modelColliderDia * currentColumn;
                        currentModel++;
                    }
                }
                break;
            case Arrangement.Skirmish:
                rows = modelCount / Columns;
                firstPosition = ((Columns - 1) / 2f * modelColliderDia) * localLeft * ColumnSkirmishDispersion + companyPos;
                for (currentModel = 0; currentModel < rows * Columns; currentModel++)
                {
                    modelPositions[currentModel] =
                        firstPosition - (currentModel % Columns) * localLeft * modelColliderDia *
                                      ColumnSkirmishDispersion
                                      - localLeft * ColumnSkirmishDispersion * (currentRow % 2) / 2 
                        + currentRow * localBack * modelColliderDia * RowSkirmishDispersion;
                    if (currentModel > 0 && (currentModel + 1) % Columns == 0)
                        currentRow++;
                }
                firstPosition = ((leftOvers - 1) / 2 * modelColliderDia) * localLeft * ColumnSkirmishDispersion / 2
                                + rows * localBack * RowSkirmishDispersion
                                + companyPos;
                for (int i = 0; i < leftOvers; i++)
                {
                    modelPositions[currentModel + i] = firstPosition - i * localLeft * modelColliderDia * ColumnSkirmishDispersion;
                }
                break;
        }
    }

    public static bool DetermineTarget(Vector3 companyPosition, List<GameObject> enemiesList, TargetingMode targetingMode, ref GameObject target)
    {
        GameObject currentTarget = null;
        float distSqr;
        float shortestDistSqr = Mathf.Infinity;
        switch (targetingMode)
        {
            case TargetingMode.ClosestSquad:
                
                foreach (GameObject enemyCompany in enemiesList)
                {
                    if(!enemyCompany)
                        continue;
                    distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                    if (distSqr < shortestDistSqr)
                    {
                        shortestDistSqr = distSqr;
                        currentTarget = enemyCompany;
                    }
                }

                break;
            case TargetingMode.LowestHealth:
                float companyAverageHP;
                float currentTargetAverageHP = Mathf.Infinity;
                foreach (GameObject enemyCompany in enemiesList)
                {
                    if(!enemyCompany)
                        continue;
                    companyAverageHP = enemyCompany.GetComponent<Company>().AverageHealth();
                    if (currentTargetAverageHP >= companyAverageHP)
                    {
                        distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                        if (distSqr < shortestDistSqr)
                        {
                            shortestDistSqr = distSqr;
                            currentTargetAverageHP = companyAverageHP;
                            currentTarget = enemyCompany;
                        }
                    }
                }

                break;
            case TargetingMode.LowestMorale:
                float currentTargetMorale =Mathf.Infinity;
                foreach (GameObject enemyCompany in enemiesList)
                {
                    if (!enemyCompany)
                        continue;
                    float currentMorale = enemyCompany.GetComponent<Company>().GetMorale();
                    if (currentTargetMorale > currentMorale)
                    {
                        distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                        if (distSqr < shortestDistSqr)
                        {
                            shortestDistSqr = distSqr;
                            currentTargetMorale = currentMorale;
                            currentTarget = enemyCompany;
                        }
                    }
                }

                break;
            case TargetingMode.LowestEndurance:
                float currentTargetEndurance = Mathf.Infinity;
                foreach (GameObject enemyCompany in enemiesList)
                {
                    if (!enemyCompany)
                        continue;
                    float currentEndurance = enemyCompany.GetComponent<Company>().AverageEndurance();
                    if (currentTargetEndurance > currentEndurance)
                    {
                        distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                        if (distSqr < shortestDistSqr)
                        {
                            shortestDistSqr = distSqr;
                            currentTargetEndurance = currentEndurance;
                            currentTarget = enemyCompany;
                        }
                    }
                }

                break;
            case TargetingMode.RangedSquad:
                foreach (GameObject enemyCompany in enemiesList)
                {
                    if (!enemyCompany)
                        continue;
                    if (enemyCompany.GetComponent<Company>().RangedCompany)
                    {
                        distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                        if (distSqr < shortestDistSqr)
                        {
                            shortestDistSqr = distSqr;
                            currentTarget = enemyCompany;
                        }
                    }
                }

                break;
            case TargetingMode.FlyingSquad:
                foreach (GameObject enemyCompany in enemiesList)
                {
                    if (!enemyCompany)
                        continue;
                    if (enemyCompany.GetComponent<Company>().FlyingCompany)
                    {
                        distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                        if (distSqr < shortestDistSqr)
                        {
                            shortestDistSqr = distSqr;
                            currentTarget = enemyCompany;
                        }
                    }
                }

                break;
            case TargetingMode.HighestHealth:
                float AverageHP;
                float TargetAverageHP = 0f;
                foreach (GameObject enemyCompany in enemiesList)
                {
                    if (!enemyCompany)
                        continue;
                    AverageHP = enemyCompany.GetComponent<Company>().AverageHealth();
                    if (TargetAverageHP <= AverageHP)
                    {
                        distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                        if (distSqr < shortestDistSqr)
                        {
                            shortestDistSqr = distSqr;
                            TargetAverageHP = AverageHP;
                            currentTarget = enemyCompany;
                        }
                    }
                }

                break;
            case TargetingMode.Protective:
                //TODO
                break;
        }

        if (!currentTarget) return false;
        target = currentTarget;
        return true;
    }
}
