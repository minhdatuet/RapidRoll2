using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] GameObject player;
    public GameObject Player => player;

    public GameObject groundPrefab, firePrefab, heartPrefab, enemyPrefab; // Khởi tạo Prefab
    private int fixedUpdateCounter = 0; // Biến đếm số lần FixedUpdate
    private Camera mainCamera;
    [SerializeField] int SpawnSpeed = 120; //Sau SpawnSpeed lần FixedUpdate sẽ tạo Ground 1 lần
    private bool isScrolling = true; // Biến kiểm soát việc di chuyển của camera
    [SerializeField] int _score = 0;
    private float groundSize;
    public int Score
    {
        get { return _score; }

        set { _score = value; }
    }


    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        groundSize = groundPrefab.transform.GetComponent<Renderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        isScrolling = mainCamera.GetComponent<CameraController>().IsScrolling;
        SpawnSpeed = mainCamera.GetComponent<CameraController>().SpawnSpeed;
    }

    private void FixedUpdate()
    {
        if (isScrolling)
        {
            fixedUpdateCounter++;
            _score++;
            if (fixedUpdateCounter >= SpawnSpeed)
            {
                SpawnGroundPrefab();
                fixedUpdateCounter = 0; // Reset biến đếm
            }
        }
    }

    private void SpawnGroundPrefab()
    {
        Debug.Log(Score);
        if (mainCamera != null)
        {
            // Lấy chiều ngang của camera
            float cameraWidth = mainCamera.orthographicSize * mainCamera.aspect;

            // Lấy ra GroundList, FireList
            GameObject groundList = GameObject.Find("GroundList");
            GameObject fireList = GameObject.Find("FireList");
            GameObject itemList = GameObject.Find("ItemList");
            GameObject enemyList = GameObject.Find("EnemyList");

            // Lấy chiều ngang của Prefab
            Renderer renderer = groundPrefab.GetComponent<Renderer>();
            float groundWidth = renderer.bounds.size.x;

            // Xác định vị trí x ngẫu nhiên trong khoảng chiều ngang của camera
            float randomX1 = Random.Range(-cameraWidth / 2 - groundWidth / 2, -groundWidth / 2);
            float randomX2 = Random.Range(groundWidth / 2, cameraWidth / 2 + groundWidth / 2);

            // Xác định vị trí y
            float spawnY = mainCamera.transform.position.y - mainCamera.orthographicSize;

            // Tạo mới Prefab
            if (randomX2 - randomX1 < 2.5 * groundWidth)
            {
                float randomToFire = Random.Range(0f, 1f);
                Debug.Log(randomToFire);
                Vector3 spawnPosition = new Vector3(0, spawnY + 0.5f, 0);
                if (randomToFire < 0.1)
                {
                    GameObject fire = ObjectPooling.Instance.GetObject(firePrefab.gameObject, fireList.gameObject);
                    fire.transform.position = spawnPosition;
                    fire.SetActive(true);
                }
                else
                {
                    GameObject ground = ObjectPooling.Instance.GetObject(groundPrefab.gameObject, groundList.gameObject);
                    ground.transform.position = spawnPosition;
                    ground.SetActive(true);
                    if (randomToFire < 0.15)
                    {
                        GameObject heart = ObjectPooling.Instance.GetObject(heartPrefab.gameObject, itemList.gameObject);
                        spawnPosition.y += groundSize;
                        heart.transform.position = spawnPosition;
                        heart.SetActive(true);
                    } else if (randomToFire <0.25)
                    {
                        GameObject enemy = ObjectPooling.Instance.GetObject(enemyPrefab.gameObject, enemyList.gameObject);
                        spawnPosition.y += 2*groundSize;
                        enemy.transform.position = spawnPosition;
                        enemy.SetActive(true);
                    }

                }


            }
            else
            {
                float randomToFire1 = Random.Range(0f, 1f);
                Debug.Log(randomToFire1);
                Vector3 spawnPosition1 = new Vector3(randomX1, spawnY + 0.5f, 0);
                if (randomToFire1 < 0.1)
                {
                    GameObject fire = ObjectPooling.Instance.GetObject(firePrefab.gameObject, fireList.gameObject);
                    fire.transform.position = spawnPosition1;
                    fire.SetActive(true);
                }
                else
                {
                    GameObject ground = ObjectPooling.Instance.GetObject(groundPrefab.gameObject, groundList.gameObject);
                    ground.transform.position = spawnPosition1;
                    ground.SetActive(true);
                    if (randomToFire1 < 0.15)
                    {
                        GameObject heart = ObjectPooling.Instance.GetObject(heartPrefab.gameObject, itemList.gameObject);
                        spawnPosition1.y += groundSize;
                        heart.transform.position = spawnPosition1;
                        heart.SetActive(true);
                    }
                    else if (randomToFire1 < 0.25)
                    {
                        GameObject enemy = ObjectPooling.Instance.GetObject(enemyPrefab.gameObject, enemyList.gameObject);
                        spawnPosition1.y += 2*groundSize;
                        enemy.transform.position = spawnPosition1;
                        enemy.SetActive(true);
                    }
                }

                float randomToFire2 = Random.Range(0f, 1f);
                Debug.Log(randomToFire2);
                Vector3 spawnPosition2 = new Vector3(randomX2, spawnY + 0.5f, 0);
                if (randomToFire2 < 0.1)
                {
                    GameObject fire = ObjectPooling.Instance.GetObject(firePrefab.gameObject, fireList.gameObject);
                    fire.transform.position = spawnPosition2;
                    fire.SetActive(true);
                }
                else
                {
                    GameObject ground = ObjectPooling.Instance.GetObject(groundPrefab.gameObject, groundList.gameObject);
                    ground.transform.position = spawnPosition2;
                    ground.SetActive(true);
                    if (randomToFire2 < 0.15)
                    {
                        GameObject heart = ObjectPooling.Instance.GetObject(heartPrefab.gameObject, itemList.gameObject);
                        spawnPosition2.y += groundSize;
                        heart.transform.position = spawnPosition2;
                        heart.SetActive(true);
                    }
                    else if (randomToFire2 < 0.25)
                    {
                        GameObject enemy = ObjectPooling.Instance.GetObject(enemyPrefab.gameObject, enemyList.gameObject);
                        spawnPosition2.y += 2 * groundSize;
                        enemy.transform.position = spawnPosition2;
                        enemy.SetActive(true);
                    }
                }
            }
        }
    }

}
