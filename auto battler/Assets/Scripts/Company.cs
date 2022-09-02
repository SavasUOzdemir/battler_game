using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Company : MonoBehaviour
{
    public enum Formation
    {
        Line,
        Skirmish
    }

    
    public GameObject prefab;
    public Vector3 firstPos;
    public int modelCount = 16;

    List<GameObject> model = new List<GameObject>();
    Formation formation = 0;
    Vector3[] modelPositions = new Vector3[16];
    Vector3 directionVector = Vector3.right;

    void Start()
    {
        Init();
        moveCompany(firstPos);
    }

    
    void Update()
    {
        
    }

    private void Init()
    {
        calcModelPositions(transform.position);
        for (int i = 0; i < modelCount; i++)
        {
            model.Add(Instantiate(prefab, modelPositions[i], Quaternion.identity));
        }
    }

    void calcModelPositions(Vector3 companyPos)
    {
        Vector3 directionVector = transform.position - companyPos;
        if (directionVector == Vector3.zero)
            directionVector = Vector3.right;
        Vector3 localLeft = Vector3.Cross(directionVector, Vector3.down).normalized;
        Vector3 localBack = -directionVector.normalized;
        Vector3 firstPosition = companyPos + 1.75f * localLeft;
        switch (formation)
        {
            case Formation.Line:
                for(int i = 0; i < modelCount; i++)
                {
                    if(i < 8)
                    {
                        modelPositions[i] = firstPosition - i * localLeft;
                    }else if (i >= 8)
                    {
                        modelPositions[i] = firstPosition + localBack - (i % 8) * localLeft;
                    }
                }
                break;
            case Formation.Skirmish:
                //TODO
                break;

        }
    }

    void moveCompany(Vector3 target)
    {
        calcModelPositions(target);
        for (int i = 0; i < model.Count; i++)
        {
            model[i].gameObject.SendMessage("Move", modelPositions[i]);
        }
    }

    public void RemoveModel(GameObject model)
    {
        //TODO
    }
}
