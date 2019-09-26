﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public abstract class AbilityTrigger : MonoBehaviour
{
    public enum TargetingType { None, PointStrict, PointNonStrict, Direction, Target }



    [Header("Trigger Settings")]
    public TargetingType targetingType;
    [ShowIf("ShouldRangeFieldShow")]
    public float range;
    [ShowIf("ShouldTargetValidatorFieldShow")]
    public TargetValidator targetValidator;
    public SelfValidator selfValidator;

    public float cooldownTime;

    



    private LivingThing livingThing
    {
        get
        {
            if(_livingThing == null)
            {
                _livingThing = transform.parent.GetComponent<LivingThing>();
            }
            return _livingThing;
        }
    }
    private LivingThing _livingThing;
    public bool isCooledDown
    {
        get
        {
            return remainingCooldownTime == 0;
        }
    }



    [ShowNativeProperty]
    public float remainingCooldownTime
    {
        get
        {
            return Mathf.Max(cooldownTime - (Time.time - cooldownStartTime), 0);
        }
    }


    private float cooldownStartTime = 0;

    public abstract void OnCast(AbilityInstanceManager.CastInfo info);


    protected bool ShouldTargetValidatorFieldShow()
    {
        return targetingType == TargetingType.Target;
    }

    protected bool ShouldRangeFieldShow()
    {
        return targetingType == TargetingType.Target || targetingType == TargetingType.PointStrict || targetingType == TargetingType.PointNonStrict;
    }

    public void StartCooldown()
    {
        cooldownStartTime = Time.time;
    }

    public void StartBasicAttackCooldown()
    {
        cooldownStartTime = Time.time - cooldownTime + 1 / livingThing.stat.finalAttacksPerSecond;
    }

    public void SetCooldown(float time)
    {
        cooldownStartTime = time + Time.time - cooldownTime;
    }

    public void ResetCooldown()
    {
        cooldownStartTime = Time.time - cooldownTime;
    }

    public void ApplyCooldownReduction(float time)
    {
        cooldownStartTime -= time;
    }

    public virtual bool CanActivate()
    {
        return selfValidator.Evaluate(livingThing);
    }
}
