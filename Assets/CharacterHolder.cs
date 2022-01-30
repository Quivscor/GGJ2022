using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMProText = TMPro.TextMeshProUGUI;

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
            MatchController.Instance.ChangeMatchState(MatchState.FINISHED);
        }
    }

    public void OnPlayerDeath(CharacterStats stats)
    {
        StartCoroutine(FadeOut(2f));
        StartCoroutine(FadeInText(1.5f));
    }

    public AnimationCurve curve;
    public Image img;
    public TMProText text;
    private IEnumerator FadeOut(float t)
    {
        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
        SceneManager.LoadScene(0);
    }

    private IEnumerator FadeInText(float t)
    {
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            text.color = new Color(1f, 1f, 1f, a);
            yield return 0;
        }
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
        _player.Resurect();
        _deadCharacters.Clear();
        StartCoroutine(ChangeStateAfterTime(MatchState.ACTIVE, 1f));
    }

    public IEnumerator ChangeStateAfterTime(MatchState state, float time)
    {
        yield return new WaitForSeconds(time);
        MatchController.Instance.ChangeMatchState(state);
    }
}
