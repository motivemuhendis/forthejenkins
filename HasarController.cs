using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasarController : MonoBehaviour
{

    PlayerHealthController playerHealthController;
    private void Awake()
    {
        playerHealthController = Object.FindObjectOfType<PlayerHealthController>(); // farkl� bir script olan PlayerHealtController'a eri�ebilmek i�in.
    }
    private void OnTriggerEnter2D(Collider2D other)     //Objeye de�di�inde
    {
        if (other.tag == "Player")                  // tagi Player olan nesne ise
        {
            playerHealthController.HasarAl();       //hasar almas�n� sa�la. 
            SesController.instance.SesEfektiCikar(1);
        }
    }
}
