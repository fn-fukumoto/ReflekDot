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

    private RectTransform _mainPanelRect;

    /// <summary>
    /// 開始時に1回だけ呼ばれる処理
    /// </summary>
    void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// 1フレームごとに呼ばれる処理
    /// </summary>
    void Update()
    {
        if (IsGameOver()) GameOver();
    }

    /// <summary>
    /// 一定時間ごとに呼ばれる処理
    /// </summary>
    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// ゲームの初期化処理
    /// ドットの位置を初期化し、TAP TO STARTのみ表示
    /// </summary>
    private void InitializeGame()
    {
        _dot.localPosition = Vector3.zero;
        _startPanel.gameObject.SetActive(true);
        _mainPanel.gameObject.SetActive(false);
        _controlPanel.gameObject.SetActive(false);
        _gameOverPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// TAP TO STARTでタップした時に呼ばれる処理
    /// TAP TO STARTを非表示にし、ゲーム開始
    /// </summary>
    public void OnTapToStart()
    {
        _startPanel.gameObject.SetActive(false);
        _mainPanel.gameObject.SetActive(true);
        _controlPanel.gameObject.SetActive(true);
        _dot.GetComponent<DotController>().InitializeDot();

        _mainPanelRect = _mainPanel.GetComponent<RectTransform>();
    }

    /// <summary>
    /// リトライボタン押下時、ゲームの初期化を行う
    /// </summary>
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
            // 移動処理
            _player.position = new Vector3(
                _player.position.x - _playerSpeed,
                _player.position.y,
                _player.position.z
                );
            // 左端移動制限
            _player.position = _player.position.x < 0
                ? new Vector3(0, _player.position.y, _player.position.z)
                : _player.position;
        }

        // 右移動
        if (R_Flg || Input.GetKey(KeyCode.RightArrow))
        {
            // 移動処理
            _player.position = new Vector3(
                _player.position.x + _playerSpeed,
                _player.position.y,
                _player.position.z
                );
            // 右端移動制限
            _player.position = _player.position.x > _mainPanelRect.rect.width
                ? new Vector3(_mainPanelRect.rect.width, _player.position.y, _player.position.z)
                : _player.position;
        }
    }

    /// <summary>
    /// ゲームオーバーか否か
    /// </summary>
    /// <returns></returns>
    private bool IsGameOver()
    {
        return _dot.position.y < 0;
    }

    /// <summary>
    /// ゲームオーバー時処理
    /// 該当画面を表示し、スコアを更新
    /// </summary>
    private void GameOver()
    {
        _gameOverPanel.gameObject.SetActive(true);
        _score.text = "SCORE: " + PlayerPrefs.GetInt("SCORE").ToString();
    }
}
