using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Text _scoreText;
    [SerializeField] Text _liveText;
    [SerializeField] Text _countToStartText;
    [SerializeField] Text _gameOverText;
    [SerializeField] Text _addScoreText;

    private bool _isCountdownFinished = false;  // Biến cờ để theo dõi trạng thái đếm ngược

    void Start()
    {
        StartCoroutine(CountdownToStart());
        _scoreText.text = "Scores: 0";
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_isCountdownFinished)  // Chỉ cập nhật nếu đếm ngược đã hoàn thành
        {
            _scoreText.text = "Scores: " + GameManager.Instance.Score.ToString();
            _liveText.text = "x " + GameManager.Instance.Player.GetComponent<PlayerController>().Health.ToString();
        }
    }

    private IEnumerator CountdownToStart()
    {
        GameManager.Instance.Player.GetComponent<PlayerController>().Paused = true;
        _countToStartText.gameObject.SetActive(true);
        _countToStartText.text = "3";
        yield return new WaitForSeconds(1);

        _countToStartText.text = "2";
        yield return new WaitForSeconds(1);

        _countToStartText.text = "1";
        yield return new WaitForSeconds(1);

        _countToStartText.text = "GO!";
        yield return new WaitForSeconds(1);

        _countToStartText.gameObject.SetActive(false);

        GameManager.Instance.Player.GetComponent<PlayerController>().Paused = false;
        CameraController.Instance.StartScrolling();

        _isCountdownFinished = true;  // Đặt biến cờ thành true khi đếm ngược hoàn thành
    }

    public void SetGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
    }

    public void ShowAddScoreText()
    {
        StartCoroutine(DisplayAddScoreText());
    }

    private IEnumerator DisplayAddScoreText()
    {
        _addScoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _addScoreText.gameObject.SetActive(false);
    }
}
