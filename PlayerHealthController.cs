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
        gecerliSaglik = maxSaglik;             //max saglik ile ba�l�yoruz. 
    }

    private void Update()
    {
        yenilmezlikSayaci -= Time.deltaTime;        //yenilmezlik sayac�n� azaltmak i�in
        if (yenilmezlikSayaci <= 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);   //yenilmezliSayaci 0 dan b�y�kse alpha de�erini tekrar 1 yap.
        }
    }
    public void HasarAl()
    {
        if (yenilmezlikSayaci <= 0)                 //karakterin belli bir s�re hasar almas�n� engellemek i�in
        {

            gecerliSaglik--;                        //ge�erli saglik de�erini bir azalt�

            if (gecerliSaglik <= 0)
            {
                gecerliSaglik = 0;

                gameObject.SetActive(false);                                              //saglik 0dan k���k old. karakter g�z�kmez
                Instantiate(yokolmaEfekti, transform.position, transform.rotation);       //karakterimiz �ld���nde yok olma efekti olu�turur.
                SesController.instance.SesEfektiCikar(2);                                  //�lme sesi                                       
                SceneManager.LoadScene("GameOver");


            }
            else
            {
                yenilmezlikSayaci = yenilmezlikSuresi;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); //karakter dikene �arpt���nda alpfha de�erinin azalmas�
                playerController.geriTepmeFNC(); //karakter dikene �arpt���nda bir geri tepme etkisi g�sterir. 
                SesController.instance.SesEfektiCikar(1);       //darbe alma sesi
            }

            uIController.SaglikDurumunuGuncelle();
        }
    }

    public void TanktanHasarAl()
    {
        if (yenilmezlikSayaci <= 0)                 //karakterin belli bir s�re hasar almas�n� engellemek i�in
        {

            gecerliSaglik=gecerliSaglik-2;                        //ge�erli saglik de�erini iki azalt�

            if (gecerliSaglik <= 0)
            {
                gecerliSaglik = 0;
                gameObject.SetActive(false);                                              //saglik 0dan k���k old. karakter g�z�kmez
                Instantiate(yokolmaEfekti, transform.position, transform.rotation);       //karakterimiz �ld���nde yok olma efekti olu�turur.
                SesController.instance.SesEfektiCikar(2);                                  //�lme sesi                                       
                SceneManager.LoadScene("GameOver");


            }
            else
            {
                yenilmezlikSayaci = yenilmezlikSuresi;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); //karakter dikene �arpt���nda alpfha de�erinin azalmas�
                playerController.geriTepmeFNC(); //karakter dikene �arpt���nda bir geri tepme etkisi g�sterir. 
            }

            uIController.SaglikDurumunuGuncelle();
        }
    }

    public void CaniArttirFNC()                     //Kiraz vs toplad���nda �a��raca��m�z fonksiyon
    {
        gecerliSaglik++;

        if (gecerliSaglik >= maxSaglik)             //ge�erli sa�l���n alt�y� ge�mesini istemiyoruz.
        {
            gecerliSaglik = maxSaglik; 
        }

        uIController.SaglikDurumunuGuncelle();
    }


   
       
   

}
