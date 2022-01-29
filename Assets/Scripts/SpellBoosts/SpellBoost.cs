using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBoost
{
	public float costModifier = 0f;
	public string description = "";
	public SpellType spellType;
	public abstract void ProcessSpellBoost(Spell spell);
}
