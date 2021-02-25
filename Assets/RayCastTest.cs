﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    public string la_main;
    private float timer;
    private float ping;
    public LayerMask[] layers;
    public string[] names;
    public GameObject[] meubles;
    private GameObject meuble;
    private bool est_tenu;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        ping = 2f;
        est_tenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (timer > ping) {
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Debug.Log(la_main + " pointe sur l'objet " +hit.transform.gameObject.name);
        }
            timer = 0f;
        }
        timer += Time.deltaTime;
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && la_main == "La main droite")
        {
            if (Physics.Raycast(transform.position,transform.forward,out hit))
            {
                Debug.Log("bouton poussé, raycast a touché");
                int index = 0;
                foreach(string name in names)
                {
                    if (name == hit.transform.gameObject.name) {
                        meuble = Instantiate(meubles[index],transform.position+transform.forward,Quaternion.identity);
                        est_tenu = true;
                        break;
                    }
                    index++;
                }
            }
        }
        if (est_tenu)
        {
            meuble.transform.position = transform.position + transform.forward;
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) && la_main == "La main droite")
        {
            est_tenu = false;
        }
    }
}
