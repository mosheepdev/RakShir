﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cons_Sparkstone : Consumable
{
    public override bool OnUse(CastInfo info)
    {
        AbilityInstanceManager.CreateAbilityInstance("ai_cons_Sparkstone", transform.position, Quaternion.identity, info, new SourceInfo());
        SFXManager.CreateSFXInstance("si_cons_Sparkstone", info.owner.transform.position);
        DestroySelf();
        return true;
    }

}
