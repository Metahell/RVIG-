using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftHand : MonoBehaviour
{
    public GameObject catalogue;
    [SerializeField]
    private Canvas Help;
    private bool vue_maquette;
    public OVRPlayerController controller;
    public Transform position_maquette;
    public Transform position_defaut;
    private bool off;
    private bool helpbool;
    private float time;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        timer = 0.1f;
        catalogue.SetActive(false);
        vue_maquette = false;
        helpbool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            catalogue.SetActive(!catalogue.activeInHierarchy);
        }
        if (OVRInput.GetDown(OVRInput.Button.Three) || Input.GetMouseButtonDown(1))
        {
            controller.enabled = false;
            off = true;
            time = 0f;
            if (!vue_maquette)
            {
                controller.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                controller.transform.position = position_maquette.position;
                vue_maquette = true;
            }
            else
            {
                controller.transform.localScale = new Vector3(1, 1, 1);
                controller.transform.position = position_defaut.position;
                vue_maquette = false;
            }

        }
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            if (!helpbool)
            {
                Help.enabled = true;
                helpbool = true;
            }
            else
            {
                Help.enabled = false;
                helpbool = false;
            }

        }
        if (off)
        {
            time += Time.deltaTime;
            if (time > timer)
            {
                controller.enabled = true;
                off = false;
                time = 0f;
            }
        }
    }
}
