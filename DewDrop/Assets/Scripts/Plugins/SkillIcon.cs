﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public int skillIndex = 1;

    private Image image_icon;
    private Image image_disabled;
    private TextMeshProUGUI tmpu_cooldown;
    private Image image_cooldownFill;
    private Image image_specialFill;
    private Image image_specialIndicator;

    private PointerEventData hover;
    public void OnPointerEnter(PointerEventData data)
    {
        if(UnitControlManager.instance.selectedUnit.control.skillSet[skillIndex] != null)
        {
            DescriptionBox.ShowDescription(UnitControlManager.instance.selectedUnit.control.skillSet[skillIndex]);
            hover = data;
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if(hover != null)
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
        image_specialFill = transform.Find("Special Fill Image").GetComponent<Image>();
        tmpu_cooldown = transform.Find("Mask/Cooldown Text").GetComponent<TextMeshProUGUI>();
        image_cooldownFill = transform.Find("Mask/Cooldown Fill Image").GetComponent<Image>();
        image_specialIndicator = transform.Find("Special Indicator").GetComponent<Image>();
    }
    private void Update()
    {
        LivingThing target = UnitControlManager.instance.selectedUnit;

        if (target == null || !target.photonView.IsMine)
        {
            image_icon.sprite = null;
            image_disabled.enabled = true;
            tmpu_cooldown.text = "";
            image_cooldownFill.fillAmount = 0f;
            return;
        }

        if(target.control.skillSet[skillIndex] == null)
        {
            image_icon.sprite = null;
            image_icon.color = new Color(0.2f, 0.2f, 0.2f);
            image_disabled.enabled = true;
            image_specialFill.fillAmount = 0f;
            image_specialIndicator.enabled = image_specialFill.fillAmount != 0;
            image_cooldownFill.fillAmount = 0f;
        }
        else
        {
            image_icon.color = new Color(1f, 1f, 1f);
            image_icon.sprite = target.control.skillSet[skillIndex].abilityIcon ?? null;
            image_disabled.enabled = !target.control.skillSet[skillIndex].isCooledDown || !target.control.skillSet[skillIndex].IsReady() || !target.control.skillSet[skillIndex].selfValidator.Evaluate(target) || !target.HasMana(target.control.skillSet[skillIndex].manaCost);
            if (skillIndex == 0) image_disabled.enabled = false;
            image_specialFill.fillAmount = target.control.skillSet[skillIndex].GetSpecialFillAmount();
            if(skillIndex == 0)
            {
                image_cooldownFill.fillAmount = 0f;
            }
            else
            {
                image_cooldownFill.fillAmount = target.control.skillSet[skillIndex].cooldownTime == 0 ? 0 : target.control.cooldownTime[skillIndex] / target.control.skillSet[skillIndex].cooldownTime;
            }
            
            image_specialIndicator.enabled = image_specialFill.fillAmount != 0 && !image_disabled.enabled;
        }

        if(target.control.cooldownTime[skillIndex] == 0f || skillIndex == 0)
        {
            tmpu_cooldown.text = "";
        }
        else
        {
            if(target.control.cooldownTime[skillIndex] > 1)
            {
                tmpu_cooldown.text = ((int)target.control.cooldownTime[skillIndex] + 1).ToString();
            }
            else
            {
                tmpu_cooldown.text = target.control.cooldownTime[skillIndex].ToString("F1");
            }
            
        }


    }
}
