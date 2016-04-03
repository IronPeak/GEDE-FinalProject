using UnityEngine;
using System.Collections;

public class Track2Trigger : MonoBehaviour
{

    public Gun gun;
    public GameObject ui;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gun.enabled = false;
            ui.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            gun.enabled = true;
            ui.SetActive(false);
        }
    }

}
