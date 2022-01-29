using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    public static CharacterHolder Instance;

    [SerializeField] Transform _player;
    public Transform Player => _player;
    [SerializeField] List<Transform> _characters;
    List<Transform> _deadCharacters;
    public List<Transform> Characters => _characters;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        _deadCharacters = new List<Transform>();

        foreach (Transform t in Characters)
        {
            if (t != _player)
            {
                if (t.TryGetComponent(out CharacterStats stats))
                {
                    stats.died += OnCharacterDeath;
                }
            }
            else
            {
                if (t.TryGetComponent(out CharacterStats stats))
                {
                    stats.died += OnPlayerDeath;
                }
            }
        }
    }

    public void OnCharacterDeath(CharacterStats stats)
    {
        if(!_deadCharacters.Contains(stats.transform))
            _deadCharacters.Add(stats.transform);

        //check if all but player are dead
        if(_deadCharacters.Count == _characters.Count - 1)
        {
            MatchController.Instance.ChangeMatchState(MatchState.FINISHED);
        }
    }

    public void OnPlayerDeath(CharacterStats stats)
    {
        Debug.LogError("Kurwa nie ten projekt");
    }
}
