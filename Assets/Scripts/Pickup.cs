using Enums;
using UnityEngine;

public class Pickup : MonoBehaviour
{ 
    private SkillsManager _skillsManager;

    private void Start()
    {
        _skillsManager = SkillsManager.Singleton;
    }

    // Имитация подбора скилла с предмета
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var skill = new Skill(
                "Title", 
                SkillType.Attack, 
                "Description", 
                new SkillEffect(EffectType.Damage, 5f, 0f),
                new SkillEffect(EffectType.Damage, 10f, 0f),
                5f,
                false);
            
            _skillsManager.AddSkillToAvailable(skill);
            Debug.Log($"Player picked up item and gained skill: {skill.Title}");
            Destroy(gameObject);
        }
    }
}