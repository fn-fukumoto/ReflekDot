using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _dot;

    [Header("Panel")]
    [SerializeField] private Transform _startPanel;
    [SerializeField] private Transform _mainPanel;
    [SerializeField] private Transform _controlPanel;
    [SerializeField] private Transform _gameOverPanel;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _resultScore;
    [SerializeField] private TextMeshProUGUI _hiScore;

    [Header("Option")]
    [SerializeField] private float _playerSpeed = 20f;
    [SerializeField] private bool _isButtonControll = true;
    [SerializeField] private float _swipeSensitivity = 1f;

    public bool L_Flg { get; set; } = false;
    public bool R_Flg { get; set; } = false;

    private RectTransform _mainPanelRect;

    private Vector3 _prePos, _curPos;

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
        SwipeMovePlayer();
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
    /// ドットとプレイヤーの位置を初期化し、スコアの初期化、TAP TO STARTのみ表示
    /// </summary>
    private void InitializeGame()
    {
        _dot.localPosition = Vector3.zero;
        _player.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 500, 0);
        _score.text = "0";
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
        _dot.GetComponent<DotController>().Initialize();

        if (_isButtonControll)
        {
            _controlPanel.GetChild(0).gameObject.SetActive(true);
            _controlPanel.GetChild(1).gameObject.SetActive(false);
        } else
        {
            _controlPanel.GetChild(0).gameObject.SetActive(false);
            _controlPanel.GetChild(1).gameObject.SetActive(true);
        }

        _mainPanelRect = _mainPanel.GetComponent<RectTransform>();
    }

    /// <summary>
    /// リトライボタン押下時、ゲームの初期化を行う
    /// </summary>
    public void OnClickRetryButton()
    {
        InitializeGame();
    }

    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    public void MovePlayer()
    {
        if (!_isButtonControll) return;

        // 左移動
        if (L_Flg || Input.GetKey(KeyCode.LeftArrow))
        {
            // 移動処理
            _player.transform.Translate(-_playerSpeed, 0, 0);
        }

        // 右移動
        if (R_Flg || Input.GetKey(KeyCode.RightArrow))
        {
            // 移動処理
            _player.transform.Translate(_playerSpeed, 0, 0);
        }

        checkPlayerPositionLimit();
    }

    /// <summary>
    /// プレイヤーの移動処理（スワイプ操作）
    /// </summary>
    public void SwipeMovePlayer()
    {
        if (_isButtonControll) return;

        if (Input.GetMouseButton(0))
        {
            // 移動
            _player.transform.Translate(
                Input.touches[0].deltaPosition.x * _swipeSensitivity, 0, 0);
        }

        checkPlayerPositionLimit();
    }

    /// <summary>
    /// 移動制限処理
    /// </summary>
    public void checkPlayerPositionLimit()
    {
        var px = _player.position.x;
        var py = _player.position.y;
        var pz = _player.position.z;
        var minX = 0f;
        var maxX = _mainPanelRect != null ? _mainPanelRect.rect.width : 0f;

        if (px < minX)
        {
            _player.position = new Vector3(minX, py, pz);
        }
        else if(px > maxX)
        {
            _player.position = new Vector3(maxX, py, pz);
        }
    }

    /// <summary>
    /// ゲームオーバーか否か
    /// </summary>
    /// <returns></returns>
    private bool IsGameOver()
    {
        return _dot.position.y < _player.position.y;
    }

    /// <summary>
    /// ゲームオーバー時処理
    /// 該当画面を表示し、スコアを更新
    /// </summary>
    private void GameOver()
    {
        _gameOverPanel.gameObject.SetActive(true);
        _mainPanel.gameObject.SetActive(false);
        _controlPanel.gameObject.SetActive(false);

        int score = PlayerPrefs.GetInt("SCORE", 0);
        int hiScore = PlayerPrefs.GetInt("HISCORE", 0);

        _resultScore.text = score.ToString();
        if (score > hiScore)
        {
            _hiScore.text = score.ToString();
            PlayerPrefs.SetInt("HISCORE", score);
            PlayerPrefs.Save();
        } else
        {
            _hiScore.text = hiScore.ToString();
        }
    }
}
