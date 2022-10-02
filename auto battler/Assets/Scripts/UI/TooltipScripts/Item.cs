using UnityEngine;
using UnityEngine.UI;

namespace DapperDino.TooltipUI
{
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private int sellPrice;
        [SerializeField] private int energyCost = 999;
        [SerializeField] private Type type;
        [SerializeField] private Sprite thumbnail;
        [SerializeField] private string upgrade;

        public string Name { get { return name; } }
        public abstract string ColouredName { get; }
        public int SellPrice { get { return sellPrice; } }
        public Type Type { get { return type; } }
        public int EnergyCost { get { return energyCost; } }
        public Sprite Thumbnail { get { return thumbnail; } }
        public System.Type Upgrade { get { return System.Type.GetType(upgrade); } }

        public abstract string GetTooltipInfoText();
    }
}