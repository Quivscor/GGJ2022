using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class ResourceChanged : UnityEvent <float> { }

public class CharacterStats : MonoBehaviour
{
    [Header("Debug options")]
    [SerializeField]
    private bool infiniteResources = false;
    [SerializeField]
    private bool immortality = false;

    [Header("References")]
    [SerializeField]
    private Image healthBarFill;
    [SerializeField]
    private TextMeshProUGUI wizardNameTMP;
    [SerializeField]
    private GameObject upgradeIconsHolder;

    [Header("Stats")]
    public string wizardName;

    //use this to initialize stats with values
    [Header("Base stats")]
    [SerializeField] private List<StatToChange> initialStatValues;
    private Dictionary<CharacterStatType, GameStat> _statsDictionary;
    [SerializeField] private Spell _leftSpell;
    [SerializeField] private Spell _rightSpell;
    public Spell LeftSpell => _leftSpell;
    public Spell RightSpell => _rightSpell;

    [SerializeField]
    private int harmonyLevel = 0;
    [SerializeField]
    private int chaosLevel = 0;

    #region Buffs
    private List<Buff> _activeBuffs;
    #endregion

    [Header("Event actions")]
    [SerializeField]
    private ResourceChanged resourceChanged;
    [SerializeField]
    private ResourceChanged healthChanged;
    public event Action<Transform, float> damageReceived;

    public Action<CharacterStats> died;
    public bool isDead { get; private set; }


    public int HarmonyLevel { get => harmonyLevel; }
    public int ChaosLevel { get => chaosLevel; }
    public bool Immortality { get => immortality; set => immortality = value; }
    public bool InfiniteResources { get => infiniteResources; set => infiniteResources = value; }

    public AudioSource healAudio;

    public void PlayHealSound()
    {
        if (healAudio != null && transform.CompareTag("Player"))
            healAudio.Play();
    }

    private void Awake()
    {
        _statsDictionary = new Dictionary<CharacterStatType, GameStat>();
        for (CharacterStatType i = 0; i != CharacterStatType.UNDEFINED; i++)
        {
            float inspectorStat = initialStatValues.Find((x) => x.CharacterStatType == i).value;
            _statsDictionary.Add(i, new GameStat(inspectorStat));
        }

        _activeBuffs = new List<Buff>();
    }

    void Start()
    {
        if(wizardNameTMP != null)
        wizardNameTMP.text = wizardName;

        GameStat health = _statsDictionary[CharacterStatType.Health];
        if (healthBarFill != null)
            healthBarFill.fillAmount = health.Current / health.Max;
        _statsDictionary[CharacterStatType.Resource].SetCurrentValue(_statsDictionary[CharacterStatType.Resource].Max / 2);
        resourceChanged?.Invoke(_statsDictionary[CharacterStatType.Resource].Current / _statsDictionary[CharacterStatType.Resource].Max);
        isDead = false;
    }

    private void Update()
    {
        float time = Time.deltaTime;

        //has to be new list, because it is being updated in real time
        foreach (Buff b in new List<Buff>(_activeBuffs))
            b.UpdateTimer(time);
    }

    public GameStat GetStat(CharacterStatType type)
    {
        return _statsDictionary[type];
    }

    public void ApplyBuff(Buff buff)
    {
        if(_activeBuffs.Contains(buff))
        {
            Buff b = _activeBuffs.Find((x) => x.Equals(buff));
            if(b.IsCanStack)
            {
                b.stackCount++;
                b.ApplyEffect(this, true);
            }
            if (b.IsRefreshDurationOnReapply)
                b.RefreshDuration();
        }
        else
        {
            _activeBuffs.Add(buff);
            buff.ApplyEffect(this);
            buff.BuffExpired += RemoveBuff;
        }
    }

    public void RemoveBuff(Buff buff)
    {
        _activeBuffs.Remove(buff);

        buff.RemoveEffect(this);
    }

    public void UpdateBaseStat(CharacterStatType type, float value, bool isMultiplicative = false)
    {
        if (isMultiplicative)
            _statsDictionary[type].UpdateBaseValue(_statsDictionary[type].InitValue * value);
        else
            _statsDictionary[type].UpdateBaseValue(value);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddUpgradeIcon(Color upgradeColor)
    {
        var loadedIcon =  Instantiate(Resources.Load<GameObject>("UpgradeIcon"), upgradeIconsHolder.transform);
        loadedIcon.GetComponent<Image>().color = upgradeColor;
    }

    public void Heal(float value)
    {
        GameStat health = _statsDictionary[CharacterStatType.Health];
        health.UpdateCurrentValue(value);

        healthChanged?.Invoke(health.Current/health.Max);
        if(healthBarFill != null)
            healthBarFill.fillAmount = health.Current/health.Max;
    }

    public void Resurect()
    {
        GameStat health = _statsDictionary[CharacterStatType.Health];
        health.SetCurrentValue(health.Max);
        if (healthBarFill != null)
            healthBarFill.fillAmount = health.Current / health.Max;
        isDead = false;
        GetComponent<CharacterMaterialReplacer>().RevertMat();
    }

    public void DealDamage(float value)
    {
        if(!Immortality)
        {
            GameStat health = _statsDictionary[CharacterStatType.Health];
            if (value > 0)
                value *= -1;
            health.UpdateCurrentValue(value);
            if (health.Current <= 0 && !isDead)
            {
                died?.Invoke(this);
                GetComponent<CharacterMaterialReplacer>().UseNewMat();
                isDead = true;
            }

            if (healthBarFill != null)
                healthBarFill.fillAmount = health.Current / health.Max;

            healthChanged?.Invoke(health.Current / health.Max);
            damageReceived?.Invoke(this.transform, value);
        }
    }

    public void CastTestSpell(int spellType)
    {
        SpendResource(10, (SpellType)spellType);
    }
    public bool HaveEnoughResource(float value, SpellType spellType)
    {
        GameStat resource = _statsDictionary[CharacterStatType.Resource];
        if (spellType == SpellType.Left)
        {
            if (resource.Current >= value)
                return true;
            else
                return false;
        }
        else if (resource.Max - resource.Current >= value)
        {
             return true;
        }
        else
            return false;
    }

    public void SpendResource(float value, SpellType spellType)
    {
        GameStat resource = _statsDictionary[CharacterStatType.Resource];
        if (!InfiniteResources)
        {
            if (spellType == SpellType.Left)
            {
                resource.UpdateCurrentValue(-value);
            }
            else
            {
                resource.UpdateCurrentValue(value);
            }
        }

        resourceChanged?.Invoke(resource.Current / resource.Max);
    }

    public void AddLevel(SpellType spellType)
    {
        if (spellType == SpellType.Left)
            harmonyLevel++;
        else
            chaosLevel++;

        //if(transform.root.CompareTag("Player"))
        //    maxHealth += 25;
        //else
        //    maxHealth += 20;
    }
}
