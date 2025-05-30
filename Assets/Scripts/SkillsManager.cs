using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SkillsManager : MonoBehaviour
{
    public static SkillsManager Singleton { get; private set; }

    private List<Skill> _allSkills;
    private List<Skill> _availableSkills;
    private List<Skill> _activeSkills;
    private int _skillsOnCooldown;

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitSkillPool();
        GenerateSkillSelection();
        SelectInitialSkills();
    }

    private void Update()
    {
        UpdateSkillsCoolDown();

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            ActivateSkill(0);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ActivateSkill(1);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ActivateSkill(2);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            ActivateSkill(3);
        }
    }

    private void InitSkillPool()
    {
        _allSkills = new List<Skill>
        {
            new(
                "Fireball", SkillType.Attack,
                "Description",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                5f,
                false),
            new(
                "Ice Blast", SkillType.Attack,
                "Description.",
                new SkillEffect(EffectType.Damage, 10f, 0),
                new SkillEffect(EffectType.Damage, 20f, 0),
                10f,
                false),
            new(
                "Shield", SkillType.Defense,
                "Description",
                new SkillEffect(EffectType.Buff, 30f, 15f, StatType.Armor),
                new SkillEffect(EffectType.Buff, 60f, 30f, StatType.Armor),
                25f,
                false),
            new(
                "Lightning Strike", SkillType.Attack,
                "Description",
                new SkillEffect(EffectType.Damage, 5f, 0),
                new SkillEffect(EffectType.Damage, 10f, 0),
                12f,
                false),
            new(
                "Invisibility", SkillType.Utility,
                "Description",
                new SkillEffect(EffectType.Buff, 1f, 25f, StatType.Visibility),
                new SkillEffect(EffectType.Buff, 1f, 50f, StatType.Visibility),
                60f,
                false),
            new(
                "Barrier", SkillType.Defense,
                "Description",
                new SkillEffect(EffectType.Buff, 8f, 15f, StatType.Armor),
                new SkillEffect(EffectType.Buff, 16f, 30f, StatType.Armor),
                5f,
                false),
            new(
                "Poison Dart", SkillType.Attack,
                "Description",
                new SkillEffect(EffectType.Damage, 15f, 0),
                new SkillEffect(EffectType.Damage, 30f, 0),
                6f,
                false),
            new(
                "Speed Boost", SkillType.Utility,
                "Description",
                new SkillEffect(EffectType.Buff, 5f, 15f, StatType.Speed),
                new SkillEffect(EffectType.Buff, 10f, 30f, StatType.Speed),
                15f,
                false)
        };
        _availableSkills = new List<Skill>();
        _activeSkills = new List<Skill>();
        _skillsOnCooldown = 0;
        Debug.Log("Skill pool initialized with " + _allSkills.Count + " skills.");
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

    public void AddSkillToPool(Skill newSkill)
    {
        _allSkills.Add(newSkill);
        Debug.Log($"New skill added: {newSkill.Title}. Total active skills: {_activeSkills.Count}");
    }

    public void AddSkillToAvailable(Skill newSkill)
    {
        _availableSkills.Add(newSkill);
        Debug.Log($"New skill added: {newSkill.Title}. Total active skills: {_activeSkills.Count}");
    }

    public void AddSkillToActive(Skill newSkill)
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
}