﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equip_Boots_Rare_SlimyOceanFinBoots : Equipment
{
    public override void OnEquip(LivingThing owner)
    {
        owner.stat.bonusMaximumHealth -= 150f;
        owner.stat.bonusHealthRegenerationPerSecond += 6.5f;
        owner.stat.bonusMovementSpeed += 50f;
    }

    public override void OnUnequip(LivingThing owner)
    {
        owner.stat.bonusMaximumHealth += 150f;
        owner.stat.bonusHealthRegenerationPerSecond -= 6.5f;
        owner.stat.bonusMovementSpeed -= 50f;
    }
}
