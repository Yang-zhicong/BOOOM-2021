using System;
using System.Threading;
using System.Reflection.Emit;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public GameObject enemy;
    public GameObject boom;
    public GameObject HPUI;
    public GameObject BOOOMUI;
    public float maxhp = 100;
    public float hp;
    float maxbooomValue = 300;
    float booomValue;
    public float booomValueRecovery = 1;
    GameObject booom1, booom2, booom3;
    public GameObject deathUI;
    public AudioClip getShotSFX, shootSFX;
    AudioSource audioSource;

    public Slider TimeFeeling;
    public float TragetTimeFeel;
    public float TragetTimeFeelHurt;
    public float expSpeed;

    public bool IsTimeFeeling;
    public GameObject PlayerSoul;

    //public DelayMove delaymove;

    public bullet Bullet;







    bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        TimeFeeling.value = 0f;
        audioSource = GetComponent<AudioSource>();
        rb = transform.GetComponent<Rigidbody>();     
        hp = maxhp;
        booomValue = maxbooomValue;
        booom1 = BOOOMUI.transform.GetChild(1).gameObject;
        booom2 = BOOOMUI.transform.GetChild(2).gameObject;
        booom3 = BOOOMUI.transform.GetChild(3).gameObject;


    }

    // Update is called once per frame
    void Update()
    {
        //角色朝向敌人
        //transform.LookAt(new Vector3(
        //enemy.transform.position.x,
        //1,
        //enemy.transform.position.z));


        //角色朝向光标
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray,out hit);
        if (hit.transform != null && hit.transform.tag != "Player")
        {
            transform.LookAt(new Vector3(hit.point.x, 1, hit.point.z));
        }      

        // if(alive)
        // {
        //     if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
        //     {
        //         rb.velocity = new Vector3(
        //             Input.GetAxis("Horizontal"),
        //             0,
        //             Input.GetAxis("Vertical")) * speed * Mathf.Sqrt(2) / 2;
        //     }
        //     else
        //         rb.velocity = new Vector3(
        //            Input.GetAxis("Horizontal"),
        //            0,
        //            Input.GetAxis("Vertical")) * speed;

        // }

        if (Input.GetMouseButton(0))
        {
            Debug.DrawLine(transform.position, hit.point);
        }

        if (Input.GetMouseButtonUp(0) && booomValue >= 100)
        {
            GameObject boomClone;
            boomClone = Instantiate(boom, transform.position, 
                transform.rotation);
            boomClone.GetComponent<Rigidbody>().velocity =
                transform.TransformDirection(Vector3.forward * 8);
            audioSource.clip = shootSFX;
            audioSource.Play();
            booomValue -= 100;

            TragetTimeFeel = TimeFeeling.value + 3f;

            IsTimeFeeling = true;
            PlayerSoul.GetComponent<DelayMove>().delay += 0.5f;


        }

        //时间感知条增幅
        if(IsTimeFeeling)
        {
            if(TimeFeeling.value < TragetTimeFeel)
        {            
            float SliderLength = 10f;

            TimeFeeling.value += Mathf.Clamp(TragetTimeFeel -  TimeFeeling.value, SliderLength * 0.05f , SliderLength * 0.5f ) * expSpeed * Time.deltaTime;
            //delaymove.delay += 0.1f;
        }
        else
        {
            IsTimeFeeling =false;

        }

        }





        if(hp<=0)
        {
            alive = false;
            updatePlayerHPInfo();
            Time.timeScale = 0.3f;
            audioSource.clip = getShotSFX;
            audioSource.Play();
            deathUI.SetActive(true);
        }

        updatePlayerBOOOMInfo();
        if (alive && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }



    }

    public void updatePlayerHPInfo()
    {   

        if(alive)
        {
            HPUI.transform.GetChild(0).GetComponent<Image>().fillAmount = hp/maxhp;
            audioSource.clip = getShotSFX;
            audioSource.Play();

            //受击后时间感知

            float SliderLength = 10f;
            PlayerSoul.GetComponent<DelayMove>().delay -= 0.5f;

            TragetTimeFeelHurt = TimeFeeling.value + 5f;
            if(TimeFeeling.value < TragetTimeFeelHurt)
            {
                TimeFeeling.value -= Mathf.Clamp(TragetTimeFeelHurt - TimeFeeling.value, SliderLength * 0.05f , SliderLength * 0.5f ) * expSpeed * Time.deltaTime;
                
            }

        }   
        else
        {
            //rb.AddForce(Vector3.up * 20);
            transform.GetComponent<BoxCollider>().enabled =false;
        }
    }
    public void updatePlayerBOOOMInfo()
    {
        BOOOMUI.transform.GetChild(0).GetComponent<Image>().fillAmount =
            booomValue / maxbooomValue;

        if (booomValue >= maxbooomValue)
            booom3.SetActive(true);
        else
            booom3.SetActive(false);

        if (booomValue >= maxbooomValue / 3 * 2)
            booom2.SetActive(true);
        else
            booom2.SetActive(false);

        if (booomValue >= maxbooomValue / 3)
            booom1.SetActive(true);
        else
            booom1.SetActive(false);

        if (booomValue<=maxbooomValue)
            booomValue += booomValueRecovery * Time.deltaTime;
    }
}
