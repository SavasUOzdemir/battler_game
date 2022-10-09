using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace temp
{
    [CreateAssetMenu(fileName = "Formation", menuName = "ScriptableObjects/Formation", order = 1)]
    public class Formation : ScriptableObject
    {
        public string formationName;
        void OnEnable()
        {
            CompanyFormations.tempList.Add(this);
        }
    }
}