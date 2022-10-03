using System.Collections.Generic;
using UnityEngine;

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

    public enum Targets
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

    public static readonly int Columns = 8;

    //Wedge variable
    //WARNING:: Bad variables might break shit
    private static readonly int InnerWedge = 4;
    private static readonly int WedgeArmWidth = 2;

    public static Targets[] GetTargetingOptions(Formation formation, int team)
    {
        switch (formation)
        {
            case Formation.Square:
                return new Targets[] { Targets.ClosestSquad };
            case Formation.Saw:
                return new Targets[] { Targets.Protective, Targets.HighestHealth };
            case Formation.Wedge:
                return new Targets[] { Targets.RangedSquad, Targets.LowestHealth, Targets.LowestEndurance };
            case Formation.RangedSquare:
                return new Targets[]
                    { Targets.FlyingSquad, Targets.LowestHealth, Targets.LowestMorale, Targets.LowestEndurance };
            case Formation.Dispersed:
                return new Targets[]
                    { Targets.FlyingSquad, Targets.LowestHealth, Targets.LowestMorale, Targets.LowestEndurance };
        }
        return null;
    }


    public static void CalcModelPositions(Vector3 companyPos, Vector3 direction, List<GameObject> models, Vector3[] modelPositions, Arrangement arrangement, float modelColliderDia)
    {
        Vector3 localLeft = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 localBack = -(direction.normalized);
        Vector3 firstPosition;
        int rows;
        int currentRow = 0;
        int currentModel;
        int leftOvers = models.Count % Columns;
        switch (arrangement)
        {
            case Arrangement.Line:
                rows = models.Count / Columns;
                firstPosition = ((Columns - 1) / 2f * modelColliderDia) * localLeft + companyPos;
                for (currentModel = 0; currentModel < rows * Columns; currentModel++)
                {
                    modelPositions[currentModel] = firstPosition - (currentModel % Columns) * localLeft + currentRow * localBack;
                    if (currentModel > 0 && (currentModel + 1) % Columns == 0)
                        currentRow++;
                }
                firstPosition = ((leftOvers - 1) / 2 * modelColliderDia) * localLeft + rows * localBack + companyPos;
                for (int i = 0; i < leftOvers; i++)
                {
                    modelPositions[currentModel + i] = firstPosition - i * localLeft;
                }
                break;
            case Arrangement.Wedge:
                firstPosition = companyPos;
                currentModel = 0;
                for (currentRow = 0; currentRow < models.Count; currentRow++)
                {
                    for (int currentColumn = 0; currentColumn <= currentRow && currentModel < models.Count; currentColumn++)
                    {
                        if(currentRow >= InnerWedge && (currentColumn >= WedgeArmWidth && currentColumn - WedgeArmWidth < currentRow - InnerWedge + 1))
                            continue;
                        modelPositions[currentModel] = firstPosition + localLeft * currentRow * modelColliderDia / 2 + currentRow * localBack - localLeft * modelColliderDia * currentColumn;
                        currentModel++;
                    }
                }
                break;
            case Arrangement.Skirmish:
                break;
        }
    }
}
