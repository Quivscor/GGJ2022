using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedBoost : SpellBoost
{
    public MoveSpeedBoost()
    {
        costModifier = 0;
        spellType = SpellType.Any;
        spellName = "Fleet Footwork";
        description = "You walk much faster. Additionally +5% damage";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.GetComponent<CharacterMovement>().IncreaseMovementSpeed(1.45f);
        spell.Damage += spell.Damage * 0.05f;
    }
}
