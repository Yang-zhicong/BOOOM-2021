using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullet : MonoBehaviour
{
    public float speed;
    public float damage = 10;
    public float damegeSpeed = 5;
    public GameObject blood;
    public GameObject bloodE;



    Rigidbody rb;
    GameObject bulletlight;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (rb.velocity.magnitude >= damegeSpeed)
            {

                collision.transform.GetComponent<player>().hp -= damage;

                collision.transform.GetComponent<player>().updatePlayerHPInfo();
                ContactPoint contact = collision.contacts[0];               
                Instantiate(blood, contact.point, Quaternion.FromToRotation(Vector3.forward, contact.normal));
                Instantiate(blood, contact.point, Quaternion.FromToRotation(Vector3.back, contact.normal));
                Destroy(gameObject);

            }                   
        }
        if (collision.transform.tag == "Enemy")
        {
            //UnityEngine.Debug.Log("���е���");
            collision.transform.GetComponent<enemy>().hurt();
            ContactPoint contact = collision.contacts[0];
            Instantiate(bloodE, contact.point, Quaternion.FromToRotation(Vector3.forward, contact.normal));
            Destroy(gameObject);
        }
    }

    public void autoDestory()
    {
        Destroy(gameObject);
    }

    public void lightDestory()
    {
        Destroy(bulletlight);
    }

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        bulletlight = transform.GetChild(1).gameObject;
        Invoke("autoDestory",20);
        Invoke("lightDestory", 0.1f);

    }

}
