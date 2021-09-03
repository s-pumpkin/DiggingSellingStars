using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ScoreManager>();
            return instance;
        }
    }

    public int Score = 0;
    public Text ScoreText;
    void Start()
    {

    }

    void Update()
    {
        ScoreText.text = "¤À¼Æ: " + Score;
    }

    public void AddScore(int score)
    {
        Score += score;
    }
}
