using System;
using DrunkenDwarves;

public class Buff : IEquatable<Buff>
{
    public float timeDuration;
    private Timer _expirationTimer;

    public int stackCount = 1;
    protected bool _isCanStack = false;
    public bool IsCanStack => _isCanStack;
    protected bool _isRefreshDurationOnReapply = false;
    public bool IsRefreshDurationOnReapply => _isRefreshDurationOnReapply;

    protected BuffData _data;
    public BuffData BuffData => _data;

    public event Action<Buff> BuffExpired;

    public Buff(Buff buff)
    {
        this.timeDuration = buff.timeDuration;
        _expirationTimer = new Timer(timeDuration, Dispose);

        _data = buff.BuffData;
        _isCanStack = buff._isCanStack;
        _isRefreshDurationOnReapply = buff._isRefreshDurationOnReapply;
    }

    public Buff(float duration, BuffDataScriptable buffData, bool isCanStack = false, bool isRefreshDurationOnIncreaseStack = false)
    {
        this.timeDuration = duration;
        _expirationTimer = new Timer(timeDuration, Dispose);

        _data = buffData.data;
        _isCanStack = isCanStack;
        _isRefreshDurationOnReapply = isRefreshDurationOnIncreaseStack;
    }

    public virtual void UpdateTimer(float deltaTime)
    {
        _expirationTimer.Update(deltaTime);
    }

    public virtual void RefreshDuration()
    {
        _expirationTimer.RestartTimer();
    }

    public virtual void ExpireBuff()
    {
        Dispose();
    }

    public virtual void ApplyEffect(CharacterStats stats, bool isAdditionalStack = false)
    {
        stats.GetStat(BuffData.charBuffType).UpdateTemporaryValue(BuffData.buffValue);
    }

    public virtual void RemoveEffect(CharacterStats stats)
    {
        stats.GetStat(BuffData.charBuffType).UpdateTemporaryValue(-BuffData.buffValue);
    }

    protected void Dispose()
    {
        BuffExpired?.Invoke(this);
    }

    public bool Equals(Buff other)
    {
        return this.BuffData == other.BuffData;
    }
}

[System.Serializable]
public class BuffData
{
    public CharacterStatType charBuffType;
    public SpellStatType spellBuffType;
    public float buffValue;
    //flag to see if stat temporary value should a percentage of max stat value
    public bool isPercentageOfMaxValue;
}