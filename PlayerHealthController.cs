using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealthController : MonoBehaviour
{
    public int maxSaglik, gecerliSaglik;

    UIController uIController;

    public float yenilmezlikSuresi;
    float yenilmezlikSayaci;

    SpriteRenderer sr;

    PlayerController playerController;

    [SerializeField]
    GameObject yokolmaEfekti;

    private void Awake()
    {
        playerController = Object.FindObjectOfType<PlayerController>();
        uIController = Object.FindObjectOfType<UIController>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        gecerliSaglik = maxSaglik;             //max saglik ile baþlýyoruz. 
    }

    private void Update()
    {
        yenilmezlikSayaci -= Time.deltaTime;        //yenilmezlik sayacýný azaltmak için
        if (yenilmezlikSayaci <= 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);   //yenilmezliSayaci 0 dan büyükse alpha deðerini tekrar 1 yap.
        }
    }
    public void HasarAl()
    {
        if (yenilmezlikSayaci <= 0)                 //karakterin belli bir süre hasar almasýný engellemek için
        {

            gecerliSaglik--;                        //geçerli saglik deðerini bir azaltý

            if (gecerliSaglik <= 0)
            {
                gecerliSaglik = 0;

                gameObject.SetActive(false);                                              //saglik 0dan küçük old. karakter gözükmez
                Instantiate(yokolmaEfekti, transform.position, transform.rotation);       //karakterimiz öldüðünde yok olma efekti oluþturur.
                SesController.instance.SesEfektiCikar(2);                                  //ölme sesi                                       
                SceneManager.LoadScene("GameOver");


            }
            else
            {
                yenilmezlikSayaci = yenilmezlikSuresi;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); //karakter dikene çarptýðýnda alpfha deðerinin azalmasý
                playerController.geriTepmeFNC(); //karakter dikene çarptýðýnda bir geri tepme etkisi gösterir. 
                SesController.instance.SesEfektiCikar(1);       //darbe alma sesi
            }

            uIController.SaglikDurumunuGuncelle();
        }
    }

    public void TanktanHasarAl()
    {
        if (yenilmezlikSayaci <= 0)                 //karakterin belli bir süre hasar almasýný engellemek için
        {

            gecerliSaglik=gecerliSaglik-2;                        //geçerli saglik deðerini iki azaltý

            if (gecerliSaglik <= 0)
            {
                gecerliSaglik = 0;
                gameObject.SetActive(false);                                              //saglik 0dan küçük old. karakter gözükmez
                Instantiate(yokolmaEfekti, transform.position, transform.rotation);       //karakterimiz öldüðünde yok olma efekti oluþturur.
                SesController.instance.SesEfektiCikar(2);                                  //ölme sesi                                       
                SceneManager.LoadScene("GameOver");


            }
            else
            {
                yenilmezlikSayaci = yenilmezlikSuresi;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); //karakter dikene çarptýðýnda alpfha deðerinin azalmasý
                playerController.geriTepmeFNC(); //karakter dikene çarptýðýnda bir geri tepme etkisi gösterir. 
            }

            uIController.SaglikDurumunuGuncelle();
        }
    }

    public void CaniArttirFNC()                     //Kiraz vs topladýðýnda çaðýracaðýmýz fonksiyon
    {
        gecerliSaglik++;

        if (gecerliSaglik >= maxSaglik)             //geçerli saðlýðýn altýyý geçmesini istemiyoruz.
        {
            gecerliSaglik = maxSaglik; 
        }

        uIController.SaglikDurumunuGuncelle();
    }


   
       
   

}
