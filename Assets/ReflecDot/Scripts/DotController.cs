using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DotController : MonoBehaviour
{
    [SerializeField] private float _speed = 500f;
    [SerializeField, Range(0f, 200f)] private float _speedRatio = 100f;
    [SerializeField] private int _addScore = 100;

    [SerializeField] private TextMeshProUGUI _score;

    private Rigidbody2D rb;
    private Transform _dotTransform;

    private int _reflectCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        _dotTransform = transform;
        rb.velocity = new Vector2(_speed, _speed);
        PlayerPrefs.SetInt("SCORE", 0);
        PlayerPrefs.Save();
        _reflectCnt = 0;

        Vector3 v = transform.position;
        transform.position = new Vector3(v.x, v.y, -10);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // ドットとプレイヤーの衝突時処理
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 playerPos = collision.transform.position;
            Vector3 dotPos = _dotTransform.position;

            Vector3 dir = (dotPos - playerPos).normalized;

            _reflectCnt++;
            float currentSpeed = rb.velocity.magnitude + (_reflectCnt * _speedRatio);

            rb.velocity = dir * currentSpeed;

            AddScore();
        }
    }

    private void AddScore()
    {
        int currentScore = PlayerPrefs.GetInt("SCORE") + (_addScore * _reflectCnt);

        PlayerPrefs.SetInt("SCORE", currentScore);
        PlayerPrefs.Save();

        _score.text = currentScore.ToString();
    }
}
