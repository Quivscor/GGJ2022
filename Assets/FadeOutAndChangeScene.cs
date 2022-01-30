using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOutAndChangeScene : MonoBehaviour
{
    bool isInRange = false;
    [SerializeField] GameObject continuePopup;
    public AnimationCurve curve;
    public Image img;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            continuePopup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            continuePopup.SetActive(false);
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeOut(.5f));
        }
    }

    public IEnumerator FadeOut(float t)
    {
        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
        ChangeScene();
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
