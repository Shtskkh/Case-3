using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private SkillsManager skillsManager;

    private void Awake()
    {
        if (!skillsManager)
            Debug.LogError("No SkillsManager found");
    }

    public void BuySkill()
    {
        
    }
}