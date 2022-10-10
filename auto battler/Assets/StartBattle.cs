using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    [SerializeField] GameObject layoutObject;
    int waittime = 3;
    public void OnButtonDown()
    {
        if (DoneCheck())
        {
            MoveLayoutObject(layoutObject);
            StartCoroutine(WaitForSeconds(waittime));
            DisableLayoutObject(layoutObject);
            GoStartBattle();
        }
    }

    bool DoneCheck()
    {
        return true;
    }

    bool MoveLayoutObject(GameObject layoutObj)
    {
        //some logic to move obj

        if (true) //if movement is done
        {
            return true;
        }
    }

    void DisableLayoutObject(GameObject layoutObj)
    {
        layoutObj.SetActive(false);
    }

    void GoStartBattle()
    {

    }

    IEnumerator WaitForSeconds(int t)
    {
        yield return new WaitForSeconds(t);
    }
}
