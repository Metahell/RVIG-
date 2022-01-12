using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RightHand : MonoBehaviour
{
    public GameObject[] meubles;    //liste des meubles disponibles
    public Transform TTable;
    public Transform TChaise;
    public Transform TArmoire;
    public Transform TCommode;
    public Transform TPlanteHaute;
    public Transform TPlanteLarge;

    private GameObject meuble;    //meuble tenu par le joueur

    public bool est_tenu;    //booléen déterminant si le joueur tient un meuble

    public Text text;   //texte de log pour le debug

    public OVRPlayerController controller;  //controller du joueur

    private float rotation;     // pour la rotation selon l'axe y des meubles

    public GameObject pour_mural;   //objet vide placeholder gardant la position standard d'un objet mural selectionné

    // Start is called before the first frame update
    //initialisation de pour_mural, est_tenu rotation
    void Start()
    {
        //pour_mural est initialisé devant la main droite du joueur
        pour_mural = Instantiate(new GameObject("pour_mural"), transform.position + transform.forward * 0.5f, Quaternion.identity);
        pour_mural.transform.parent = transform;

        est_tenu = false; //aucun mur tenu par défaut
        rotation = 0;   //rotation nulle au départ
    }

    // Update is called once per frame
    //gère les inputs de la main droite
    void Update()
    {
        RaycastHit hit;
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetMouseButtonDown(0)) //gère la gachette main droite
        {
            if (est_tenu && meuble.GetComponent<Meuble>().can_place)    //pose le meuble
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
                if (Physics.Raycast(transform.position, transform.forward, out hit) && !est_tenu)   //regarde si le joueur a visé un meuble ou le bouton reset
                {
                    if (hit.transform.gameObject.CompareTag("reset"))
                    {
                        hit.transform.gameObject.GetComponent<ResetButton>().Reset();
                    }
                    bool new_meuble = false;    //indique si le joueur créé un nouveau meuble du catalogue ou non
                    foreach (GameObject meuble_c in meubles)    //vérifie si le joueur vise un meuble du catalogue
                    {
                        if (meuble_c.name == hit.transform.gameObject.name)
                        {
                            meuble = Instantiate(meuble_c, transform.position + transform.forward * 0.5f, Quaternion.identity);

                            Rigidbody rigi = meuble.AddComponent<Rigidbody>();
                            rigi.useGravity = false;
                            rigi.isKinematic = true;
                            rigi.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                            meuble.transform.parent = transform;
                            text.text = meuble.name;
                            est_tenu = true;
                            new_meuble = true;
                            if (meuble.GetComponent<Meuble>().mural)    //empèche les Raycast de prendre en compte le meuble s'il est destiné à sa placer sur un mur
                            {
                                meuble.layer = 2;
                            }
                            break;
                        }
                    }
                    if (!new_meuble)    //le joueur n'as pas créé de nouveau meuble
                    {
                        if (hit.transform.gameObject.tag == "meuble")   //le joueur vise un meuble déjà existant
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
        if (est_tenu)   //le joueur tient un meuble
        {
            Meuble meuble_script = meuble.GetComponent<Meuble>();
            meuble_script.sur_un_mur = false;
            if (meuble_script.mural)    //le meuble tenu se place sur un mur
            {
                if (Physics.Raycast(transform.position,transform.forward,out hit))//vérifie si le joueur vise un mur
                {
                    if (hit.transform.gameObject.CompareTag("mur")) //déplace le meuble sur le mur visé
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
            }

            Vector3 lookat = transform.position;
            lookat.y = meuble.transform.position.y;
            if (!meuble.GetComponent<Meuble>().sur_un_mur)// le meuble fait face au joueur
            {
                meuble.transform.LookAt(lookat);
            }

            Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);  //mouvement du joystick droit

            rotation -= secondaryAxis.x * 2;
            if (!meuble.GetComponent<Meuble>().mural)   //désactive la rotation du joueur, sauf si le meuble tenu va sur un mur
            {
                controller.EnableRotation = false;
            }
            if (OVRInput.Get(OVRInput.Button.Two))  //éloigne le meuble du joueur
            {
                pour_mural.transform.position += transform.forward / 50;    //pour sauvegarder la position d'un meuble mural (s'il doit se placer sur un mur puis revenir devant la main du joueur)
                if (!meuble.GetComponent<Meuble>().sur_un_mur)  //ne déplace pas le meuble s'il est mural
                {
                    meuble.transform.position += transform.forward / 50;
                }
            }

            if (OVRInput.Get(OVRInput.Button.One))  //rapproche le meuble du joueur
            {
                pour_mural.transform.position -= transform.forward / 50;
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {
                    meuble.transform.position -= transform.forward / 50;
                }
            }

            if ((pour_mural.transform.position - transform.position).magnitude > 1f)    //limite lointaine de la sauvegarde de position pour les meubles muraux
            {
                pour_mural.transform.position = transform.position + transform.forward;
            }

            if ((pour_mural.transform.position - transform.position).magnitude < 0.2f)  //limite proche de la sauvegarde de position pour les meubles muraux
            {
                pour_mural.transform.position = transform.position + transform.forward * 0.2f;
            }

            if ((meuble.transform.position - transform.position).magnitude > 1f)    //limite proche du meuble
            {
                if (!meuble.GetComponent<Meuble>().sur_un_mur)  //rapprocher le meuble sauf s'il placé sur un mur
                {

                    meuble.transform.position = transform.position + transform.forward;
                }
            }
            if ((meuble.transform.position - transform.position).magnitude < 0.2f)  //éloigner le meuble sauf s'il est sur un mur
            {
                if (!meuble.GetComponent<Meuble>().sur_un_mur)
                {
                    meuble.transform.position = transform.position + transform.forward * 0.2f;
                }
            }
            if (!meuble.GetComponent<Meuble>().mural)   //tourne le meuble conformément au mouvement du joystick droit, sauf si le meuble est mural
            {
                meuble.transform.Rotate(0, rotation, 0);
            }
            switch (meuble.name)
            {
                case "Armoire(Clone)":
                    if (Vector3.Distance(meuble.transform.position, TArmoire.position) < 0.1f)
                    {
                        meuble.transform.position = TArmoire.position;
                        meuble.transform.rotation = TArmoire.rotation;
                    }
                    break;
                case "OfficeChair_01_snaps001(Clone)":
                    if (Vector3.Distance(meuble.transform.position, TChaise.position) < 0.1f)
                    {
                        meuble.transform.position = TChaise.position;
                        meuble.transform.position += new Vector3(0, 0.01f, 0);
                        meuble.transform.rotation = TChaise.rotation;
                    }
                    break;
                case "Cabinet_02_snaps001(Clone)":
                    if (Vector3.Distance(meuble.transform.position, TCommode.position) < 0.1f)
                    {
                        meuble.transform.position = TCommode.position;
                        meuble.transform.position += new Vector3(0, 0.01f, 0);
                        meuble.transform.rotation = TCommode.rotation;
                    }
                    break;
                case "PotPlant_01_snaps001(Clone)":
                    if (Vector3.Distance(meuble.transform.position, TPlanteHaute.position) < 0.1f)
                    {
                        meuble.transform.position = TPlanteHaute.position;
                        meuble.transform.position += new Vector3(0, 0.01f, 0);
                        meuble.transform.rotation = TPlanteHaute.rotation;
                    }
                    break;
                case "PotPlant_02_snaps001(Clone)":
                    if (Vector3.Distance(meuble.transform.position, TPlanteLarge.position) < 0.1f)
                    {
                        meuble.transform.position = TPlanteLarge.position;
                        meuble.transform.position += new Vector3(0, 0.01f, 0);
                        meuble.transform.rotation = TPlanteLarge.rotation;
                    }
                    break;
                case "ComputerDesk_01_snaps001(Clone)":
                    if (Vector3.Distance(meuble.transform.position, TTable.position) < 0.1f)
                    {
                        meuble.transform.position = TTable.position;
                        meuble.transform.position += new Vector3(0, 0.01f, 0);
                        meuble.transform.rotation = TTable.rotation;
                    }
                    break;
                default: break;
            }
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) || Input.GetMouseButtonDown(1))  //supprime le meuble
            {
                Destroy(meuble);
                est_tenu = false;
                controller.EnableRotation = true;
            }
        }
    }
    void Print(string s)    //fonction d'affichage pour le debug
    {
        text.text += s;
    }

    void Clear()    //fonction d'affichage pour le debug
    {
        text.text = "";
    }

}
