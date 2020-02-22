﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Gem : Item
{
    [Header("Gem Settings")]
    public AbilityTrigger trigger;
    public int level = 0;
    public int maxLevel = 4;

    private List<AbilityInstance> instances = new List<AbilityInstance>();

    private GameObject deactivatedModel;
    private List<Collider> deactivatedColliders = new List<Collider>();
    private List<Rigidbody> deactivatedRigidbodies = new List<Rigidbody>();


    protected SourceInfo source
    {
        get
        {
            return new SourceInfo { gem = this, thing = owner };
        }
    }

    public void CreateAbilityInstance(string prefabName, Vector3 position, Quaternion rotation, object[] data = null)
    {
        CastInfo info = new CastInfo { owner = owner };
        PurgeInstancesList();
        AbilityInstance instance = AbilityInstanceManager.CreateAbilityInstance(prefabName, position, rotation, info, source, data);
        instances.Add(instance);
    }

    public void CreateAbilityInstance(string prefabName, Vector3 position, Quaternion rotation, CastInfo info, object[] data = null)
    {
        PurgeInstancesList();
        AbilityInstance instance = AbilityInstanceManager.CreateAbilityInstance(prefabName, position, rotation, info, source, data);
        instances.Add(instance);
    }

    private void PurgeInstancesList()
    {
        for (int i = instances.Count - 1; i >= 0; i--)
        {
            if (instances[i] == null || !instances[i].isAlive)
            {
                instances.RemoveAt(i);
            }
        }
    }

    public bool IsAnyInstanceActive()
    {
        PurgeInstancesList();
        return instances.Count != 0;
    }

    public AbilityInstance GetLastInstance()
    {
        PurgeInstancesList();
        if (instances.Count == 0) return null;
        return instances[instances.Count - 1];
    }

    public AbilityInstance GetFirstInstsance()
    {
        PurgeInstancesList();
        if (instances.Count == 0) return null;
        return instances[0];
    }



    public void SendEventToAbilityInstance(string eventString, AbilityInstanceEventTargetType target)
    {
        if (!IsAnyInstanceActive())
        {
            Debug.LogWarning("There is no active AbilityInstance to send event to!\n" + name);
            return;
        }
        switch (target)
        {
            case AbilityInstanceEventTargetType.EveryInstance:
                for (int i = 0; i < instances.Count; i++)
                {
                    instances[i].photonView.RPC("RpcDoEvent", RpcTarget.All, eventString);
                }
                break;
            case AbilityInstanceEventTargetType.FirstInstance:
                instances[0].photonView.RPC("RpcDoEvent", RpcTarget.All, eventString);
                break;
            case AbilityInstanceEventTargetType.LastInstance:
                instances[instances.Count - 1].photonView.RPC("RpcDoEvent", RpcTarget.All, eventString);
                break;
        }
    }

    public abstract void OnEquip(LivingThing owner, AbilityTrigger trigger);

    public abstract void OnUnequip(LivingThing owner, AbilityTrigger trigger);

    public virtual void OnTriggerCast(bool isMine) { }

    public virtual void AliveUpdate(bool isMine) { }

    private void Update()
    {
        if (owner != null && trigger != null) AliveUpdate(owner.photonView.IsMine);
    }

    public void SetLevel(int level)
    {
        photonView.RPC("RpcSetLevel", RpcTarget.All, level);
    }

    public void Equip(string triggerName)
    {
        photonView.RPC("RpcEquip", RpcTarget.All, triggerName);
    }

    public void Unequip()
    {
        photonView.RPC("RpcUnequip", RpcTarget.All);
    }

    [PunRPC]
    protected void RpcEquip(string triggerName)
    {
        trigger = owner.transform.Find(triggerName).GetComponent<AbilityTrigger>();
        trigger.connectedGems.Add(this);

        Transform t = transform.Find("Model");
        if (t != null)
        {
            deactivatedModel = t.gameObject;
            deactivatedModel.SetActive(false);
        }

        deactivatedColliders.AddRange(GetComponents<Collider>());
        deactivatedRigidbodies.AddRange(GetComponents<Rigidbody>());
        for (int i = deactivatedColliders.Count - 1; i >= 0; i--)
        {
            if (!deactivatedColliders[i].enabled) deactivatedColliders.RemoveAt(i);
            else deactivatedColliders[i].enabled = false;
        }
        for (int i = deactivatedRigidbodies.Count - 1; i >= 0; i--)
        {
            if (deactivatedRigidbodies[i].isKinematic) deactivatedRigidbodies.RemoveAt(i);
            else
            {
                deactivatedRigidbodies[i].isKinematic = true;
                deactivatedRigidbodies[i].detectCollisions = false;
            }
        }


        gameObject.SetActive(true);

        OnEquip(owner, trigger);
    }

    [PunRPC]
    protected void RpcUnequip()
    {
        OnUnequip(owner, trigger);
        trigger.connectedGems.Remove(this);
        trigger = null;

        for (int i = 0; i < deactivatedColliders.Count; i++)
        {
            deactivatedColliders[i].enabled = true;
        }
        for (int i = 0; i < deactivatedRigidbodies.Count; i++)
        {
            deactivatedRigidbodies[i].isKinematic = false;
            deactivatedRigidbodies[i].detectCollisions = true;
        }


        if (deactivatedModel != null) deactivatedModel.gameObject.SetActive(true);

        deactivatedColliders.Clear();
        deactivatedModel = null;

        gameObject.SetActive(false);
    }


    [PunRPC]
    protected void RpcSetLevel(int level)
    {
        this.level = level;
    }

    [PunRPC]
    protected virtual void RpcOnTriggerCast()
    {
        OnTriggerCast(false);
    }

   
}
