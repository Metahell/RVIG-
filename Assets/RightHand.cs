﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RightHand : MonoBehaviour
{
    //liste des meubles disponibles
    public GameObject[] meubles;
    //meuble tenu par le joueur
    private GameObject meuble;
    //booléen déterminant si le joueur tient un meuble
    private bool est_tenu;
    public Text text;
    public OVRPlayerController controller;
    private float rotation;
    //objet vide placeholder gardant la position standard d'un objet mural selectionné
    public GameObject pour_mural;
    // Start is called before the first frame update
    //initialisation de pour_mural, nécessaire pour placer les objets muraux, est_tenu qui permet de savoir si le joueur tient un meuble et rotation qui définit la rotation du meuble
    void Start()
    {
        pour_mural = Instantiate(new GameObject("pour_mural"), transform.position + transform.forward * 0.5f, Quaternion.identity);
        pour_mural.transform.parent = transform;
        est_tenu = false;
        rotation = 0;
    }

    // Update is called once per frame
    //gère les inputs de la main droite
    void Update()
    {
        RaycastHit hit;
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetMouseButtonDown(0))
        {
            if (est_tenu && meuble.GetComponent<Meuble>().can_place)
            {
                est_tenu = false;
                Rigidbody rigi = meuble.GetComponent<Rigidbody>();

                if (!meuble.GetComponent<Meuble>().mural)
                {
                    rigi.useGravity = true;
                    rigi.isKinematic = false;
                }
                meuble.layer = 0;
                rigi.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                meuble.transform.parent = null;
                meuble.GetComponent<Collider>().isTrigger = false;

                controller.EnableRotation = true;
                rotation = 0.0f;

            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit) && !est_tenu)
                {
                    if (hit.transform.gameObject.CompareTag("reset"))
                    {
                        hit.transform.gameObject.GetComponent<ResetButton>().Reset();
                    }
                    Debug.Log(hit.transform.gameObject.name);
                    bool new_meuble = false;
                    foreach (GameObject meuble_c in meubles)
                    {
                        if (meuble_c.name == hit.transform.gameObject.name)
                        {
                            meuble = Instantiate(meuble_c, transform.position + transform.forward * 0.5f, Quaternion.identity);

                            Rigidbody rigi = meuble.AddComponent<Rigidbody>();
                            rigi.useGravity = false;
                            rigi.isKinematic = true;
                            rigi.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                            meuble.transform.parent = transform;
                            est_tenu = true;
                            new_meuble = true;
                            if (meuble.GetComponent<Meuble>().mural)
                            {
                                meuble.layer = 2;
                            }
                            break;
                        }
                    }
                    if (!new_meuble)
                    {
                        if (hit.transform.gameObject.tag == "meuble")
                        {
                            meuble = hit.transform.gameObject;
                            meuble.transform.position = transform.position + transform.forward * 0.5f;
                            meuble.GetComponent<Rigidbody>().useGravity = false;
                            meuble.transform.parent = transform;
                            meuble.GetComponent<Collider>().isTrigger = true;
                            est_tenu = true;
                            if (meuble.GetComponent<Meuble>().mural)
                            {
                                meuble.layer = 2;
                            }
                        }
                    }
                }
            }
        }
        if (est_tenu)
        {
            Meuble meuble_script = meuble.GetComponent<Meuble>();
            meuble_script.sur_un_mur = false;
            if (meuble_script.mural)
            {
                if (Physics.Raycast(transform.position,transform.forward,out hit))
                {
                    if (hit.transform.gameObject.CompareTag("mur"))
                    {
                        meuble.transform.parent = null;
                        meuble.transform.position = hit.point;
                        Quaternion new_rotation = new Quaternion();
                        new_rotation.SetLookRotation(hit.normal, Vector3.up);
                        meuble.transform.rotation = new_rotation;
                        meuble_script.can_place = true;
                        meuble_script.sur_un_mur = true;
                    }
                    else
                    {
                        meuble_script.can_place = false;
                        meuble_script.sur_un_mur = false;
                        meuble.transform.parent = transform;
                        meuble.transform.position = pour_mural.transform.position;
                    }
                }




                //Collider[] hit_colliders = Physics.OverlapSphere(pour_mural.transform.position, 0.2f);
                //foreach (Collider col in hit_colliders)
                //{
                //    if (col.transform.gameObject.CompareTag("mur"))
                //    {
                //        Vector3 mur_position = col.ClosestPoint(pour_mural.transform.position);
                //        if (Physics.Raycast(pour_mural.transform.position, mur_position - pour_mural.transform.position, out hit))
                //        {
                //            if (hit.transform.gameObject.CompareTag("mur"))
                //            {
                //                if (hit.normal == Vector3.up)
                //                {
                //                    break;
                //                }
                //                meuble.transform.parent = null;
                //                meuble.transform.position = hit.point;
                //                Quaternion new_rotation = new Quaternion();
                //                new_rotation.SetLookRotation(hit.normal, Vector3.up);
                //                meuble.transform.rotation = new_rotation;
                //                meuble_script.can_place = true;
                //                meuble_script.sur_un_mur = true;
                //                break;

                //            }
                //            else
                //            {
                //                meuble_script.can_place = false;
                //                meuble_script.sur_un_mur = false;
                //                meuble.transform.parent = transform;
                //                meuble.transform.position = pour_mural.transform.position;

                //            }
                //        }
                //        else
                //        {
                //            meuble_script.can_place = false;
                //            meuble_script.sur_un_mur = false;
                //            meuble.transform.parent = transform;
                //            meuble.transform.position = pour_mural.transform.position;

                //        }

                //    }
                //    else
                //    {
                //        meuble_script.can_place = false;
                //        meuble_script.sur_un_mur = false;
                //        meuble.transform.parent = transform;
                //        meuble.transform.position = pour_mural.transform.position;
                //    }
                //}
            }

            Vector3 lookat = transform.position;
            lookat.y = meuble.transform.position.y;
            if (!meuble.GetComponent<Meuble>().sur_un_mur)
            {
                meuble.transform.LookAt(lookat);
            }
            Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            rotation -= secondaryAxis.x * 2;
            if (!meuble.GetComponent<Meuble>().mural)
            {
                controller.EnableRotation = false;
            }
            if (OVRInput.Get(OVRInput.Button.Two))
            {
                pour_mural.transform.position += transform.forward / 50;
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {
                    meuble.transform.position += transform.forward / 50;
                }
            }

            if (OVRInput.Get(OVRInput.Button.One))
            {
                pour_mural.transform.position -= transform.forward / 50;
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {
                    meuble.transform.position -= transform.forward / 50;
                }
            }

            if ((pour_mural.transform.position - transform.position).magnitude > 1f)
            {
                pour_mural.transform.position = transform.position + transform.forward;
            }

            if ((pour_mural.transform.position - transform.position).magnitude < 0.2f)
            {
                pour_mural.transform.position = transform.position + transform.forward * 0.2f;
            }

            if ((meuble.transform.position - transform.position).magnitude > 1f)
            {
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {

                    meuble.transform.position = transform.position + transform.forward;
                }
            }
            if ((meuble.transform.position - transform.position).magnitude < 0.2f)
            {
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {
                    meuble.transform.position = transform.position + transform.forward * 0.2f;
                }
            }
            if (!meuble.GetComponent<Meuble>().mural)
            {
                meuble.transform.Rotate(0, rotation, 0);
            }
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) || Input.GetMouseButtonDown(1))
            {
                Destroy(meuble);
                est_tenu = false;
                controller.EnableRotation = true;
            }
        }
    }
    void Print(string s)
    {
        text.text += s;
    }

    void Clear()
    {
        text.text = "";
    }

}
