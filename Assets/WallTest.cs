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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (wallmode)
        {
            righthand.enabled = false;
        }
        else righthand.enabled = true;
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            if (wallmode)
            {
                wallmode = false;
                Print("Mode mur désactivé");
            }
            else
            {
                wallmode = true;
                Print("Mode mur activé");
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (wallmode)
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.transform.gameObject.tag == "sol")
                    {
                        /**Vector3 adjustpos=hit.transform.gameObject.GetComponent<Collider>().ClosestPoint(hit.transform.localPosition);**/
                        Vector3 raypos = hit.transform.InverseTransformPoint(hit.point);
                        Vector3 adjustpos = new Vector3(raypos.x+8.2f,raypos.y-2.22f, raypos.z+14.8f);
                        if (canPlaceWall(adjustpos))
                        {
                            Instantiate(wall, adjustpos, Quaternion.identity);
                            Print("CurrentPos"+hit.transform.InverseTransformPoint(adjustpos).ToString());
                        }
                    }
                }
            }
            if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
            {
                if (wallmode)
                {
                    if (Physics.Raycast(transform.position, transform.forward, out hit))
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
        }
        return true;
    }
    void Print(string s)
    {
        text.text = s;
    }
}
