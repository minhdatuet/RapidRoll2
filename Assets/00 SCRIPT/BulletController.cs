using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody2D _rigi;
    [SerializeField] float speed = 1000.0f;
    Animator _animator;
    private Camera mainCamera;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _rigi = GetComponent<Rigidbody2D>();
        _rigi.AddForce(Vector2.right * speed);
        _animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckOutOfBounds();
    }

    private void CheckOutOfBounds()
    {
        Vector3 screenPos = mainCamera.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {

            StartCoroutine(DestroyEnemy(collision.gameObject));
            _rigi.velocity = Vector2.zero;
            _animator.SetBool("isHit", true);
            StartCoroutine(DestroyBullet());
            UIManager.Instance.ShowAddScoreText();
            AudioManager.Instance.PlayExpSound();
            GameManager.Instance.Score += 500;
        }
        if (collision.gameObject.tag.Equals("Ground"))
        {
            _rigi.velocity = Vector2.zero;
            _animator.SetBool("isHit", true);
            StartCoroutine(DestroyBullet());
        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator DestroyEnemy(GameObject enemy)
    {
        enemy.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        enemy.gameObject.GetComponent<EnemyController>().IsDeath = true;
        enemy.gameObject.GetComponent<Animator>().SetBool("isDeath", true);
        yield return new WaitForSeconds(1f);
        enemy.gameObject.SetActive(false);
    }
}
