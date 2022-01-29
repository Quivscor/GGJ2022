using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    public static CharacterHolder Instance;

    [SerializeField] Transform _player;
    public Transform Player => _player;
    [SerializeField] List<Transform> _characters;
    public List<Transform> Characters => _characters;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
}
