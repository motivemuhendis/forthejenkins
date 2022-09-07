using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayinController : MonoBehaviour
{

    public GameObject patlamaEfekti;

    PlayerHealthController playerHealthController;

    private void Awake()
    {
        playerHealthController = Object.FindObjectOfType<PlayerHealthController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)             //mayina oyuncu deðdiðinde bir efeckt ile mayýnlar yok oluyor ve player hasar alýyor. 
    {
        if (collision.CompareTag("Player"))
        {
            PatlamaFNC();
            playerHealthController.HasarAl();
        }
    }

    public void PatlamaFNC()
    {
        Destroy(gameObject);                                                    //Mayinlarin bir efekt ile patlamasý için
        Instantiate(patlamaEfekti, transform.position, transform.rotation);
    }



}
