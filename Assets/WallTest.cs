using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTest : MonoBehaviour
{
    public Text text;
    private bool change = false;
    private bool wallmode = false;
    private int i = 0;
    private int max = 2;
    [SerializeField]
    private GameObject[] ListWalls;
    [SerializeField]
    private GameObject[] ListIndic;
    [SerializeField]
    private RightHand righthand;
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject wallIndic;
    private GameObject indic;
    private float rotation=0;
    private float timeHeld;
    private bool ontarget = false;
    public OVRPlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            if (wallmode)
            {
                wallmode = false;
                controller.EnableRotation = true;
                righthand.enabled = true;
            }
            else
            {
                wallmode = true;
                controller.EnableRotation = false;
                righthand.enabled = false;
            }
        }
        if (wallmode)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                Incr(0);
                wall = ListWalls[i];
                wallIndic = ListIndic[i];
            }
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                Incr(1);
                wall = ListWalls[i];
                wallIndic = ListIndic[i];
            }
            rotation -= secondaryAxis.x * 2;
            if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
            {
                rotation += 90;
            }
            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
            {
                rotation -= 90;
            }
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.transform.gameObject.tag == "sol")
                {
                    
                    if (!ontarget||indic==null)
                    {
                        ontarget = true;
                        indic = Instantiate(wallIndic, hit.point, Quaternion.identity);
                        Rigidbody rigi = indic.AddComponent<Rigidbody>();
                        rigi.useGravity = false;
                        rigi.isKinematic = true;
                        rigi.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    }
                    if (ontarget)
                    {
                        if (change)
                        {
                            change = false;
                            Destroy(indic);
                            indic= Instantiate(wallIndic, hit.point, Quaternion.identity);
                        }
                        indic.transform.position = hit.point;
                        indic.transform.rotation = Quaternion.Euler(0, rotation, 0);
                    }
                    Vector3 adjustpos = hit.point;
                    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    {
                        if (indic.GetComponent<Mur>().can_place)
                        {
                            Instantiate(wall, adjustpos, Quaternion.Euler(0, rotation, 0));
                        }
                    }
                }
                else
                {
                    ontarget = false;
                    indic.GetComponent<Renderer>().enabled = false;
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
        else
        {
            if (indic != null)
            {
                Destroy(indic);
            }
        }
    }
    bool canPlaceWall(Vector3 hitpos)
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitpos, 0.01f);
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
    private void Incr(int type)
    {
        if (i == max&&type==0)
        {
            i = 0;
        }
        else if (i == 0 && type == 1)
        {
            i = max;
        }
        else if (type == 0)
        {
            i++;
        }
        else
        {
            i--;
        }
        change = true;
    }
}
