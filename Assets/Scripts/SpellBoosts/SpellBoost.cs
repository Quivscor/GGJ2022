using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public abstract class SpellBoost
{
	public float costModifier = 0f;
	public string spellName = "Nazwa";
	public string description = "";
	public Image icon;
	public SpellType spellType = SpellType.Any;
	public abstract void ProcessSpellBoost(Spell spell);
}
