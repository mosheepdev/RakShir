﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equip_Armor_Rare_LightArmorOfSwiftness : Equipment
{
    public override void OnEquip(LivingThing owner)
    {
        owner.stat.bonusMaximumHealth += 200f;
        owner.stat.bonusMovementSpeed += 20f;
        owner.stat.bonusHealthRegenerationPerSecond += 5f;
    }

    public override void OnUnequip(LivingThing owner)
    {
        owner.stat.bonusMaximumHealth -= 200f;
        owner.stat.bonusMovementSpeed -= 20f;
        owner.stat.bonusHealthRegenerationPerSecond -= 5f;
    }
}
