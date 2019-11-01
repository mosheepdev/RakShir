﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Photon.Pun;
using UnityEngine.AI;
public enum RoomClearType { AlwaysCleared, ClearWhenAllDead, ClearManually }

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(NavMeshSurface))]
public class Room : MonoBehaviourPun
{

    public List<Room> nextRooms;
    public Transform entryPoint;

    public bool isActivated { get; private set; }

    [Header("Enemy Spawning Settings")]
    public float spawnDelay = 0.5f;
    [ReorderableList]
    public List<Spawner> spawners;

    [Button("Add Stray Spawners")]
    private void AddStraySpawners()
    {
        Spawner[] spawners = GetComponentsInChildren<Spawner>();
        for (int i = 0; i < spawners.Length; i++)
        {
            if (!this.spawners.Contains(spawners[i])) this.spawners.Add(spawners[i]);
        }
    }

    [InfoBox("Null element will delay the next spawn until all previous spawned LivingThings are dead.")]
    public RoomClearType clearType = RoomClearType.ClearWhenAllDead;

    private bool manualClearFlag = false;

    private void Start()
    {
        Transform map = transform.Find("/Map");
        if(map != null)
        {
            transform.parent = map; 
        }
    }




    public bool IsCleared()
    {
        switch (clearType)
        {
            case RoomClearType.AlwaysCleared:
                return true;
            case RoomClearType.ClearWhenAllDead:
                for(int i = 0; i < spawners.Count; i++)
                {
                    if (spawners[i] == null) continue;
                    if (!spawners[i].IsCleared()) return false;
                }
                return true;
            case RoomClearType.ClearManually:
                return manualClearFlag;
        }


        return false;
    }

    public void ManuallyClearRoom()
    {
        photonView.RPC("RpcManuallyClearRoom", RpcTarget.All);
    }



    public void ActivateRoom(LivingThing activator)
    {
        if (isActivated) return;
        photonView.RPC("RpcActivateRoom", RpcTarget.All, activator.photonView.ViewID);
    }


    [PunRPC]
    private void RpcActivateRoom(int activator_id)
    {
        isActivated = true;
        if (!PhotonNetwork.GetPhotonView(activator_id).IsMine) return;
        StartCoroutine("CoroutineSpawn");

    }

    private IEnumerator CoroutineSpawn()
    {
        int i = 0;
        while (i < spawners.Count)
        {
            if (spawners[i] == null)
            {
                bool allCleared = true;
                for (int j = 0; j < i; j++)
                {
                    if (spawners[j] == null) continue;
                    if (!spawners[j].IsCleared())
                    {
                        allCleared = false;
                        break;
                    }
                }
                if (!allCleared)
                {
                    yield return new WaitForSeconds(spawnDelay);
                    continue;
                }
            }
            else
            {
                spawners[i].Spawn();
                yield return new WaitForSeconds(spawnDelay);
            }
            i++;
        }


    }

    [PunRPC]
    private void RpcManuallyClearRoom()
    {
        manualClearFlag = true;
    }


}
