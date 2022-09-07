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
    private void OnTriggerEnter2D(Collider2D collision)             //mayina oyuncu de�di�inde bir efeckt ile may�nlar yok oluyor ve player hasar al�yor. 
    {
        if (collision.CompareTag("Player"))
        {
            PatlamaFNC();
            playerHealthController.HasarAl();
        }
    }

    public void PatlamaFNC()
    {
        Destroy(gameObject);                                                    //Mayinlarin bir efekt ile patlamas� i�in
        Instantiate(patlamaEfekti, transform.position, transform.rotation);
    }



}
