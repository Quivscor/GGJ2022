using System;

[System.Serializable]
public sealed class GameStat
{
    public readonly float InitValue;
    private float _baseValue; //value with permanent buffs
    private float _currentValue; //value for stats with max X and X as values (like HP)
    private float _temporaryValue; //value with temporary buffs
    public float Current => _currentValue;
    public float Max => _baseValue + _temporaryValue;

    public event Action MaxValueChanged;
    public event Action CurrentValueChanged;
    public event Action TempValueChanged;
    public event Action AnyValueChanged;

    public GameStat()
    {
        _baseValue = InitValue = 0;
        Init();
    }

    public GameStat(float initValue)
    {
        _baseValue = InitValue = initValue;
        Init();
    }

    private void Init()
    {
        _currentValue = _baseValue;
        _temporaryValue = 0;
    }

    public void UpdateBaseValue(float value)
    {
        _baseValue += value;
        MaxValueChanged?.Invoke();
        AnyValueChanged?.Invoke();
    }

    public void SetCurrentValue(float value)
    {
        _currentValue = value;
        CurrentValueChanged?.Invoke();
        AnyValueChanged?.Invoke();
    }

    public void UpdateCurrentValue(float value)
    {
        _currentValue += value;
        if (_currentValue > Max)
            _currentValue = Max;
        CurrentValueChanged?.Invoke();
        AnyValueChanged?.Invoke();
    }

    public void UpdateTemporaryValue(float value)
    {
        _temporaryValue += value;
        MaxValueChanged?.Invoke();
        TempValueChanged?.Invoke();
        AnyValueChanged?.Invoke();
    }

    public void ClearTemporaryValue()
    {
        _temporaryValue = 0;
        MaxValueChanged?.Invoke();
        TempValueChanged?.Invoke();
        AnyValueChanged?.Invoke();
    }
}
