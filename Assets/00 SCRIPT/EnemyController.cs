using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject groundPrefab;
    private float _groundWidth;
    [SerializeField] float speed = 2.0f; // Tốc độ di chuyển của Enemy
    private Vector2 leftBound, rightBound; // Giới hạn trái và phải của Enemy
    private bool _isDeath = false;
    public bool IsDeath { 
        get { return _isDeath; }
        set { _isDeath = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _groundWidth = groundPrefab.transform.GetComponent<Renderer>().bounds.size.x;
        leftBound = new Vector2(this.transform.position.x - _groundWidth / 2 + 0.1f, transform.position.y);
        rightBound = new Vector2(this.transform.position.x + _groundWidth / 2 - 0.1f, transform.position.y);


    }

    private void OnEnable()
    {
        _isDeath = false;
        GetComponent<Rigidbody2D>().simulated = true;
        // Thiết lập giới hạn trái và phải dựa trên chiều rộng của Ground
        leftBound = new Vector2(this.transform.position.x - _groundWidth / 2 + 0.1f, transform.position.y);
        rightBound = new Vector2(this.transform.position.x + _groundWidth / 2 - 0.1f, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.Player.GetComponent<PlayerController>().Paused && !_isDeath)
        {
            Move();
        }
        
    }

    private void Move()
    {
        if (transform.localScale.x > 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound.x)
            {
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound.x)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Lật nhân vật
        transform.localScale = localScale;
    }
}
