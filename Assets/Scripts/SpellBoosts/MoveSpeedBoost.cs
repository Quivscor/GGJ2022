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
        description = "You walk much faster.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.GetComponent<CharacterMovement>().IncreaseMovementSpeed(2f);
    }
}
