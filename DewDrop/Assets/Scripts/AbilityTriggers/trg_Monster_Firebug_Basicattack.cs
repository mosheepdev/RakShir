﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trg_Monster_Firebug_Basicattack : AbilityTrigger
{
    public float channelDurationRatio = 0.5f;
    public float channelAfterDelayRatio = 0f;
    public override void OnCast(CastInfo info)
    {
        Channel channel = new Channel(selfValidator, channelDurationRatio, false, false, false, false, ChannelSuccess, null);
        info.owner.control.StartChanneling(channel, true);
        StartCooldown(true);
    }

    private void ChannelSuccess()
    {
        AbilityInstanceManager.CreateAbilityInstance("ai_Monster_Firebug_Basicattack", info.owner.transform.position, Quaternion.identity, info);
        Channel channel = new Channel(selfValidator, channelDurationRatio, false, false, false, false, null, null);
        info.owner.control.StartChanneling(channel, true);
    }
}
