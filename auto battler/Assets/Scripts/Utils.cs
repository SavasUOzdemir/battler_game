using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    static Collider[] findUnitsBuffer = new Collider[1000];
    public static void UnitsInRadius(Vector3 center, float radius, GameObject[] buffer)
    {
        System.Array.Clear(buffer, 0, buffer.Length);
        Physics.OverlapSphereNonAlloc(center, radius, findUnitsBuffer);
        int bufferIndex = 0;
        for(int i = 0; i < buffer.Length; i++)
        {
            if(!findUnitsBuffer[i] || !findUnitsBuffer[i].GetComponent<Attributes>())
                continue;
            buffer[bufferIndex] = findUnitsBuffer[i].gameObject;
            bufferIndex++;
        }
    }

    public static void CompaniesInRadius(Vector3 center, float radius, GameObject[] buffer)
    {
        System.Array.Clear(buffer, 0, buffer.Length);
        Physics.OverlapSphereNonAlloc(center, radius, findUnitsBuffer);
        int bufferIndex = 0;
        for (int i = 0; i < findUnitsBuffer.Length; i++)
        {
            if (!findUnitsBuffer[i] || !findUnitsBuffer[i].CompareTag("Company"))
                continue;
            buffer[bufferIndex] = findUnitsBuffer[i].gameObject;
            bufferIndex++;
        }
    }
}
