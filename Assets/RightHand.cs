using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RightHand : MonoBehaviour
{
    public GameObject[] meubles;
    private GameObject meuble;
    private bool est_tenu;
    public Text text;
    public OVRPlayerController controller;
    private float rotation;
    public GameObject pour_mural;
    // Start is called before the first frame update
    void Start()
    {
        pour_mural = Instantiate(new GameObject("pour_mural"), transform.position + transform.forward * 0.5f, Quaternion.identity);
        pour_mural.transform.parent = transform;
        est_tenu = false;
        rotation = 0;
    }

    // Update is called once per frame
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
                rigi.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                meuble.transform.parent = null;
                meuble.GetComponent<Collider>().isTrigger = false;

                controller.EnableRotation = true;
                rotation = 0.0f;

            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
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
                Collider[] hit_colliders = Physics.OverlapSphere(pour_mural.transform.position, 0.2f);
                foreach (Collider col in hit_colliders)
                {
                    if (col.transform.gameObject.CompareTag("mur"))
                    {
                        Vector3 mur_position = col.ClosestPoint(pour_mural.transform.position);
                        Debug.Log(mur_position);
                        meuble.transform.parent = null;
                        meuble.transform.position = mur_position;
                        meuble.transform.rotation = col.transform.rotation;
                        meuble.transform.Rotate(0, 90, 0);
                        meuble_script.can_place = true;
                        meuble_script.sur_un_mur = true;
                        break;
                    }
                    else
                    {
                        meuble_script.can_place = false;
                        meuble_script.sur_un_mur = false;
                        meuble.transform.parent = transform;
                        meuble.transform.position = pour_mural.transform.position;
                    }
                }
            }

            Vector3 lookat = transform.position;
            lookat.y = meuble.transform.position.y;
            if (!meuble.GetComponent<Meuble>().sur_un_mur)
            {
                meuble.transform.LookAt(lookat);
            }
            Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            rotation -= secondaryAxis.x * 2;
            Clear();
            Print("test\n");
            Print(secondaryAxis.ToString() + "\n");
            Print(rotation.ToString() + "\n");
            Print(OVRInput.Get(OVRInput.Button.Any).ToString() + "\n");
            controller.EnableRotation = false;
            if (OVRInput.Get(OVRInput.Button.Two))
            {
                Print("B pressé\n");
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {
                    meuble.transform.position += transform.forward / 50;
                }
            }

            if (OVRInput.Get(OVRInput.Button.One))
            {
                Print("A pressé\n");
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {
                    meuble.transform.position -= transform.forward / 50;
                }
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
            if (meuble.name == "Classic_Window_03_snaps001(Clone)")
            {
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {
                    meuble.transform.Rotate(0, 180, 0);
                }
            }
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) || Input.GetMouseButtonDown(1))
            {
                Destroy(meuble);
                est_tenu = false;
                controller.EnableRotation = true;
            }
        }
        else
        {
            Clear();
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
