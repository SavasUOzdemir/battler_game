using UnityEngine;

namespace DapperDino.TooltipUI
{
    [CreateAssetMenu]
    public class Type : ScriptableObject
    {
        [SerializeField] private new string name;
        public string Name { get { return name; } }
    }
}