﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equip_Weapon_Rare_PoisonDagger : Equipment
{
    public float bonusDamageToPoisoned = 45f;
    
    public override void OnEquip(LivingThing owner)
    {
        owner.stat.baseAttackDamage = 40f;
        owner.stat.baseAttacksPerSecond = 1.4f;
        owner.stat.bonusMaximumHealth += 100f;
        if (photonView.IsMine)
        {
            owner.ChangeStandAnimation("Rare - PoisonDagger Stand");
            owner.ChangeWalkAnimation("Walk");
            owner.OnDoBasicAttackHit += BasicAttackHit;
        }
    }

    public override void OnUnequip(LivingThing owner)
    {
        owner.stat.baseAttackDamage = 1f;
        owner.stat.baseAttacksPerSecond = 1f;
        owner.stat.bonusMaximumHealth -= 100f;
        if (photonView.IsMine)
        {
            owner.ChangeStandAnimation("Stand");
            owner.ChangeWalkAnimation("Walk");
            owner.OnDoBasicAttackHit -= BasicAttackHit;
        }
    }

    private void BasicAttackHit(InfoBasicAttackHit info)
    {
        if (info.to.IsAffectedBy(StatusEffectType.DamageOverTime)) info.source.thing.DoMagicDamage(45f, info.to, false, SourceInfo.CasterOnly(info.source.thing));
    }
}
