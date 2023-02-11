using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType
{
    Pistol,
    SMG,
    Rifle,
    Shotgun
}

public class AmmoManager : MonoBehaviour
{
    public Dictionary<AmmoType, int> ammoCounts = new Dictionary<AmmoType, int>();

    void Start()
    {
        ammoCounts[AmmoType.Pistol] = 20;
        ammoCounts[AmmoType.SMG] = 20;
        ammoCounts[AmmoType.Shotgun] = 6;
        ammoCounts[AmmoType.Rifle] = 30;
    }

    public int GetAmmoCount(AmmoType type)
    {
        return ammoCounts[type];
    }

    public void UseAmmo(AmmoType type, int amount)
    {
        ammoCounts[type] -= amount;
    }
}