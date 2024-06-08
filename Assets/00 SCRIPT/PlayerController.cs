using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rigi;
    [SerializeField] float Speed = 3.0f;
    [SerializeField] int _health = CONSTANT.MAX_HEALTH;

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }
    [SerializeField] GameObject bulletPrefab;
    GameObject bulletList;
    [SerializeField] Vector2 ForceToJump = new Vector2(0, 150.0f);
    [SerializeField] LayerMask groundLayer; // LayerMask để kiểm tra mặt đất
    [SerializeField] LayerMask fireLayer; // LayerMask để kiểm tra mặt đất
    [SerializeField] AnimationControllerBase _anim;
    private Camera mainCamera;
    private float playerHeight;
    private float playerWidth;
    private float cameraButtom = 5.0f, cameraTop = -5.0f;
    private float cameraLeft, cameraRight;
    private float rayLength;
    [SerializeField] bool _isGrounded = true;
    [SerializeField] bool _isFired = false;
    private bool _isLosed = false;
    private bool _isCollisionEnemy = false;
    private bool _hasDied = false; // Biến mới để kiểm tra trạng thái đã chết
    public bool HasDied
    {
        get { return _hasDied; }
        set { _hasDied = value; }
    }
    [SerializeField] float _shootSpeed = 5.0f;
    [SerializeField] private float _countToShoot = 0.0f;
    [SerializeField] PlayerState _state = PlayerState.IDLE;
    private bool isPaused = false; // Biến để kiểm soát trạng thái tạm dừng
    public bool Paused
    {
        get { return isPaused; }
        set { isPaused = value; }
    }
    void Start()
    {
        _rigi = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        playerHeight = this.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;
        playerWidth = this.transform.GetChild(0).GetComponent<Renderer>().bounds.size.x;
        bulletList = GameObject.Find("BulletList");
        rayLength = playerWidth / 2 + 0.01f;
    }

    void Update()
    {
        if (!isPaused)
        {
            CheckPlayerDeath();
            HandleJump();
            HandleShoot();
            _anim.UpdateAnimation(_state);
            CheckLosed();
        }
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            Move();
            CheckCanShoot();
        }
        else
        {
            _rigi.velocity = Vector3.zero;
        }
    }

    private void CheckPlayerDeath()
    {
        cameraTop = mainCamera.transform.position.y + mainCamera.orthographicSize;
        cameraButtom = mainCamera.transform.position.y - mainCamera.orthographicSize;
        cameraLeft = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;
        cameraRight = mainCamera.transform.position.x + mainCamera.orthographicSize * mainCamera.aspect;

        //Check xem có đi vào vùng lửa không
        Vector2 origin = this.transform.position;

        RaycastHit2D hitDown = Physics2D.Raycast(origin, Vector2.down, rayLength, fireLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin, Vector2.left, rayLength, fireLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(origin, Vector2.right, rayLength, fireLayer);

        if (hitDown.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            _isFired = true;
        }
        else
        {
            _isFired = false;
        }

        if ((this.transform.position.y + playerHeight > cameraTop - 0.5 
            || this.transform.position.y - 0.5 < cameraButtom || _isFired || _isCollisionEnemy ) && !_hasDied)
        {
            _health -= 1;
            _hasDied = true; // Đánh dấu người chơi đã chết
            if (_health == 0)
            {
                _isLosed = true;
            }
            AudioManager.Instance.PlayDeathSound();
            Camera.main.GetComponent<CameraController>().StopScrolling(3f);
            StartCoroutine(HandleDeathAndRespawn()); // Bắt đầu coroutine khi nhân vật chết
        }
    }

    private void CheckLosed()
    {
        if (_isLosed)
        {
            isPaused = true;
            Camera.main.GetComponent<CameraController>().IsScrolling = false;
            UIManager.Instance.SetGameOver();
            AudioManager.Instance.PlayOverSound();
            PlayerPrefs.SetInt("YourScore", GameManager.Instance.Score);
            if (GameManager.Instance.Score > PlayerPrefs.GetInt("HighScore")) 
            {
                PlayerPrefs.SetInt("HighScore", GameManager.Instance.Score);
            }
            StartCoroutine(WaitAndLoadGameOverScene());
        }
    }

    private IEnumerator WaitAndLoadGameOverScene()
    {
        // Đợi 2 giây
        yield return new WaitForSeconds(2f);

        // Chuyển sang GameOverScene
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator HandleDeathAndRespawn()
    {
        isPaused = true; // Tạm dừng di chuyển
        _state = PlayerState.DEATH;
        yield return new WaitForSeconds(1f); // Chờ 1 giây ở trạng thái DEATH

        // Tìm ground thấp nhất đang active
        Transform groundList = GameObject.Find("GroundList").transform;
        Transform lowestActiveGround = null;

        foreach (Transform child in groundList)
        {
            if (child.gameObject.activeSelf)
            {
                if (lowestActiveGround == null || child.position.y < lowestActiveGround.position.y)
                {
                    lowestActiveGround = child;
                }
            }
        }

        if (lowestActiveGround != null)
        {
            Vector3 pos = lowestActiveGround.position;
            pos.z = this.transform.position.z;
            pos.y += playerHeight / 2;
            this.transform.position = pos;
        }

        _state = PlayerState.PORTAL;
        _anim.UpdateAnimation(_state);
        yield return new WaitForSeconds(2f); // Chờ 2 giây ở trạng thái PORTAL
        isPaused = false; // Tiếp tục di chuyển
        _state = PlayerState.IDLE; // Trở lại trạng thái IDLE sau khi kết thúc
        _hasDied = false; // Đặt lại trạng thái đã chết
    }

    private void Move()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");
        if (moveDirection != 0)
        {
            _state = PlayerState.RUN;
            Vector2 movement = _rigi.velocity;
            movement.x = moveDirection * Speed;

            // Kiểm tra nếu di chuyển ra khỏi lề trái hoặc phải của camera
            if ((transform.position.x - playerWidth / 2 < cameraLeft && moveDirection < 0) || (transform.position.x + playerWidth / 2 > cameraRight && moveDirection > 0))
            {
                movement.x = 0; // Dừng di chuyển
            }

            _rigi.velocity = movement;
        }
        else
        {
            if (!isPaused) _state = PlayerState.IDLE;
        }
    }

    private void HandleJump()
    {
        CheckCollisionWithGround();
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_isGrounded)
            {
                _rigi.AddForce(ForceToJump);
            }
        }
    }

    private void HandleShoot()
    {
        if (Input.GetKey(KeyCode.J))
        {
            _state = PlayerState.SHOOT;
            if (_countToShoot > -0.1f && _countToShoot < 0.1f)
            {
                AudioManager.Instance.PlayGunSound();
                _countToShoot = _shootSpeed;

                GameObject bullet = ObjectPooling.Instance.GetObject(bulletPrefab.gameObject, bulletList.gameObject);

                Vector3 pos = this.transform.position;
                pos.y += playerHeight / 2;
                float speedBullet = bullet.GetComponent<BulletController>().Speed;
                if (this.transform.GetChild(0).transform.localScale.x * speedBullet > 0)
                {
                    if (speedBullet > 0) pos.x += playerWidth / 2;
                    else pos.x -= playerWidth / 2;
                }
                else
                {
                    speedBullet *= -1;
                    if (speedBullet > 0) pos.x += playerWidth / 2;
                    else pos.x -= playerWidth / 2;
                    bullet.GetComponent<BulletController>().Speed = speedBullet;
                    Vector3 dirBullet = bullet.transform.localScale;
                    dirBullet.x *= -1;
                    bullet.transform.localScale = dirBullet;
                }
                bullet.transform.position = pos;

                bullet.SetActive(true);
            }
            
        }
    }

    private void CheckCanShoot()
    {
        if (_countToShoot > 0)
        {
            _countToShoot -= 0.1f;
        }
    }

    private void CheckCollisionWithGround()
    {
        Vector2 origin = this.transform.position;

        RaycastHit2D hitDown = Physics2D.Raycast(origin, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin, Vector2.left, rayLength, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(origin, Vector2.right, rayLength, groundLayer);

        if (hitDown.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            Debug.Log("Ground detected");
            _isGrounded = true;
            if (Input.GetAxisRaw("Horizontal") == 0 && !isPaused) _state = PlayerState.IDLE;
        }
        else
        {
            if (!isPaused) _state = PlayerState.JUMP;
            _isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            _isCollisionEnemy = true;
        } else
        {
            _isCollisionEnemy = false;
        }
        
    }


    public enum PlayerState
    {
        IDLE,
        RUN,
        SHOOT,
        DEATH,
        PORTAL,
        JUMP,
    }

}
