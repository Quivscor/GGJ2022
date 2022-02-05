using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTime : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseCanvas;

    private static float m_TimeScale;
    public static float TimeScale
    {
        get => m_TimeScale;
        private set
        {
            m_TimeScale = value;
            Time.timeScale = m_TimeScale;
        }
    }

    private void Start()
    {
        m_TimeScale = Time.timeScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    public void PauseGame()
    {
        if (TimeScale == 0)
        {
            TimeScale = 1;
            pauseCanvas.SetActive(false);
        }
        else
        {
            TimeScale = 0;
            pauseCanvas.SetActive(true);
        }
    }
}
