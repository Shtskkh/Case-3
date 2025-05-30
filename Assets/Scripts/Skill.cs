using System;
using Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class Skill
{
    [SerializeField] private string title;
    [SerializeField] private SkillType type;
    [SerializeField] private string description;
    [SerializeField] private SkillEffect baseEffect;
    [SerializeField] private SkillEffect enhancedEffect;
    [SerializeField] private float baseCooldown;
    [SerializeField] private float currentCooldown;
    [SerializeField] private bool isOnCooldown;
    [SerializeField] private int useCount;
    [SerializeField] private bool isElit;

    public string Title
    {
        get => title;
        private set => title = value;
    }

    public SkillType Type
    {
        get => type;
        private set => type = value;
    }

    public string Description
    {
        get => description;
        private set => description = value;
    }

    public SkillEffect BaseEffect
    {
        get => baseEffect;
        private set => baseEffect = value;
    }

    public SkillEffect EnhancedEffect
    {
        get => enhancedEffect;
        private set => enhancedEffect = value;
    }

    public float BaseCooldown
    {
        get => baseCooldown;
        private set => baseCooldown = value;
    }

    public bool IsOnCooldown
    {
        get => isOnCooldown;
        private set => isOnCooldown = value;
    }

    public int UseCount
    {
        get => useCount;
        private set => useCount = value;
    }

    public bool IsElit
    {
        get => isElit;
        private set => isElit = value;
    }

    public Skill(string title, SkillType type, string description, SkillEffect baseEffect, SkillEffect enhancedEffect,
        float baseCooldown, bool isElit)
    {
        Title = title;
        Type = type;
        Description = description;
        BaseEffect = baseEffect;
        EnhancedEffect = enhancedEffect;
        BaseCooldown = baseCooldown;
        IsOnCooldown = false;
        IsElit = isElit;
    }
    
    public void Activate()
    {
        if (!isOnCooldown)
        {
            useCount++;
            isOnCooldown = true;
            var effectToApply = isElit ? enhancedEffect : baseEffect;
            effectToApply.Apply();
            Debug.Log($"Skill {title} activated. Use count: {useCount}");
        }
        else
        {
            Debug.Log($"Skill {title} is on cooldown. Time remaining: {currentCooldown}");
        }
    }

    public void MakeElite()
    {
        if (!isElit)
        {
            isElit = true;
        }
    }
    
    public void StartCooldown(float multiplier)
    {
        currentCooldown = baseCooldown * multiplier;
        isOnCooldown = true;
        Debug.Log($"Skill {title} started cooldown.");
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (!isOnCooldown) return;
        currentCooldown -= deltaTime;
        if (!(currentCooldown <= 0f)) return;
        isOnCooldown = false;
        currentCooldown = 0f;
        Debug.Log($"Skill {title} is ready to use again.");
    }
}