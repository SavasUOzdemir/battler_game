using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all upgrades for the companies purchasable in the game.
//Do not inherit directly from this class for implementing ingame upgrades.
//Inherit from UnitAction for active abilites(e.g. an attack).
//Inherit from UnitPassive for passive upgrades (e.g. armor).
public abstract class UnitUpgrade : MonoBehaviour
{
    
}
