using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    public string la_main;
    public LayerMask[] layers;
    public string[] names;
    public GameObject[] meubles;
    private GameObject meuble;
    private bool est_tenu;
    // Start is called before the first frame update
    void Start()
    {
        est_tenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if ((OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetMouseButtonDown(0)) && la_main == "La main droite")
        {
            if (est_tenu && !meuble.GetComponent<Meuble>().collision)
            {
                est_tenu = false;

                meuble.AddComponent<Rigidbody>();

            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    int index = 0;
                    foreach (string name in names)
                    {
                        if (name == hit.transform.gameObject.name)
                        {
                            meuble = Instantiate(meubles[index], transform.position + transform.forward * 0.5f, Quaternion.identity);
                            est_tenu = true;
                            break;
                        }
                        index++;
                    }
                }
            }
        }
        if (est_tenu)
        {
            meuble.transform.position = transform.position + transform.forward;
        }
    }
    

}
