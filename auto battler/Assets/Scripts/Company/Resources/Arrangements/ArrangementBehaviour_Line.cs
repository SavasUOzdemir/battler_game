using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangementBehaviour_Line : ArrangementBehaviour
{
    //Line arrangement
    [SerializeField] protected int Columns = 5;

    public override void ArrangeModels(Vector3 companyPos, Vector3 direction)
    {
        Vector3 localLeft = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 localBack = -(direction.normalized);
        Vector3 firstPosition;
        int rows;
        int currentRow = 0;
        int currentModel;
        int leftOvers = company.models.Count % Columns;
        rows = company.models.Count / Columns;
        firstPosition = ((Columns - 1) / 2f * companyMover.ModelColliderDia) * localLeft + companyPos;
        for (currentModel = 0; currentModel < rows * Columns; currentModel++)
        {
            companyMover.ModelPositions[currentModel] = firstPosition - (currentModel % Columns) * localLeft * companyMover.ModelColliderDia + currentRow * localBack * companyMover.ModelColliderDia;
            if (currentModel > 0 && (currentModel + 1) % Columns == 0)
                currentRow++;
        }
        firstPosition = ((leftOvers - 1) / 2 * companyMover.ModelColliderDia) * localLeft + rows * localBack + companyPos;
        for (int i = 0; i < leftOvers; i++)
        {
            companyMover.ModelPositions[currentModel + i] = firstPosition - i * localLeft * companyMover.ModelColliderDia;
        }
    }

    protected override void UpdateBannerPosition()
    {
        Vector3 sum = Vector3.zero;
        int i;
        for (i = 0; i < company.models.Count && i < CompanyFormations.Columns; i++)
        {
            sum += company.models[i].transform.position;
        }
        transform.position = sum / i;

    }
}
