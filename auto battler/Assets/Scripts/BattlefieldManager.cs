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
    static GameObject[] buffer = new GameObject[500];

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static void ModelsInRadius(Vector3 center, float radius)
    {

    }

    public static void CompaniesInRadius(Vector3 center, float radius)
    {

    }
    //TODO: Finish
    //static void UnitsInRadius(Vector3 center, float radius, FindOptions option, int team = -1)
    //{
    //    System.Array.Clear(buffer, 0, buffer.Length);
    //    int bufferIndex = 0;
    //    List<GameObject> searchList = null;
    //    switch (option)
    //    {
    //        case FindOptions.Model:
    //            searchList = modelsOnField;
    //            break;
    //        case FindOptions.Company:
    //            searchList = companiesOnField;
    //            break;
    //    }
    //    foreach(GameObject go in searchList)
    //    {
    //        if (!go)
    //        {
    //            continue;
    //        }
    //        else if((go.transform.position - center).sqrMagnitude <= radius * radius 
    //            && (team = -1 || team !=  go.GetComponent<Attributes>)
    //        {
    //            buffer[bufferIndex] = go;
    //            bufferIndex++;
    //        }
    //    }
    //}

}
