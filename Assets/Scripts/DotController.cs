using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour
{
    [SerializeField] private float _speed = 500f;

    private Rigidbody2D rb;
    private Transform _dotTransform;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDot();
        PlayerPrefs.SetInt("SCORE", 0);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeDot()
    {
        rb = GetComponent<Rigidbody2D>();
        _dotTransform = transform;
        rb.velocity = new Vector2(_speed, _speed);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // ドットとプレイヤーの衝突時処理
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 playerPos = collision.transform.position;
            Vector3 dotPos = _dotTransform.position;

            Vector3 dir = (dotPos - playerPos).normalized;

            float currentSpeed = rb.velocity.magnitude;

            rb.velocity = dir * currentSpeed;
        }

        AddScore();
    }

    private void AddScore()
    {
        int currentScore = PlayerPrefs.GetInt("SCORE");

        PlayerPrefs.SetInt("SCORE", currentScore + 10);
        PlayerPrefs.Save();
    }
}
