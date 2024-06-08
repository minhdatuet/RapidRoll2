using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    
    private Camera mainCamera;
    

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        CheckIfBelowCamera();
    }


    private void CheckIfBelowCamera()
    {
        if (mainCamera != null)
        {
            // Lấy tọa độ y trên cùng của camera
            float cameraTop = mainCamera.transform.position.y + mainCamera.orthographicSize;
            // Nếu vị trí y của đối tượng (mặt đất) nhỏ hơn vị trí đáy của camera
            if (transform.position.y > cameraTop - 0.5)
            {
                //Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
        }
    }

    
}
