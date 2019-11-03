﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trg_BasicAttack_Universal : AbilityTrigger
{
    [Header("Universal Basicattack Settings")]
    public float channelDuration = 0.3f;
    public string abilityInstance = "ai_BasicAttack_Rare_";
    public override void OnCast(CastInfo info)
    {
        Channel channel = new Channel(selfValidator, channelDuration, false, true, false, true, ChannelSuccess, ResetCooldown);
        info.owner.control.StartChanneling(channel, true);
        StartCooldown(true);
    }


    private void ChannelSuccess()
    {
        AbilityInstanceManager.CreateAbilityInstance(abilityInstance, info.owner.transform.position + info.owner.GetCenterOffset(), Quaternion.identity, info);
    }
}
