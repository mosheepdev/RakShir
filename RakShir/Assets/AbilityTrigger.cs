﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public abstract class AbilityTrigger : MonoBehaviour
{
    public enum TargetingType { None, PointStrict, PointNonStrict, Direction, Target }


    public GameObject indicator;
    [Header("Trigger Settings")]
    public TargetingType targetingType;
    [ShowIf("ShouldRangeFieldShow")]
    public float range;
    [ShowIf("ShouldTargetValidatorFieldShow")]
    public TargetValidator targetValidator;
    public SelfValidator selfValidator;

    public float cooldownTime;

    



    public LivingThing owner
    {
        get
        {
            if(_owner == null)
            {
                _owner = transform.parent.GetComponent<LivingThing>();
            }
            return _owner;
        }
    }
    private LivingThing _owner;
    public bool isCooledDown
    {
        get
        {
            return remainingCooldownTime == 0;
        }
    }


    public float remainingCooldownTime
    {
        get
        {
            return Mathf.Max(cooldownTime - (Time.time - cooldownStartTime), 0);
        }
    }


    private float cooldownStartTime = 0;

    public abstract void OnCast(CastInfo info);


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
        ApplyCooldownReduction(cooldownTime * owner.stat.finalCooldownReduction / 100f);
    }

    public void StartBasicAttackCooldown()
    {
        cooldownStartTime = Time.time - cooldownTime + (1 / owner.stat.finalAttacksPerSecond) / ((100 + owner.statusEffect.totalHasteAmount) / 100f);
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
        return selfValidator.Evaluate(owner);
    }

    public void ShowIndicator()
    {
        if (indicator == null) return;
        indicator.SetActive(true);
    }

    public void HideIndicator()
    {
        if (indicator == null) return;
        indicator.SetActive(false);
    }
}
