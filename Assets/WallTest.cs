using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTest : MonoBehaviour
{
    public Text text;
    private bool wallmode = false;
    [SerializeField]
    private RightHand righthand;
    [SerializeField]
    private Mur wall;
    private GameObject wallIndic;
    private float rotation;
    private float timeHeld;
    private bool ontarget=false;
    public OVRPlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        ResetIndic();
    }

    // Update is called once per frame
    void Update()
    {
        Print(wallIndic.transform.position.ToString());
        RaycastHit hit;
        Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            if (wallmode)
            {
                wallmode = false;
                controller.EnableRotation = true;
                righthand.enabled = true;
                Print("Mode mur désactivé");
            }
            else
            {
                wallmode = true;
                controller.EnableRotation = false;
                righthand.enabled = false;
                Print("Mode mur activé");
            }
        }
        if (wallmode)
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
            {
                if (secondaryAxis.x > 0.5)
                {
                    rotation += 90;
                }
                else if (secondaryAxis.x < 0.5)
                {
                    rotation -= 90;
                }
            }
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.transform.gameObject.tag == "sol")
                {
                    if (ontarget)
                    {
                        wallIndic.transform.position = hit.point;
                        wallIndic.transform.rotation = Quaternion.Euler(0, rotation, 0);
                    }
                    if (!ontarget)
                    {
                        ontarget = true;
                        wallIndic.GetComponent<Renderer>().enabled = true;
                        wallIndic = Instantiate(wallIndic, hit.point, Quaternion.Euler(0, rotation, 0));
                    }
                    Vector3 adjustpos = hit.point;
                    if (canPlaceWall(adjustpos))
                    {
                        wallIndic.GetComponent<Mur>().canplace();
                    }
                    else
                    {
                        wallIndic.GetComponent<Mur>().cantplace();
                    }
                    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    {
                        if (canPlaceWall(adjustpos))
                        {
                            Instantiate(wall.gameObject, adjustpos, Quaternion.Euler(0, rotation, 0));
                            Print("CurrentPos" + adjustpos.ToString());
                        }
                    }
                }
                else
                {
                    ontarget = false;
                    wallIndic.GetComponent<Renderer>().enabled = false;
                    ResetIndic();
                }
                if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
                {
                    if (hit.transform.gameObject.tag == "mur")
                    {
                        GameObject mur = hit.transform.gameObject;
                        Destroy(mur);
                    }
                }
            }
        }
    }
        bool canPlaceWall(Vector3 hitpos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(hitpos, 0.05f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("mur"))
                {
                    return false;
                }
            if (hitCollider.CompareTag("meuble"))
            {
                return false;
            }
            }
            return true;
        }
    void ResetIndic()
    {
        wallIndic = wall.gameObject;
        wallIndic.GetComponent<Mur>().newmur = false;
        wallIndic.GetComponent<Collider>().isTrigger = true;
        wallIndic.tag = "Default";
        wallIndic.layer = 2;
    }
        void Print(string s)
        {
            text.text = s;
        }
    }
