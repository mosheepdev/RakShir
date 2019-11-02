﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai_Spell_Rare_Stomp : AbilityInstance
{

    public float damage = 50f;
    public float stunDuration = 1.5f;
    public float radius = 2f;
    public TargetValidator targetValidator;

    protected override void OnCreate(CastInfo castInfo, object[] data)
    {
        if (photonView.IsMine)
        {
            List<LivingThing> targets = info.owner.GetAllTargetsInRange(transform.position, radius, targetValidator);
            for(int i = 0; i < targets.Count; i++)
            {
                info.owner.DoMagicDamage(damage, targets[i]);
                targets[i].ApplyStatusEffect(StatusEffect.Stun(info.owner, stunDuration));
            }
            DetachChildParticleSystemsAndAutoDelete();
            DestroySelf();
        }
    }
}
