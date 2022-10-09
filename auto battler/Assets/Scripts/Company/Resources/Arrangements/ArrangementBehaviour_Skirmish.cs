using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Application;

public class ArrangementBehaviour_Skirmish : ArrangementBehaviour
{
    //Skirmish arrangement
    [SerializeField] protected int Columns = 5;
    [SerializeField] protected float ColumnSkirmishDispersion = 2f;
    [SerializeField] protected float RowSkirmishDispersion = 1.5f;

    public override void ArrangeModels(Vector3 companyPos, Vector3 direction)
    {
        Vector3 localLeft = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 localBack = -(direction.normalized);
        int currentRow = 0;
        int currentModel;
        int leftOvers = company.models.Count % Columns;
        var rows = company.models.Count / Columns;
        var firstPosition =
            ((Columns - 1) / 2f * companyMover.ModelColliderDia) * localLeft * ColumnSkirmishDispersion + companyPos;
        for (currentModel = 0; currentModel < rows * Columns; currentModel++)
        {
            companyMover.ModelPositions[currentModel] =
                firstPosition - (currentModel % Columns) * localLeft * companyMover.ModelColliderDia *
                              ColumnSkirmishDispersion
                              - localLeft * ColumnSkirmishDispersion * (currentRow % 2) / 2
                + currentRow * localBack * companyMover.ModelColliderDia * RowSkirmishDispersion;
            if (currentModel > 0 && (currentModel + 1) % Columns == 0)
                currentRow++;
        }

        firstPosition = ((leftOvers - 1) / 2 * companyMover.ModelColliderDia) * localLeft * ColumnSkirmishDispersion / 2
                        + rows * localBack * RowSkirmishDispersion
                        + companyPos;
        for (int i = 0; i < leftOvers; i++)
        {
            companyMover.ModelPositions[currentModel + i] = firstPosition -
                                                            i * localLeft * companyMover.ModelColliderDia *
                                                            ColumnSkirmishDispersion;
        }
    }

    protected override void UpdateBannerPosition()
    {
        Vector3 sum = Vector3.zero;
        int i;
        for (i = 0; i < company.models.Count && i < Columns; i++)
        {
            sum += company.models[i].transform.position;
        }
        transform.position = sum / i;

    }
}
