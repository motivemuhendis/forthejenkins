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
        {                                           //tagi kurbaga olan nesneye deðdiðinde

            other.transform.parent.gameObject.SetActive(false);                        //nesnenin parentininin görünürlüðünü false yap
            Instantiate(yokOlmaEfecti, transform.position, transform.rotation);         //yok olmak efekti oluþtur.

            playerController.ustundeZiplaFNC();                         //kurbaga bastýðýmýzda ekstra bir zýplama saðlar.

            float cikmaAraligi = Random.Range(0f, 100f);                     //kurbagayý ezdiðimizde belli bir oranda þans ile kiraz çýkmasýný oluþturduk

            SesController.instance.SesEfektiCikar(0);       //statik olarak tanýmlamýþ olduðumuz ses scriptine bu instance ile ulaþýyoruz ve istediðimiz indisi oynatýyoruz. 

            if (cikmaAraligi < cikmaSansi)
            {
                Instantiate(kirazObjesi, other.transform.position, other.transform.rotation);
            }
        }
        else if (other.CompareTag("Kartal"))
        {

            other.transform.parent.gameObject.SetActive(false);                        //nesnenin parentininin görünürlüðünü false yap
            Instantiate(yokOlmaEfecti, transform.position, transform.rotation);         //yok olmak efekti oluþtur.

            playerController.ustundeZiplaFNC();                         //kartala bastýðýmýzda ekstra bir zýplama saðlar.

            float cikmaAraligi = Random.Range(0f, 100f);                     //kartalý ezdiðimizde belli bir oranda þans ile kiraz çýkmasýný oluþturduk

            SesController.instance.SesEfektiCikar(0);       //statik olarak tanýmlamýþ olduðumuz ses scriptine bu instance ile ulaþýyoruz ve istediðimiz indisi oynatýyoruz. 

            if (cikmaAraligi < cikmaSansi)
            {
                Instantiate(kirazObjesi, other.transform.position, other.transform.rotation);
            }

        }
    }
}
