using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermiController : MonoBehaviour
{
    public float mermiHizi;

    PlayerHealthController playerHealthController;

    private void Awake()
    {
        playerHealthController = Object.FindObjectOfType<PlayerHealthController>();
    }

    private void Update()
    {
        transform.position += new Vector3(-mermiHizi * transform.localScale.x * Time.deltaTime, 0f, 0f);    //merminin scale yönüne göre hareket etmesi için
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealthController.TanktanHasarAl();            //mermi çarpýtðýnda healthcontroller scriptindeki tanktanHasarAl methodu çalýþýyor. 
        }

        Destroy(gameObject);  //merminin çarptýktan sonra yok olmasý için
    }

}
