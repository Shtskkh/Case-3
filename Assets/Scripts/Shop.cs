using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    private SkillsManager _skillsManager;
    private bool _shopIsOpen;

    private void Start()
    {
        _skillsManager = SkillsManager.Singleton;
    }
    
    // Имитация покупки скилла у торговца
    private void Update()
    {
        if (_shopIsOpen)
        {
            BuySkill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        _shopIsOpen = true;
        Debug.Log("Shop opened.");
    }

    private void OnTriggerExit(Collider other)
    {
        _shopIsOpen = false;
        Debug.Log("Shop closed.");
    }
    
    private void BuySkill()
    {
        var newSkill = new Skill(
            "Awesome title", 
            SkillType.Attack, 
            "Awesome description", 
            new SkillEffect(EffectType.Damage, 5f, 0f),
            new SkillEffect(EffectType.Damage, 10f, 0f),
            5f,
            false);
        _skillsManager.AddSkillToAvailable(newSkill);
    }
}