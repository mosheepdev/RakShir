﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatusEffectType : byte
{
    // Neutral
    Stasis, Dash,

    // Beneficial
    Invulnerable, Untargetable, Unstoppable, Protected, Speed, Haste, HealOverTime,

    // Harmful
    Stun, Airborne, Sleep,

    Polymorph,

    MindControl, Charm, Fear,

    Root, Slow,

    Silence, Blind,

    DamageOverTime,

    // Custom
    Custom,

    Shield,

    SpellPowerBoost, SpellPowerReduction,

    AttackDamageBoost, AttackDamageReduction
}

[System.Serializable]
public class StatusEffect
{
    #region Instance Members

    public long uid;
    public SourceInfo source;
    public LivingThing owner;
    public StatusEffectType type;
    public float duration;
    public float originalDuration;
    public object parameter;

    public System.Action OnExpire = () => { };

    public bool isAlive
    {
        get
        {
            return (duration > 0) && owner != null && owner.statusEffect.GetStatusEffectByUID(uid) != null;
        }
    }

    public StatusEffect(SourceInfo source, StatusEffectType type, float duration, object parameter = null)
    {
        this.source = source;
        this.type = type;
        this.duration = duration;
        this.parameter = parameter;
        this.originalDuration = duration;
    }

    public void Remove()
    {
        owner.statusEffect.RemoveStatusEffect(this);
    }

    public void AddDuration(float duration)
    {
        owner.statusEffect.AddDurationToStatusEffect(this, duration);
    }

    public void SetDuration(float duration)
    {
        owner.statusEffect.SetDurationOfStatusEffect(this, duration);
    }

    public void SetParameter(object parameter)
    {
        owner.statusEffect.SetParameterOfStatusEffect(this, parameter);
    }

    public void ResetDuration()
    {
        owner.statusEffect.SetDurationOfStatusEffect(this, originalDuration);
    }


    public bool IsHarmful()
    {
        return harmfulEffectTypes.Contains(type);
    }


    #endregion Instance Members

    #region Static Members

    private static List<StatusEffectType> harmfulEffectTypes = new List<StatusEffectType>
    {
        StatusEffectType.Stun,
        StatusEffectType.Airborne,
        StatusEffectType.Sleep,
        StatusEffectType.Polymorph,
        StatusEffectType.MindControl,
        StatusEffectType.Charm,
        StatusEffectType.Fear,
        StatusEffectType.Root,
        StatusEffectType.Slow,
        StatusEffectType.Silence,
        StatusEffectType.Blind,
        StatusEffectType.DamageOverTime
    };
    public static StatusEffect Stasis(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Stasis, duration);
    }
    public static StatusEffect Invulnerable(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Invulnerable, duration);
    }
    public static StatusEffect Untargetable(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Untargetable, duration);
    }
    public static StatusEffect Unstoppable(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Unstoppable, duration);
    }
    public static StatusEffect Protected(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Protected, duration);
    }
    public static StatusEffect Speed(SourceInfo source, float duration, float amount)
    {
        return new StatusEffect(source, StatusEffectType.Speed, duration, amount);
    }
    public static StatusEffect Haste(SourceInfo source, float duration, float amount)
    {
        return new StatusEffect(source, StatusEffectType.Haste, duration, amount);
    }
    public static StatusEffect HealOverTime(SourceInfo source, float duration, float amount, bool ignoreSpellPower = false)
    {
        return new StatusEffect(source, StatusEffectType.HealOverTime, duration, ignoreSpellPower ? amount : amount * source.thing.stat.finalSpellPower / 100f);
    }
    public static StatusEffect Stun(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Stun, duration);
    }
    public static StatusEffect Root(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Root, duration);
    }
    public static StatusEffect Slow(SourceInfo source, float duration, float amount)
    {
        return new StatusEffect(source, StatusEffectType.Slow, duration, amount);
    }
    public static StatusEffect Silence(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Silence, duration);
    }

    public static StatusEffect Blind(SourceInfo source, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Blind, duration);
    }


    public static StatusEffect DamageOverTime(SourceInfo source, float duration, float amount, bool ignoreSpellPower = false)
    {
        return new StatusEffect(source, StatusEffectType.DamageOverTime, duration, (ignoreSpellPower || source.thing == null) ? amount : amount * source.thing.stat.finalSpellPower / 100f);
    }
    public static StatusEffect Custom(SourceInfo source, string name, float duration)
    {
        return new StatusEffect(source, StatusEffectType.Custom, duration, name);
    }

    public static StatusEffect Shield(SourceInfo source, float duration, float amount, bool ignoreSpellPower = false)
    {
        return new StatusEffect(source, StatusEffectType.Shield, duration, (ignoreSpellPower || source.thing == null) ? amount : amount * source.thing.stat.finalSpellPower / 100f);
    }

    public static StatusEffect AttackDamageBoost(SourceInfo source, float duration, float amount)
    {
        return new StatusEffect(source, StatusEffectType.AttackDamageBoost, duration, amount);
    }

    public static StatusEffect AttackDamageReduction(SourceInfo source, float duration, float amount)
    {
        return new StatusEffect(source, StatusEffectType.AttackDamageReduction, duration, amount);
    }

    public static StatusEffect SpellPowerBoost(SourceInfo source, float duration, float amount)
    {
        return new StatusEffect(source, StatusEffectType.SpellPowerBoost, duration, amount);
    }

    public static StatusEffect SpellPowerReduction(SourceInfo source, float duration, float amount)
    {
        return new StatusEffect(source, StatusEffectType.SpellPowerReduction, duration, amount);
    }



    #endregion Static Members

}

