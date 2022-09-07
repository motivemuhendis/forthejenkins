using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasarController : MonoBehaviour
{

    PlayerHealthController playerHealthController;
    private void Awake()
    {
        playerHealthController = Object.FindObjectOfType<PlayerHealthController>(); // farklý bir script olan PlayerHealtController'a eriþebilmek için.
    }
    private void OnTriggerEnter2D(Collider2D other)     //Objeye deðdiðinde
    {
        if (other.tag == "Player")                  // tagi Player olan nesne ise
        {
            playerHealthController.HasarAl();       //hasar almasýný saðla. 
            SesController.instance.SesEfektiCikar(1);
        }
    }
}
