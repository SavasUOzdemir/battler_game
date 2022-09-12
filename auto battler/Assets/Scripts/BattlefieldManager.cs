using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public enum FindOptions
    {
        Model,
        Company
    }

    public static BattlefieldManager instance;

    static List<GameObject> companiesOnField = new List<GameObject>();
    static List<GameObject> modelsOnField = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    static void UnitsInRadius(Vector3 center, float radius, FindOptions option, GameObject[] buffer)
    {
        System.Array.Clear(buffer, 0, buffer.Length);
        int bufferIndex = 0;
        List<GameObject> searchList = null;
        Vector3 unitPos;
        switch (option)
        {
            case FindOptions.Model:
                searchList = modelsOnField;
                break;
            case FindOptions.Company:
                searchList = companiesOnField;
                break;
        }
        foreach (GameObject go in searchList)
        {
            if (!go)
            {
                continue;
            }
            else if ((go.transform.position - center).sqrMagnitude <= radius * radius)
            {
                buffer[bufferIndex] = go;
                if (bufferIndex == buffer.Length - 1)
                    return;
                bufferIndex++;
            }
        }
    }

    public static void ModelsInRadius(Vector3 center, float radius, GameObject[] buffer)
    {
        UnitsInRadius(center, radius, FindOptions.Model, buffer);
    }

    public static void CompaniesInRadius(Vector3 center, float radius, GameObject[] buffer)
    {
        UnitsInRadius(center, radius, FindOptions.Company, buffer);
    }

    public static void AddCompany(GameObject company)
    {
        if(!companiesOnField.Contains(company))
            companiesOnField.Add(company);
    }

    public static void RemoveCompany(GameObject company)
    {
        if(companiesOnField.Contains(company))
            companiesOnField.Remove(company);
    }

    public static void AddModel(GameObject model)
    {
        if(!modelsOnField.Contains(model))
            modelsOnField.Add(model);
    }

    public static void RemoveModel(GameObject model)
    {
        if (modelsOnField.Contains(model))
            modelsOnField.Remove(model);
    }

}
