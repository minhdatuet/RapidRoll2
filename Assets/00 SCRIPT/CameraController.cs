using System.Collections;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    float initialScrollSpeed = 0.02f; // Tốc độ di chuyển ban đầu của camera
    float maxScrollSpeed = 0.06f; // Tốc độ di chuyển tối đa của camera
    int initialSpawnSpeed = 120; // Tốc độ sinh đối tượng ban đầu
    [SerializeField] private float scrollSpeed;
    [SerializeField] private int spawnSpeed;
    public int SpawnSpeed
    {
        get
        {
            return spawnSpeed;
        }
    }
    private bool isScrolling = false; // Biến kiểm soát việc di chuyển của camera
    public bool IsScrolling
    {
        get { return isScrolling; }
        set { isScrolling = value; }
    }
    private float elapsedTime = 0f; // Thời gian đã trôi qua
    private float duration = 120f; // 2 phút
    private float screenHeight;

    void Start()
    {
        scrollSpeed = initialScrollSpeed;
        spawnSpeed = initialSpawnSpeed;
        screenHeight = 2 * Camera.main.orthographicSize; // Chiều cao của màn hình
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isScrolling)
        {
            transform.position += Vector3.down * scrollSpeed;
            elapsedTime += Time.fixedDeltaTime;

            // Điều chỉnh tốc độ sinh đối tượng
            float scrollFraction = (scrollSpeed - initialScrollSpeed) / (maxScrollSpeed - initialScrollSpeed);
            spawnSpeed = Mathf.RoundToInt(initialSpawnSpeed / (1 + 2 * scrollFraction)); // Điều chỉnh tốc độ sinh đối tượng dựa trên tốc độ di chuyển hiện tại
        }
    }

    private IEnumerator StopScrollingForSeconds(float seconds)
    {
        isScrolling = false;
        yield return new WaitForSeconds(seconds);
        isScrolling = true;
    }

    public void StopScrolling(float seconds)
    {
        StartCoroutine(StopScrollingForSeconds(seconds));
    }

    private IEnumerator UpdateScrollSpeed()
    {
        while (elapsedTime < duration)
        {
            scrollSpeed = Mathf.Lerp(initialScrollSpeed, maxScrollSpeed, elapsedTime / duration);
            yield return null;
        }
        scrollSpeed = maxScrollSpeed;
    }

    public void StartScrolling()
    {
        isScrolling = true;
        StartCoroutine(UpdateScrollSpeed());
    }
}
