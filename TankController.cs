using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public enum tankDurumlari {atesEtme,darbeAlma,hareketEtme,sonaErdi}              //Karýþýk durumlarda kodu kontrol edebilmemize fayda saðlar.
    public tankDurumlari gecerliDurum;                                      //enum deðiþkeninine eriþebilmek için


    [SerializeField]
    Transform tankObje;

    public Animator anim;

    [Header("Hareket")]
    public float hareketHizi;
    public Transform solHedef, sagHedef;
    bool yonuSagmi;
    public GameObject mayinObje;
    public Transform mayinMerkezNoktasi;
    public float mayinBirakmasuresi;
    float mayinbirakmaSayac;


    [Header("AtesEtme")]
    public GameObject mermi;
    public Transform mermiMerkezi;
    public float mermiAtmaSuresi;
    float mermiAtmaSayac;

    [Header("ZararAlma")]
    public float darbeSuresi;
    float darbeSayaci;

    [Header("CanDurumu")]
    public int canDurumu = 5;
    public GameObject tankPatlamaEfekti;
    public bool yenildimi;
    public float mermiSuresiArttir, mayinBirakmaSuresiArttir;

    public GameObject tankEziciKutu;

    private void Start()
    {
        gecerliDurum = tankDurumlari.atesEtme;
    }

    private void Update()
    {
        switch (gecerliDurum)
        {
            case tankDurumlari.atesEtme:
                //ates edildiðinde olacak durumlar

                mermiAtmaSayac -= Time.deltaTime;               //mermi belli aralýklarla atýlsýn diye sayacý azaltýyoruz.
                if (mermiAtmaSayac <= 0)                        //sayac sýfýr olduðunda mermi atacaðýz.
                {
                    mermiAtmaSayac = mermiAtmaSuresi;           //tekrardan sayacý resetliyoruz ki terkar mermi atabilsin.

                   var yeniMermi= Instantiate(mermi, mermiMerkezi.position, mermiMerkezi.rotation);         //mermi oluþturuyoruz.
                    yeniMermi.transform.localScale = tankObje.localScale;       //merminin yönünü tankýn yönü ile eþitliyoruz. 
                    SesController.instance.SesEfektiCikar(8);

                }

                break;

            case tankDurumlari.darbeAlma:
                //Tank darbe aldýðýnda olacak durumlar
                if (darbeSayaci > 0)
                {
                    darbeSayaci -= Time.deltaTime;

                    if (darbeSayaci <= 0)
                    {
                        gecerliDurum = tankDurumlari.hareketEtme;

                        mayinbirakmaSayac = 0;          //Darbe aldýðý durumda mayýn býrakamamasý için

                        if (yenildimi)
                        {
                           
                            tankObje.gameObject.SetActive(false);           //tankýn caný bittiðinde yok olmasý için
                            Instantiate(tankPatlamaEfekti, tankObje.position, tankObje.rotation);  //yokOlmaEfekti ile yok oluyor
                            SesController.instance.SesEfektiCikar(0);
                            gecerliDurum = tankDurumlari.sonaErdi;      //sahnede mayýn mermi vs kalmamasý için
                        }
                    }
                }

                break;

            case tankDurumlari.hareketEtme:
                //tank hareket ettiðinde olacak durumlar

                if (yenildimi == false)
                {
                    if (yonuSagmi)
                    {
                        tankObje.position += new Vector3(hareketHizi * Time.deltaTime, 0f, 0f);     //tankýmýzýn pozisyonunu hareket ettirme.

                        if (tankObje.position.x > sagHedef.position.x)          //sag hedefimizi geçtiðinde
                        {
                            tankObje.localScale = Vector3.one;                  //yonunu 1f yapýyoruz. scaleden
                            yonuSagmi = false;                                  //artýk yonu sol oluyor.

                            HareketiDurdurFNC();

                        }

                    }
                    else
                    {
                        tankObje.position -= new Vector3(hareketHizi * Time.deltaTime, 0f, 0f);         //sol yönde bir hareket için -=

                        if (tankObje.position.x < solHedef.position.x)
                        {
                            tankObje.localScale = new Vector3(-1, 1, 1);            //yonu deðiþtirmek için
                            yonuSagmi = true;

                            HareketiDurdurFNC();

                        }

                    }
                }

                mayinbirakmaSayac -= Time.deltaTime;                                    //mayin býrakma sayacýný baþlatýyoruz.

                if (mayinbirakmaSayac <= 0)                                         //0 dan küçük olduðunda mayýn býraksýn
                {       
                    mayinbirakmaSayac = mayinBirakmasuresi;                                  //sureyi tekrar sayaca atýyoruz ki tekrar mayýn býrakabilsin.
                    Instantiate(mayinObje, mayinMerkezNoktasi.position, mayinMerkezNoktasi.rotation);
                    SesController.instance.SesEfektiCikar(10);
                }


                break;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DarbeAlFNC();
        }

        
    }

    public void DarbeAlFNC()
    {
        
        gecerliDurum = tankDurumlari.darbeAlma;
        darbeSayaci = darbeSuresi;                                          //Darbe sayacýný baþlatabilmek için eþitliyoruz. 

        anim.SetTrigger("Vur");             //animasyonlarý tetikleme

        MayinController[] mayinlar = FindObjectsOfType<MayinController>();

        if (mayinlar.Length > 0)
        {
            foreach (MayinController bulunanMayin in mayinlar)          //mayinlar dizi içerisindeki MayinController objelerini bul ve bulunanMayin isimli yere at
            {
                bulunanMayin.PatlamaFNC();    //Tank darbealdýðýnda mayinlari yok etmek için
            }
        }

        canDurumu--;                //Darbe aldýkça tankýn caný bir azalýyor ve sýfýrdan küçük oldumu yenildimi fonksiyonu true oluyor.
        if (canDurumu <= 0)
        {
            yenildimi = true;
        } else
        {
            mermiAtmaSuresi /= mermiSuresiArttir;                   //tank darbe aldýkça mayýn atma ve mermi atma süresini azaltýyoruz.
            mayinBirakmasuresi /= mayinBirakmaSuresiArttir;
        }

    }


    void HareketiDurdurFNC()
    {
        tankEziciKutu.SetActive(true);
        //Karakter tanka bastýðýnda kapanan ezici kutunun aktifliðini tekrar arttýrýyoruz.


        gecerliDurum = tankDurumlari.atesEtme;              //ates etme durumuna geçiyor ve mermi sayacýný baþlatýyor.
        mermiAtmaSayac = mermiAtmaSuresi;

        anim.SetTrigger("HareketiDurdur");

    }

}
