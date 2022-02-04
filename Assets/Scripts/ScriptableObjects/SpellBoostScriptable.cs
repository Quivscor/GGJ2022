using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpellBoost", menuName = "ScriptableObjects")]
public abstract class SpellBoostScriptable : ScriptableObject
{
    public int id;
    public string spellName;
    public string description;
    public Image icon;
    public float resourceModifier;
    public SpellType spellType = SpellType.Any;

    public abstract void ProcessSpellBoost(Spell spell);



}
