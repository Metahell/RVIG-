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
    private float[] zPos = {14.649f,14.89f,15.131f};
    private float[] xPos = {7.944f, 8.185f, 8.426f};
    private Vector3 n_pos;
    public Text UIwallmode;
    // Start is called before the first frame update
    void Start()
    {
        UIwallmode.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            wallmode = !wallmode;
            righthand.enabled = !wallmode;
            UIwallmode.enabled = wallmode;
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
                    
                    if (!ontarget)
                    {
                        ontarget = true;
                        n_pos = new Vector3(getClosest(xPos, hit.point.x), hit.point.y, getClosest(zPos, hit.point.z));
                        if (indic == null)
                        {
                            indic = Instantiate(wallIndic, n_pos, Quaternion.identity);
                            Rigidbody rigi = indic.AddComponent<Rigidbody>();
                            rigi.useGravity = false;
                            rigi.isKinematic = true;
                            rigi.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                        }
                        else
                        {
                            indic.GetComponent<Renderer>().enabled = true;
                        }
                    }
                    if (ontarget)
                    {
                        if (change)
                        {
                            change = false;
                            Destroy(indic);
                            n_pos = new Vector3(getClosest(xPos, hit.point.x), hit.point.y, getClosest(zPos, hit.point.z));
                            indic = Instantiate(wallIndic, n_pos, Quaternion.identity);
                        }
                        n_pos = new Vector3(getClosest(xPos, hit.point.x), hit.point.y, getClosest(zPos, hit.point.z));
                        indic.transform.position = n_pos;
                        indic.transform.rotation = Quaternion.Euler(0, rotation, 0);
                    }
                    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    {
                        if (indic.GetComponent<Mur>().can_place)
                        {
                            n_pos = new Vector3(getClosest(xPos, hit.point.x), hit.point.y, getClosest(zPos, hit.point.z));
                            Instantiate(wall, n_pos, Quaternion.Euler(0, rotation, 0));
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
                        if (mur.GetComponent<Mur>().newmur)
                        {
                            Destroy(mur);
                        }
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

    private float getClosest(float[] positions,float position)
    {
        float min_diff = float.MaxValue;
        int index = 0;
        for (int i = 0; i<3;i++)
        {
            min_diff = Mathf.Min(min_diff, Mathf.Abs(position - positions[i]));
            if (min_diff == Mathf.Abs(position - positions[i]))
            {
                index = i;
            }
        }
        return positions[index];
    }
}
