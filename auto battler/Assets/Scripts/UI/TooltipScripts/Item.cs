using UnityEngine;

namespace DapperDino.TooltipUI
{
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private int sellPrice;
        [SerializeField] private int energyCost = 999;
        [SerializeField] private Type type;

        public string Name { get { return name; } }
        public abstract string ColouredName { get; }
        public int SellPrice { get { return sellPrice; } }
        public Type Type { get { return type; } }
        public int EnergyCost { get { return energyCost; } }

        public abstract string GetTooltipInfoText();
    }
}