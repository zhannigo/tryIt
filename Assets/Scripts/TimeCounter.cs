using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public float RoundLength = 0f;

    //public TextMesh textTime;
    public TextMeshProUGUI textTime;

    private float timeKeeper;
    private readonly bool isPaused = false;
    private bool gameIsOver = false;
    public string timeScore;

    void Start()
    {
        timeKeeper = RoundLength;
    }

    void Update()
    {
        if (!isPaused && !gameIsOver)
        {
            timeKeeper += Time.deltaTime;
            DisplayTime(timeKeeper);

            if (timeKeeper > 100f)
            {
                DoGameOver();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay/60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        textTime.text = string.Format("{0:00},{1:00}",minutes, seconds);
        timeScore = textTime.text;
    }

    void DoGameOver()
    {
        gameIsOver = true;
    }
}
