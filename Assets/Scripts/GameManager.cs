using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _dot;

    [SerializeField] private Transform _startPanel;
    [SerializeField] private Transform _mainPanel;
    [SerializeField] private Transform _controlPanel;
    [SerializeField] private Transform _gameOverPanel;

    [SerializeField] private TextMeshProUGUI _score;

    [SerializeField] private float _playerSpeed = 20f;

    public bool L_Flg { get; set; } = false;
    public bool R_Flg { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameOver()) GameOver();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void InitializeGame()
    {
        _dot.localPosition = Vector3.zero;
        _startPanel.gameObject.SetActive(true);
        _mainPanel.gameObject.SetActive(false);
        _controlPanel.gameObject.SetActive(false);
        _gameOverPanel.gameObject.SetActive(false);
    }

    public void OnTapToStart()
    {
        _startPanel.gameObject.SetActive(false);
        _mainPanel.gameObject.SetActive(true);
        _controlPanel.gameObject.SetActive(true);
        _dot.GetComponent<DotController>().InitializeDot();
    }

    public void OnClickReteryButton()
    {
        InitializeGame();
    }

    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    public void MovePlayer()
    {
        // 左移動
        if (L_Flg || Input.GetKey(KeyCode.LeftArrow))
        {
            _player.position = new Vector3(
                _player.position.x - _playerSpeed,
                _player.position.y,
                _player.position.z
                );
        }

        // 右移動
        if (R_Flg || Input.GetKey(KeyCode.RightArrow))
        {
            _player.position = new Vector3(
                _player.position.x + _playerSpeed,
                _player.position.y,
                _player.position.z
                );
        }
    }

    private bool IsGameOver()
    {
        return _dot.position.y < 0;
    }

    private void GameOver()
    {
        _gameOverPanel.gameObject.SetActive(true);
        _score.text = "SCORE: " + PlayerPrefs.GetInt("SCORE").ToString();
    }
}
