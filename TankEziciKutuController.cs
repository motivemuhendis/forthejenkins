using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEziciKutuController : MonoBehaviour
{
    PlayerController playerController;
    TankController tankController;

    private void Awake()
    {
        playerController = Object.FindObjectOfType<PlayerController>();
        tankController = Object.FindObjectOfType<TankController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerController.transform.position.y>transform.position.y)     //tankýn üstündeki ezici kutuya deðidðinde ve y olarak player
                                                                                                          //daha büyük olduðunda yani tam üstünden deðdiðinde çalýþmasý için
        {
            playerController.ustundeZiplaFNC();                     //y yönünde bir itme kuvvetini çaðýrma
            tankController.DarbeAlFNC();                            //Tankýn DarbeAlFNC() çaðrýlmasý
            SesController.instance.SesEfektiCikar(9);

            gameObject.SetActive(false);                            //ezici kutunun tekrar ezilmemesi için false yapýlmasý
        }
    }
}
