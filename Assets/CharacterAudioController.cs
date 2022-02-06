using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterAudioController : MonoBehaviour
{
    public AudioSource leftSpellSource;
    public AudioSource rightSpellSource;

    private void Awake()
    {
        if(this.transform.root.TryGetComponent(out SpellcastingController spells))
            spells.SpellFired += PlaySpellSound;
    }

    private void PlaySpellSound(SpellcastEventData data)
    {
        if (data.type == SpellType.Left)
            if(leftSpellSource != null)
                leftSpellSource.Play();
        else if (data.type == SpellType.Right)
            if(rightSpellSource != null)
                rightSpellSource.Play();
    }
}
