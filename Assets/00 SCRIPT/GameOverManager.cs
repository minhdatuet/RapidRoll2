using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : Singleton<GameOverManager>
{
    int _yourScore;
    int _highScore;
    [SerializeField] TextMeshProUGUI _yourScoresText;
    [SerializeField] TextMeshProUGUI _highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        _yourScore = PlayerPrefs.GetInt("YourScore");
        _highScore = PlayerPrefs.GetInt("HighScore");

        _yourScoresText.text = "Your Scores: " + _yourScore.ToString();
        _highScoreText.text = "High Scores: " + _highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
