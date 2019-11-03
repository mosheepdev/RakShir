﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equip_Weapon_Rare_MagicBow : Equipment
{
    public override void OnEquip(LivingThing owner)
    {
        owner.stat.baseAttackDamage = 50f;
        owner.stat.baseAttacksPerSecond = 0.7f;
        if (photonView.IsMine)
        {
            owner.ChangeStandAnimation("Rare - MagicBow Stand");
            owner.ChangeWalkAnimation("Rare - MagicBow Walk");
        }
    }

    public override void OnUnequip(LivingThing owner)
    {
        owner.stat.baseAttackDamage = 1f;
        owner.stat.baseAttacksPerSecond = 1f;
        if (photonView.IsMine)
        {
            owner.ChangeStandAnimation("Stand");
            owner.ChangeWalkAnimation("Walk");
        }
    }
}
