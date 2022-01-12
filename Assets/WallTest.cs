using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTest : MonoBehaviour
{
    public Text text;

    private bool change = false;    //utilisé lors du changement du type de mur
   
    public static bool wallmode = false;  //définit si le mode mur est activé ou non

    //indices utilisés dans les listes de murs/murs indicateurs
    private int i = 0;
    private int max = 2;

    //listes de murs/murs indicateurs
    [SerializeField]
    private GameObject[] ListWalls;
    [SerializeField]
    private GameObject[] ListIndic;
    [SerializeField]
    private RightHand righthand;

    [SerializeField]
    private LeftHand lefthand;

    //Mur et mur indicateur actuels
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject wallIndic;

    private GameObject indic;   //Instance de l'indicateur, détruite ou changée au besoin

    private float rotation=0;    //rotation de l'indicateur et du mur posé

    private bool ontarget = false;    //définit si le joueur vise le sol de la maquette ou non

    private int switchNbr = 0;
    public OVRPlayerController controller; //controller du joueur

    //positions de placement des murs possibles
    private float[] zPos = {14.649f,14.89f,15.131f};
    private float[] xPos = {7.944f, 8.185f, 8.426f};

    private Vector3 n_pos;  //position de placement arrondie à l'une de celles possible

    public Text UIwallmode;    //texte indiquant au joueur si le mode mur est activé

    // Start is called before the first frame update

    void Start()
    {
        UIwallmode.enabled = false;    //mode mur désactivé par défaut
    }

    // Update is called once per frame
    //gère les inputs relatifs au mode mur
    void Update()
    {
        RaycastHit hit;
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            switchNbr++;
            wallmode = !wallmode;
            if(!wallmode && righthand.est_tenu)
            {
                righthand.est_tenu = false;
            }
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
  
    //type : définit l'opération effectuée sur l'indice, 0 - addition, 1 - soustraction
    //incrémente ou décrémente l'indice entre 0 et max
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
    //positions : floats correspondant aux coordonnées z ou x des positions possible pour placer un mur,
    //position : float correspondant à la coordonnée z ou x de la position visée
    //retourne le float le plus proche de position parmi ceux de positions 
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
