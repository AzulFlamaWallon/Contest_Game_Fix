﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "GreyZoneTools/Create Energy Item")]
[Serializable]
public class EnergyItem : ItemInfo
{
    public ushort itemEnergy;

    public EnergyItem(ushort _Energy) : base()
    {
        itemEnergy = _Energy;
    }

}