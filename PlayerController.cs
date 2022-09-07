using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    [SerializeField]
    float hareketHizi;                      //D��ar�dan hareket h�z�m�z� belirliyoruz.

    [SerializeField]
    float ziplamaGucu;                      //D��ar�dan ziplamagucumuzu belirliyoruz.

    bool yerdemi;                               //I��n g�ndermek i�in gerekli
    public Transform zeminKontrolNoktasi;       //I��n g�ndermek i�in gerekli
    public LayerMask zeminLayer;                //I��n g�ndermek i�in gerekli

    bool ikiKezZiplayabilirmi;


    public float geriTepmeSuresi, geriTepmeGucu;
    float geriTepmeSayaci;
    bool yonSagmi;

    public float ustundeZiplaGucu;
    public float bouncerZiplaGucu;

    public bool hareketEtsinmi;         //b�l�m sonunda hareket etmeyi engellemek i�in

    private void Start()
    {
        hareketEtsinmi = true;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   //Awake i�erisinde direkt olarak //at�ld��� nesnenin RB'sine eri�meyi sa�lar
        anim = GetComponent<Animator>();    //Animasyonlar� de�i�tirebilmek i�in gerekli komponent
    }

    private void Update()
    {
        if (hareketEtsinmi)
        {
            if (geriTepmeSayaci <= 0)
            {
                HareketEttirFNC();                  //Update i�erisinde s�rekli olarak hareketin tetiklenmesi
                ZiplaFNC();                         //Update i�erisinde s�rekli olarak hareketin tetiklenmesi
                YonuDegistir();                     //tetikleme
            }
            else
            {
                geriTepmeSayaci -= Time.deltaTime;  //aksi halde geri tepme g�c� 0.25 oldu�u i�in hareket fonk. vs �al�mayacakt�r. 

                if (yonSagmi)                       //yon sag ise - y�nde bir geri tepki, sol ise + yonde bir geri tepki olusturur.
                {
                    rb.velocity = new Vector2(-geriTepmeGucu, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(geriTepmeGucu, rb.velocity.y);
                }
            }

            anim.SetFloat("hareketHizi", Mathf.Abs(rb.velocity.x)); //animasyonlar�m�z� tetikleyecek kod sat�rlar�
            anim.SetBool("yerdemi", yerdemi);                       //animasyonlar� tetiklemek i�in
        } else
        {
            rb.velocity = Vector2.zero;                                 //hareketEtsinmi false oldu�unda durmas� i�in 
            anim.SetFloat("hareketHizi", Mathf.Abs(rb.velocity.x));     //durma animasyonunu oynat�yoruz.
        }
    }

    void HareketEttirFNC()
    {
        float h = Input.GetAxis("Horizontal");              //Yatay eksende hareket etmek i�in  
        float hiz = h * hareketHizi;                        //hizimizi ayarlard�k
        rb.velocity = new Vector2(hiz, rb.velocity.y);      //velocity ile bu hizimizi nesnemizin rbsine ba�lad�k.
    }


    void ZiplaFNC()
    {
        yerdemi = Physics2D.OverlapCircle(zeminKontrolNoktasi.position, .2f, zeminLayer); //ZeminKontrol noktasindan .2f yar��apl� ���n g�nderir, ZeminLayer maskesi ile

        if (yerdemi)                        
        {
            ikiKezZiplayabilirmi = true;            //Karakter yerde ise iki kez ziplayabilir.
        }

        if (Input.GetButtonDown("Jump"))                    //Jump tu�una bas�l� tutuldu�u zaman Jump yerine Keycode.space ile ayn�
        {
            if (yerdemi)                                   //Karakter yerde ise z�plama ger�ekle�tirir.
            {
                rb.velocity = new Vector2(rb.velocity.x, ziplamaGucu);      //Y y�n�nde vermi� oldu�umuz g�c boyutunda ziplama ger�ekle�tirir. 
                SesController.instance.SesEfektiCikar(3);       //ziplama sesi
            }
            else
            {
                if (ikiKezZiplayabilirmi)                                   //ikikezZiplayabilirmi true ise tekrar z�plar sonra false d�ner.
                {
                    rb.velocity = new Vector2(rb.velocity.x, ziplamaGucu);
                    ikiKezZiplayabilirmi = false;
                    SesController.instance.SesEfektiCikar(3);       //ziplama sesi
                }
            }
        }

       
    }

    void YonuDegistir()
    {
        Vector2 geciciScale = transform.localScale;         //transform.localScale= -1f; gibi bir tan�mlama unityde olmuyor
                                                            //bu nedenle gecici bir vector olu�turup bunun atamas�n� yap�yoruz.
        if (rb.velocity.x > 0)                 //x de�eri positifse sa�a bak�yor y�nde 1f pozitif olur.            
        {
            yonSagmi = true;
            geciciScale.x=1f;
        } else if (rb.velocity.x < 0)           //x de�eri negatifse sola bakmal� y�nde -1f olmal�d�r. 
        {
            yonSagmi = false;
            geciciScale.x = -1f;
        }

        transform.localScale = geciciScale;         //gecici vectorden tekrar normal scale atama yapt�k

    }

    public void geriTepmeFNC()
    {
        geriTepmeSayaci = geriTepmeSuresi;
        rb.velocity = new Vector2(0, rb.velocity.y);  //karakter �arpt��� zaman h�z�n� 0 yap�yoruz.
        anim.SetTrigger("hasar");                       //karakter hasar ald���g�nda olu�ak animasyonu tetikledik.
    }

    public void ustundeZiplaFNC()
    {
        rb.velocity = new Vector2(rb.velocity.x, ustundeZiplaGucu);         //D��mana bast���nda y y�n�nde bir h�z ile ekstra ziplamay� saglar;
        SesController.instance.SesEfektiCikar(3);       //ziplama sesi
    }

    public void bouncerZiplamaFNC()
    {
        rb.velocity = new Vector2(rb.velocity.x, bouncerZiplaGucu);
    }

}
