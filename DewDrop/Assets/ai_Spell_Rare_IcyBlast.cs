﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ai_Spell_Rare_IcyBlast : AbilityInstance
{
    public SelfValidator channelValidator;
    public TargetValidator targetValidator;
    public float radius = 4f;

    public float slowDuration = 3f;
    public float slowAmount = 40f;
    public float rootDuration = 1f;
    public float damage = 60f;

    private ParticleSystem range;
    private ParticleSystem blast;
    private GameObject hit;
    private GameObject root;


    protected override void OnCreate(CastInfo castInfo, object[] data)
    {
        range = transform.Find("Range").GetComponent<ParticleSystem>();
        blast = transform.Find("Blast").GetComponent<ParticleSystem>();
        hit = transform.Find("Hit").gameObject;
        root = transform.Find("Root").gameObject;

        if (photonView.IsMine)
        {
            range.Play();
            info.owner.control.StartChanneling(new Channel(channelValidator, 0.5f, false, false, false, false, ChannelFinished, ChannelCanceled));
        }
    }

    protected override void AliveUpdate()
    {
        transform.position = info.owner.transform.position;
    }

    private void ChannelFinished()
    {
        photonView.RPC("RpcBlast", RpcTarget.All);
        List<LivingThing> targets = info.owner.GetAllTargetsInRange(transform.position, radius, targetValidator);
        for(int i = 0; i < targets.Count; i++)
        {
            if (targets[i].statusEffect.IsAffectedBy(StatusEffectType.Slow))
            {
                targets[i].statusEffect.ApplyStatusEffect(StatusEffect.Root(info.owner, rootDuration));
                photonView.RPC("RpcRoot", RpcTarget.All, targets[i].photonView.ViewID);
            }
            photonView.RPC("RpcHit", RpcTarget.All, targets[i].photonView.ViewID);
            targets[i].statusEffect.ApplyStatusEffect(StatusEffect.Slow(info.owner, slowDuration, slowAmount));
            info.owner.DoMagicDamage(damage, targets[i]);
        }
        DetachChildParticleSystemsAndAutoDelete();
        DestroySelf();
    }


    [PunRPC]
    private void RpcBlast()
    {
        blast.Play();
    }

    [PunRPC]
    private void RpcRoot(int viewID)
    {
        Transform target = PhotonNetwork.GetPhotonView(viewID).transform;
        Instantiate(root, target.position, Quaternion.identity, transform).GetComponent<ParticleSystem>().Play();
    }

    [PunRPC]
    private void RpcHit(int viewID)
    {
        Transform target = PhotonNetwork.GetPhotonView(viewID).transform;
        Instantiate(hit, target.position, Quaternion.identity, transform).GetComponent<ParticleSystem>().Play();
    }


    private void ChannelCanceled()
    {
        DetachChildParticleSystemsAndAutoDelete(DetachBehaviour.StopEmittingAndClear);
        DestroySelf();
    }
}
