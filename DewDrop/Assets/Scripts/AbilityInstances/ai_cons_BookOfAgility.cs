﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai_cons_BookOfAgility : AbilityInstance
{
    protected override void OnCreate(CastInfo castInfo, object[] data)
    {
        info.owner.stat.agility += 3f;
        if (photonView.IsMine) info.owner.stat.SyncSecondaryStats();
        StartCoroutine(CoroutineFollow());
    }


    IEnumerator CoroutineFollow()
    {
        float start = Time.time;
        while (Time.time - start < 3f)
        {
            transform.position = info.owner.transform.position;
            yield return null;
        }
        DetachChildParticleSystemsAndAutoDelete();
        DestroySelf();
    }
}
