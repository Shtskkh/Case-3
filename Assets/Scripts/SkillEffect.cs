using System;
using Enums;
using UnityEngine;

[Serializable]
public class SkillEffect
{
    private EffectType _effectType;
    private float _value;
    private float _duration;
    private StatType? _targetStat;

    public SkillEffect(EffectType type, float value, float duration)
    {
        _effectType = type;
        _value = value;
        _duration = duration;
    }

    public SkillEffect(EffectType type, float value, float duration, StatType? targetStat)
    {
        _effectType = type;
        _value = value;
        _duration = duration;
        _targetStat = targetStat;
    }

    public void Apply()
    {
        switch (_effectType)
        {
            case EffectType.Damage:
                Debug.Log($"Applying {_effectType} effect: {_value} damage to target.");
                break;
            case EffectType.Heal:
                Debug.Log($"Applying {_effectType} effect: Restoring {_value} HP.");
                break;
            case EffectType.Buff:
                Debug.Log($"Applying {_effectType} effect: Modifying {_targetStat} by {_value} for {_duration} seconds.");
                break;
            case EffectType.Debuff:
                Debug.Log($"Unknown effect type: {_effectType}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}