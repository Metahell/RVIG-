﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catalogue : MonoBehaviour
{
    public GameObject catalogue;
    // Start is called before the first frame update
    void Start()
    {
        catalogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            catalogue.SetActive(!catalogue.activeInHierarchy);
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            catalogue.SetActive(!catalogue.activeInHierarchy);
        }
    }
}
