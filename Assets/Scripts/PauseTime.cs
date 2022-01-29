using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTime : MonoBehaviour
{
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

        PauseGame();
    }

    public void PauseGame()
    {
        if (TimeScale == 0)
            TimeScale = 1;
        else
            TimeScale = 0;
    }
}
