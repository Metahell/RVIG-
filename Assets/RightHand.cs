using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RightHand : MonoBehaviour
{
    public string[] names;
    public GameObject[] meubles;
    private GameObject meuble;
    private bool est_tenu;
    public Text text;
    public OVRPlayerController controller;
    private float rotation;
    // Start is called before the first frame update
    void Start()
    {
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
                meuble.GetComponent<Rigidbody>().useGravity = true;
                meuble.transform.parent = null;
                meuble.GetComponent<Collider>().isTrigger = false;

                controller.EnableRotation = true;
                rotation = 0.0f;

            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    bool new_meuble = false;
                    int index = 0;
                    foreach (string name in names)
                    {
                        if (name == hit.transform.gameObject.name)
                        {
                            meuble = Instantiate(meubles[index], transform.position + transform.forward * 0.5f, Quaternion.identity);

                            meuble.AddComponent<Rigidbody>().useGravity = false;
                            meuble.transform.parent = gameObject.transform;
                            est_tenu = true;
                            new_meuble = true;
                            break;
                        }
                        index++;
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
            meuble.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            rotation += secondaryAxis.x;
            Clear();
            Print("test\n");
            Print(secondaryAxis.ToString()+"\n");
            Print(rotation.ToString()+"\n");
            Print(OVRInput.Get(OVRInput.Button.Any).ToString()+"\n");
            controller.EnableRotation = false;
            if ( OVRInput.Get(OVRInput.Button.Two))
            {
                Print("B pressé\n");
                meuble.transform.position += transform.forward / 50;
            }
            
            if ( OVRInput.Get(OVRInput.Button.One))
            {
                Print("A pressé\n");
                meuble.transform.position -= transform.forward / 50;
            }
            
            if ((meuble.transform.position - transform.position).magnitude > 1f)
            {
                meuble.transform.position = transform.position + transform.forward;
            }
            if ((meuble.transform.position - transform.position).magnitude < 0.2f)
            {
                meuble.transform.position = transform.position + transform.forward * 0.2f;
            }
            meuble.transform.Rotate(0, rotation, 0);
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) || Input.GetMouseButtonDown(1))
            {
                Destroy(meuble);
                est_tenu = false;
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
