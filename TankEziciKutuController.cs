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
        if (other.CompareTag("Player") && playerController.transform.position.y>transform.position.y)     //tank�n �st�ndeki ezici kutuya de�id�inde ve y olarak player
                                                                                                          //daha b�y�k oldu�unda yani tam �st�nden de�di�inde �al��mas� i�in
        {
            playerController.ustundeZiplaFNC();                     //y y�n�nde bir itme kuvvetini �a��rma
            tankController.DarbeAlFNC();                            //Tank�n DarbeAlFNC() �a�r�lmas�
            SesController.instance.SesEfektiCikar(9);

            gameObject.SetActive(false);                            //ezici kutunun tekrar ezilmemesi i�in false yap�lmas�
        }
    }
}
