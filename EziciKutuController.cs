using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EziciKutuController : MonoBehaviour
{
    [SerializeField]
    GameObject yokOlmaEfecti;

    PlayerController playerController;

    public float cikmaSansi;
    public GameObject kirazObjesi;

    private void Awake()
    {
        playerController = Object.FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kurbaga"))
        {                                           //tagi kurbaga olan nesneye de�di�inde

            other.transform.parent.gameObject.SetActive(false);                        //nesnenin parentininin g�r�n�rl���n� false yap
            Instantiate(yokOlmaEfecti, transform.position, transform.rotation);         //yok olmak efekti olu�tur.

            playerController.ustundeZiplaFNC();                         //kurbaga bast���m�zda ekstra bir z�plama sa�lar.

            float cikmaAraligi = Random.Range(0f, 100f);                     //kurbagay� ezdi�imizde belli bir oranda �ans ile kiraz ��kmas�n� olu�turduk

            SesController.instance.SesEfektiCikar(0);       //statik olarak tan�mlam�� oldu�umuz ses scriptine bu instance ile ula��yoruz ve istedi�imiz indisi oynat�yoruz. 

            if (cikmaAraligi < cikmaSansi)
            {
                Instantiate(kirazObjesi, other.transform.position, other.transform.rotation);
            }
        }
        else if (other.CompareTag("Kartal"))
        {

            other.transform.parent.gameObject.SetActive(false);                        //nesnenin parentininin g�r�n�rl���n� false yap
            Instantiate(yokOlmaEfecti, transform.position, transform.rotation);         //yok olmak efekti olu�tur.

            playerController.ustundeZiplaFNC();                         //kartala bast���m�zda ekstra bir z�plama sa�lar.

            float cikmaAraligi = Random.Range(0f, 100f);                     //kartal� ezdi�imizde belli bir oranda �ans ile kiraz ��kmas�n� olu�turduk

            SesController.instance.SesEfektiCikar(0);       //statik olarak tan�mlam�� oldu�umuz ses scriptine bu instance ile ula��yoruz ve istedi�imiz indisi oynat�yoruz. 

            if (cikmaAraligi < cikmaSansi)
            {
                Instantiate(kirazObjesi, other.transform.position, other.transform.rotation);
            }

        }
    }
}
