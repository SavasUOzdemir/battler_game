using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.TooltipUI;

[CreateAssetMenu]
public class Squad : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private int maxEnergy = 6;
    [SerializeField] private int currentEnergy = 6;
    [SerializeField] private Sprite sprite;

    [SerializeField] private int availableSlots = 4;
    [SerializeField] private string targetPreferencePrimary;
    [SerializeField] private string targetPreferenceSecondary;
    [SerializeField] internal Item[] heldItems = new Item[4];

    public Sprite Sprite { get { return sprite; } set { sprite = value; } }
    public string Name { get { return name; } }
    public int AvailableSlots { get { return availableSlots; } set { availableSlots = value; } }
    public int CurrentEnergy { get { return currentEnergy; } set { currentEnergy = value; } }
    public string TargetPreferencePrimary { get { return targetPreferencePrimary; } set { targetPreferencePrimary = value; } }
    public string TargetPreferenceSecondary { get { return targetPreferenceSecondary; } set { targetPreferenceSecondary = value; } }

    public class ItemsList : Squad
    {
        public Item this[int i]
        {
            get => heldItems[i];
            set => heldItems[i] = value;
        }

        //void sortHeldItems()
        //{
        //    for (int i = 0; i + 1 < heldItems.Length; i++)
        //        if (heldItems[i] == null)
        //        {
        //            heldItems[i] = heldItems[i + 1];
        //            heldItems[i + 1] = null;                
        //        }
        //}
    }
}
