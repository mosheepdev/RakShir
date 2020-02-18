﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai_Spell_Huntress_Evade : AbilityInstance
{
    public float duration = 1.5f;
    public float speedAmount = 50f;
    public float dodgeAmount = 100f;
    public float healMultiplier = .1f;

    protected override void OnCreate(CastInfo castInfo, object[] data)
    {
        StartCoroutine(CoroutineEvade());
        StartCoroutine(CoroutineFollowAndDestroy());
    }

    IEnumerator CoroutineFollowAndDestroy()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 5f)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            transform.position = info.owner.transform.position;
        }
        if (photonView.IsMine)
        {
            DetachChildParticleSystemsAndAutoDelete();
            DestroySelf();
        }
    }

    IEnumerator CoroutineEvade()
    {
        info.owner.stat.bonusDodgeChance += dodgeAmount;
        if (photonView.IsMine)
        {
            info.owner.statusEffect.ApplyStatusEffect(StatusEffect.Speed(source, duration, speedAmount));
            info.owner.statusEffect.ApplyStatusEffect(StatusEffect.HealOverTime(source, duration, info.owner.maximumHealth * healMultiplier, true));
        }
        yield return new WaitForSeconds(duration);
        info.owner.stat.bonusDodgeChance -= dodgeAmount;

    }
}
