﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ConsumableIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int consumableIndex = 1;

    private Image image_icon;
    private Image image_disabled;


    private PlayerItemBelt belt;

    private PointerEventData hover = null;
    public void OnPointerEnter(PointerEventData data)
    {
        if (belt.consumableBelt[consumableIndex] != null)
        {
            DescriptionBox.ShowDescription(belt.consumableBelt[consumableIndex]);
            hover = data;
        }
        
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (hover != null)
        {
            DescriptionBox.HideDescription();
            hover = null;
        }
    }

    private void OnDisable()
    {
        if (hover != null)
        {
            DescriptionBox.HideDescription();
            hover = null;
        }
    }

    private void Awake()
    {
        image_icon = transform.Find("Mask/Icon Image").GetComponent<Image>();
        image_disabled = transform.Find("Mask/Disabled Image").GetComponent<Image>();
    }
    private void Update()
    {
        if (UnitControlManager.instance.selectedUnit == null) return;
        if (belt == null) belt = GameManager.instance.localPlayer.GetComponent<PlayerItemBelt>();
        
        if (belt == null || !GameManager.instance.localPlayer.photonView.IsMine)
        {
            image_icon.sprite = null;
            image_disabled.enabled = true;
            return;
        }

        if (belt.consumableBelt[consumableIndex] == null)
        {
            image_icon.sprite = null;
            image_icon.enabled = false;
            image_disabled.enabled = false;
        }
        else
        {
            image_icon.sprite = belt.consumableBelt[consumableIndex].itemIcon;
            image_icon.color = new Color(1f, 1f, 1f);
            image_icon.enabled = true;
            image_disabled.enabled = !(belt.consumableBelt[consumableIndex].selfValidator.Evaluate(GameManager.instance.localPlayer) && belt.consumableBelt[consumableIndex].IsReady());
        }



    }
}
