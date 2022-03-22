using System;
using DrunkenDwarves;

public class Buff
{
    public float timeDuration;
    private Timer _expirationTimer;

    public int stackCount = 1;
    protected bool _isCanStack = false;
    protected bool _isRefreshDurationOnIncreaseStack = false;
    public bool IsRefreshDurationOnIncreaseStack => _isRefreshDurationOnIncreaseStack;

    protected BuffData _data;
    public BuffData BuffData => _data;

    public event Action<Buff> BuffExpired;

    public Buff(Buff buff)
    {
        this.timeDuration = buff.timeDuration;
        _expirationTimer = new Timer(timeDuration, Dispose);

        _data = buff.BuffData;
        _isCanStack = buff._isCanStack;
        _isRefreshDurationOnIncreaseStack = buff._isRefreshDurationOnIncreaseStack;
    }

    public Buff(float duration, BuffDataScriptable buffData, bool isCanStack = false, bool isRefreshDurationOnIncreaseStack = false)
    {
        this.timeDuration = duration;
        _expirationTimer = new Timer(timeDuration, Dispose);

        _data = buffData.data;
        _isCanStack = isCanStack;
        _isRefreshDurationOnIncreaseStack = isRefreshDurationOnIncreaseStack;
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

    protected void Dispose()
    {
        BuffExpired?.Invoke(this);
    }
}

[System.Serializable]
public class BuffData
{
    public CharacterStatType charBuffType;
    public SpellStatType spellBuffType;
    public float buffValue;
}