﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equip_Armor_Rare_PlatinumArmor : Equipment
{
    public override void OnEquip(LivingThing owner)
    {
        owner.stat.bonusMaximumHealth += 300f;
        owner.stat.bonusHealthRegenerationPerSecond += 2f;

    }

    public override void OnUnequip(LivingThing owner)
    {
        owner.stat.bonusMaximumHealth -= 300f;
        owner.stat.bonusHealthRegenerationPerSecond -= 2f;

    }
}
