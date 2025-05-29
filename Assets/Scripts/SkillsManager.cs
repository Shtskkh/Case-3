using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SkillsManager : MonoBehaviour, IManager
{
    private List<Skill> _allSkills;
    private List<Skill> _availableSkills;
    private List<Skill> _activeSkills;
    private int _skillsOnCooldown;

    private void Start()
    {
        Load();
        GenerateSkillSelection();
        SelectInitialSkills();
    }

    private void Update()
    {
        UpdateSkillsCoolDown();

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            ActivateSkill(1);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ActivateSkill(2);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ActivateSkill(3);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            ActivateSkill(4);
        }
    }

    private void OnDestroy()
    {
        Save();
    }

    private void InitSkillPool()
    {
        _allSkills = new List<Skill>
        {
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Fireball", SkillType.Attack,
                "Launches a fireball in the direction of the mouse.",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
        };
    }
    private void GenerateSkillSelection()
    {
        _availableSkills.Clear();
        var tempPool = new List<Skill>(_allSkills);
        for (var i = 0; i < 8; i++)
        {
            if (tempPool.Count == 0) break;
            var randomIndex = Random.Range(0, tempPool.Count);
            _availableSkills.Add(tempPool[randomIndex]);
            tempPool.RemoveAt(randomIndex);
        }

        Debug.Log("Generated " + _availableSkills.Count + " skills for selection: " +
                  string.Join(", ", _availableSkills.Select(s => s.Title)));
    }

    // Выбор первых 4 навыков (эмуляция выбора игрока)
    private void SelectInitialSkills()
    {
        for (var i = 0; i < 4 && i < _availableSkills.Count; i++)
        {
            _activeSkills.Add(_availableSkills[i]);
        }

        Debug.Log("Player selected " + _activeSkills.Count + " skills: " +
                  string.Join(", ", _activeSkills.Select(s => s.Title)));
    }

    private void ActivateSkill(int index)
    {
        if (index < 0 || index >= _activeSkills.Count)
        {
            Debug.Log("Invalid skill index: " + index);
            return;
        }

        var skill = _activeSkills[index];
        if (!skill.IsOnCooldown)
        {
            skill.Activate();
            _skillsOnCooldown++;
            var cooldownMultiplier = 1f + 0.1f * _skillsOnCooldown;
            skill.StartCooldown(cooldownMultiplier);
        }
        else
        {
            Debug.Log($"Cannot activate {skill.Title}. It is on cooldown.");
        }
    }

    public void AddSkill(Skill newSkill)
    {
        _activeSkills.Add(newSkill);
        Debug.Log($"New skill added: {newSkill.Title}. Total active skills: {_activeSkills.Count}");
    }

    private void UpdateSkillsCoolDown()
    {
        var prevSkillsOnCooldown = _skillsOnCooldown;
        _skillsOnCooldown = 0;

        foreach (var skill in _activeSkills)
        {
            skill.UpdateCooldown(Time.deltaTime);
            if (skill.IsOnCooldown)
                _skillsOnCooldown++;
        }

        if (prevSkillsOnCooldown != _skillsOnCooldown)
        {
            Debug.Log($"Skills on cooldown: {_skillsOnCooldown}");
        }
    }

    public List<Skill> GetSkillsByType(SkillType type)
    {
        var skills = _allSkills.Where(s => s.Type == type).ToList();
        if (skills.Count != 0)
            return skills;

        Debug.Log($"No skills found for category: {type}");
        return null;
    }

    public void Save()
    {
        var json = JsonUtility.ToJson(new SkillsWrapper
        {
            allSkills = _allSkills,
            availableSkills = _availableSkills,
            activeSkills = _activeSkills
        });
        File.WriteAllText(Application.persistentDataPath + "/skills.json", json);
    }

    public void Load()
    {
        var path = Application.persistentDataPath + "/skills.json";
        if (!File.Exists(path))
        {
            InitSkillPool();
        }

        var json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<SkillsWrapper>(json);

        _allSkills = wrapper.allSkills;
        _availableSkills = wrapper.availableSkills;
        _activeSkills = wrapper.activeSkills;
    }
}

[Serializable]
public class SkillsWrapper
{
    public List<Skill> allSkills;
    public List<Skill> availableSkills;
    public List<Skill> activeSkills;
}