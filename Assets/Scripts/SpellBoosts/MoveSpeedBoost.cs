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
        description = "You walk faster.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.GetComponent<CharacterMovement>().IncreaseMovementSpeed(1f);
    }
}
