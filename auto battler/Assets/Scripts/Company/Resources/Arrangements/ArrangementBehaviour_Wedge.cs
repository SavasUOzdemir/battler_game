using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangementBehaviour_Wedge : ArrangementBehaviour
{
    public override void ArrangeModels(Vector3 companyPos, Vector3 direction)
    {
        Vector3 localLeft = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 localBack = -(direction.normalized);
        Vector3 firstPosition;
        int currentRow = 0;
        firstPosition = companyPos;
        var currentModel = 0;
        for (currentRow = 0; currentRow < company.models.Count; currentRow++)
        {
            for (int currentColumn = 0; currentColumn <= currentRow && currentModel < company.models.Count; currentColumn++)
            {
                if (currentRow >= InnerWedge && (currentColumn >= WedgeArmWidth && currentColumn - WedgeArmWidth < currentRow - InnerWedge + 1))
                    continue;
                companyMover.ModelPositions[currentModel] = firstPosition + localLeft * currentRow * companyMover.ModelColliderDia / 2 + currentRow * localBack - localLeft * companyMover.ModelColliderDia * currentColumn;
                currentModel++;
            }
        }
    }
}
