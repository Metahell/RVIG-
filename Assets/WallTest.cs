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
            rotation -= secondaryAxis.x * 2;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.transform.gameObject.tag == "sol")
                {
                    if (!ontarget)
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
}
