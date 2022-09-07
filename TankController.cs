using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public enum tankDurumlari {atesEtme,darbeAlma,hareketEtme,sonaErdi}              //Kar���k durumlarda kodu kontrol edebilmemize fayda sa�lar.
    public tankDurumlari gecerliDurum;                                      //enum de�i�keninine eri�ebilmek i�in


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
                //ates edildi�inde olacak durumlar

                mermiAtmaSayac -= Time.deltaTime;               //mermi belli aral�klarla at�ls�n diye sayac� azalt�yoruz.
                if (mermiAtmaSayac <= 0)                        //sayac s�f�r oldu�unda mermi ataca��z.
                {
                    mermiAtmaSayac = mermiAtmaSuresi;           //tekrardan sayac� resetliyoruz ki terkar mermi atabilsin.

                   var yeniMermi= Instantiate(mermi, mermiMerkezi.position, mermiMerkezi.rotation);         //mermi olu�turuyoruz.
                    yeniMermi.transform.localScale = tankObje.localScale;       //merminin y�n�n� tank�n y�n� ile e�itliyoruz. 
                    SesController.instance.SesEfektiCikar(8);

                }

                break;

            case tankDurumlari.darbeAlma:
                //Tank darbe ald���nda olacak durumlar
                if (darbeSayaci > 0)
                {
                    darbeSayaci -= Time.deltaTime;

                    if (darbeSayaci <= 0)
                    {
                        gecerliDurum = tankDurumlari.hareketEtme;

                        mayinbirakmaSayac = 0;          //Darbe ald��� durumda may�n b�rakamamas� i�in

                        if (yenildimi)
                        {
                           
                            tankObje.gameObject.SetActive(false);           //tank�n can� bitti�inde yok olmas� i�in
                            Instantiate(tankPatlamaEfekti, tankObje.position, tankObje.rotation);  //yokOlmaEfekti ile yok oluyor
                            SesController.instance.SesEfektiCikar(0);
                            gecerliDurum = tankDurumlari.sonaErdi;      //sahnede may�n mermi vs kalmamas� i�in
                        }
                    }
                }

                break;

            case tankDurumlari.hareketEtme:
                //tank hareket etti�inde olacak durumlar

                if (yenildimi == false)
                {
                    if (yonuSagmi)
                    {
                        tankObje.position += new Vector3(hareketHizi * Time.deltaTime, 0f, 0f);     //tank�m�z�n pozisyonunu hareket ettirme.

                        if (tankObje.position.x > sagHedef.position.x)          //sag hedefimizi ge�ti�inde
                        {
                            tankObje.localScale = Vector3.one;                  //yonunu 1f yap�yoruz. scaleden
                            yonuSagmi = false;                                  //art�k yonu sol oluyor.

                            HareketiDurdurFNC();

                        }

                    }
                    else
                    {
                        tankObje.position -= new Vector3(hareketHizi * Time.deltaTime, 0f, 0f);         //sol y�nde bir hareket i�in -=

                        if (tankObje.position.x < solHedef.position.x)
                        {
                            tankObje.localScale = new Vector3(-1, 1, 1);            //yonu de�i�tirmek i�in
                            yonuSagmi = true;

                            HareketiDurdurFNC();

                        }

                    }
                }

                mayinbirakmaSayac -= Time.deltaTime;                                    //mayin b�rakma sayac�n� ba�lat�yoruz.

                if (mayinbirakmaSayac <= 0)                                         //0 dan k���k oldu�unda may�n b�raks�n
                {       
                    mayinbirakmaSayac = mayinBirakmasuresi;                                  //sureyi tekrar sayaca at�yoruz ki tekrar may�n b�rakabilsin.
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
        darbeSayaci = darbeSuresi;                                          //Darbe sayac�n� ba�latabilmek i�in e�itliyoruz. 

        anim.SetTrigger("Vur");             //animasyonlar� tetikleme

        MayinController[] mayinlar = FindObjectsOfType<MayinController>();

        if (mayinlar.Length > 0)
        {
            foreach (MayinController bulunanMayin in mayinlar)          //mayinlar dizi i�erisindeki MayinController objelerini bul ve bulunanMayin isimli yere at
            {
                bulunanMayin.PatlamaFNC();    //Tank darbeald���nda mayinlari yok etmek i�in
            }
        }

        canDurumu--;                //Darbe ald�k�a tank�n can� bir azal�yor ve s�f�rdan k���k oldumu yenildimi fonksiyonu true oluyor.
        if (canDurumu <= 0)
        {
            yenildimi = true;
        } else
        {
            mermiAtmaSuresi /= mermiSuresiArttir;                   //tank darbe ald�k�a may�n atma ve mermi atma s�resini azalt�yoruz.
            mayinBirakmasuresi /= mayinBirakmaSuresiArttir;
        }

    }


    void HareketiDurdurFNC()
    {
        tankEziciKutu.SetActive(true);
        //Karakter tanka bast���nda kapanan ezici kutunun aktifli�ini tekrar artt�r�yoruz.


        gecerliDurum = tankDurumlari.atesEtme;              //ates etme durumuna ge�iyor ve mermi sayac�n� ba�lat�yor.
        mermiAtmaSayac = mermiAtmaSuresi;

        anim.SetTrigger("HareketiDurdur");

    }

}
