using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangementBehaviour_Wedge : ArrangementBehaviour
{
    //ARRANGEMENT VARIABLES
    //WARNING:: Bad variables might break shit
    //Wedge arrangement
    protected static readonly int InnerWedge = 4;
    protected static readonly int WedgeArmWidth = 2;

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

    protected override void UpdateBannerPosition()
    {
        transform.position = company.models[0].transform.position;

    }

    protected override void UpdateColliderSize()
    {
        int remainingModelCount = company.models.Count;
        int rows = 0;
        while (remainingModelCount > 0)
        {
            if (rows == InnerWedge) break;
            rows++;
            remainingModelCount -= rows;
        }

        while (remainingModelCount > 0)
        {
            rows++;
            remainingModelCount -= WedgeArmWidth * 2;
        }
        float colliderSizeXZ = rows * companyMover.ModelColliderDia;
        collider.size = new Vector3(colliderSizeXZ, 1f, colliderSizeXZ);
    }
}
