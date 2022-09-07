using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    [SerializeField]
    float hareketHizi;                      //Dýþarýdan hareket hýzýmýzý belirliyoruz.

    [SerializeField]
    float ziplamaGucu;                      //Dýþarýdan ziplamagucumuzu belirliyoruz.

    bool yerdemi;                               //Iþýn göndermek için gerekli
    public Transform zeminKontrolNoktasi;       //Iþýn göndermek için gerekli
    public LayerMask zeminLayer;                //Iþýn göndermek için gerekli

    bool ikiKezZiplayabilirmi;


    public float geriTepmeSuresi, geriTepmeGucu;
    float geriTepmeSayaci;
    bool yonSagmi;

    public float ustundeZiplaGucu;
    public float bouncerZiplaGucu;

    public bool hareketEtsinmi;         //bölüm sonunda hareket etmeyi engellemek için

    private void Start()
    {
        hareketEtsinmi = true;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   //Awake içerisinde direkt olarak //atýldýðý nesnenin RB'sine eriþmeyi saðlar
        anim = GetComponent<Animator>();    //Animasyonlarý deðiþtirebilmek için gerekli komponent
    }

    private void Update()
    {
        if (hareketEtsinmi)
        {
            if (geriTepmeSayaci <= 0)
            {
                HareketEttirFNC();                  //Update içerisinde sürekli olarak hareketin tetiklenmesi
                ZiplaFNC();                         //Update içerisinde sürekli olarak hareketin tetiklenmesi
                YonuDegistir();                     //tetikleme
            }
            else
            {
                geriTepmeSayaci -= Time.deltaTime;  //aksi halde geri tepme gücü 0.25 olduðu için hareket fonk. vs çalþmayacaktýr. 

                if (yonSagmi)                       //yon sag ise - yönde bir geri tepki, sol ise + yonde bir geri tepki olusturur.
                {
                    rb.velocity = new Vector2(-geriTepmeGucu, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(geriTepmeGucu, rb.velocity.y);
                }
            }

            anim.SetFloat("hareketHizi", Mathf.Abs(rb.velocity.x)); //animasyonlarýmýzý tetikleyecek kod satýrlarý
            anim.SetBool("yerdemi", yerdemi);                       //animasyonlarý tetiklemek için
        } else
        {
            rb.velocity = Vector2.zero;                                 //hareketEtsinmi false olduðunda durmasý için 
            anim.SetFloat("hareketHizi", Mathf.Abs(rb.velocity.x));     //durma animasyonunu oynatýyoruz.
        }
    }

    void HareketEttirFNC()
    {
        float h = Input.GetAxis("Horizontal");              //Yatay eksende hareket etmek için  
        float hiz = h * hareketHizi;                        //hizimizi ayarlardýk
        rb.velocity = new Vector2(hiz, rb.velocity.y);      //velocity ile bu hizimizi nesnemizin rbsine baðladýk.
    }


    void ZiplaFNC()
    {
        yerdemi = Physics2D.OverlapCircle(zeminKontrolNoktasi.position, .2f, zeminLayer); //ZeminKontrol noktasindan .2f yarýçaplý ýþýn gönderir, ZeminLayer maskesi ile

        if (yerdemi)                        
        {
            ikiKezZiplayabilirmi = true;            //Karakter yerde ise iki kez ziplayabilir.
        }

        if (Input.GetButtonDown("Jump"))                    //Jump tuþuna basýlý tutulduðu zaman Jump yerine Keycode.space ile ayný
        {
            if (yerdemi)                                   //Karakter yerde ise zýplama gerçekleþtirir.
            {
                rb.velocity = new Vector2(rb.velocity.x, ziplamaGucu);      //Y yönünde vermiþ olduðumuz güc boyutunda ziplama gerçekleþtirir. 
                SesController.instance.SesEfektiCikar(3);       //ziplama sesi
            }
            else
            {
                if (ikiKezZiplayabilirmi)                                   //ikikezZiplayabilirmi true ise tekrar zýplar sonra false döner.
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
        Vector2 geciciScale = transform.localScale;         //transform.localScale= -1f; gibi bir tanýmlama unityde olmuyor
                                                            //bu nedenle gecici bir vector oluþturup bunun atamasýný yapýyoruz.
        if (rb.velocity.x > 0)                 //x deðeri positifse saða bakýyor yönde 1f pozitif olur.            
        {
            yonSagmi = true;
            geciciScale.x=1f;
        } else if (rb.velocity.x < 0)           //x deðeri negatifse sola bakmalý yönde -1f olmalýdýr. 
        {
            yonSagmi = false;
            geciciScale.x = -1f;
        }

        transform.localScale = geciciScale;         //gecici vectorden tekrar normal scale atama yaptýk

    }

    public void geriTepmeFNC()
    {
        geriTepmeSayaci = geriTepmeSuresi;
        rb.velocity = new Vector2(0, rb.velocity.y);  //karakter çarptýðý zaman hýzýný 0 yapýyoruz.
        anim.SetTrigger("hasar");                       //karakter hasar aldýðýgýnda oluþak animasyonu tetikledik.
    }

    public void ustundeZiplaFNC()
    {
        rb.velocity = new Vector2(rb.velocity.x, ustundeZiplaGucu);         //Düþmana bastýðýnda y yönünde bir hýz ile ekstra ziplamayý saglar;
        SesController.instance.SesEfektiCikar(3);       //ziplama sesi
    }

    public void bouncerZiplamaFNC()
    {
        rb.velocity = new Vector2(rb.velocity.x, bouncerZiplaGucu);
    }

}
