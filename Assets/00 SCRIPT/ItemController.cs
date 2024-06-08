using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            this.gameObject.SetActive(false);
            if (GameManager.Instance.Player.GetComponent<PlayerController>().Health < CONSTANT.MAX_HEALTH)
            {
                GameManager.Instance.Player.GetComponent<PlayerController>().Health += 1;
                AudioManager.Instance.PlayGetSound();
            }

        }
    }
}
