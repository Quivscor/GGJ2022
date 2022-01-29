using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    public static CharacterHolder Instance;

    [SerializeField] CharacterStats _player;
    public CharacterStats Player => _player;
    [SerializeField] List<CharacterStats> _characters;
    List<CharacterStats> _deadCharacters;
    public List<CharacterStats> Characters => _characters;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        _deadCharacters = new List<CharacterStats>();

        foreach (CharacterStats t in Characters)
        {
            if (t != _player)
            {
                t.died += OnCharacterDeath;
            }
            else
            {
                t.died += OnPlayerDeath;
            }
        }
    }

    public void OnCharacterDeath(CharacterStats stats)
    {
        if(!_deadCharacters.Contains(stats))
        {
            _deadCharacters.Add(stats);
            stats.GetComponent<Collider>().isTrigger = true;
        }
            

        //check if all but player are dead
        if(_deadCharacters.Count == _characters.Count - 1)
        {
            MatchController.Instance.ChangeMatchState(MatchState.ACTIVE);
        }
    }

    public void OnPlayerDeath(CharacterStats stats)
    {
        Debug.LogError("Kurwa nie ten projekt");
    }

    public void ResetEveryone()
    {
        foreach(CharacterStats t in Characters)
        {
            if(t != _player)
            {
                t.Resurect();
                t.GetComponent<Collider>().isTrigger = false;
                LevelUp.Instance.AddRandomLevelForBot(t, t.GetComponent<WandController>());
            }
        }
    }
}
