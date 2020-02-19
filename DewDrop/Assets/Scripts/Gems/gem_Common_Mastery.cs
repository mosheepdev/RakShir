﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gem_Common_Mastery : Gem
{
    public float[] manaCostReduced = { 10, 15, 20, 25, 30 };

    private float appliedReductionAmount;
    public override void OnEquip(LivingThing owner, AbilityTrigger trigger)
    {
        appliedReductionAmount = Mathf.Min(manaCostReduced[level], trigger.manaCost);
        trigger.manaCost -= appliedReductionAmount;
    }

    public override void OnUnequip(LivingThing owner, AbilityTrigger trigger)
    {
        trigger.manaCost += appliedReductionAmount;
    }
}
