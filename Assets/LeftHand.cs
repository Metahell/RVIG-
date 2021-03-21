using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftHand : MonoBehaviour
{
    //Le catalogue contenant tous les meubles
    public GameObject catalogue;
    //Bouton reset qui apparaît ou disparait selon l'input du joueur (aide affichée ou non) 
    [SerializeField]
    private GameObject Reset;
    //Affichage d'aide qui apparaît ou disparait selon l'input du joueur (bouton reset affichée ou non) 
    [SerializeField]
    private Canvas Help;
    //définit si le joueur est en vue maquette ou non
    private bool vue_maquette;
    public OVRPlayerController controller;
    //positions par défaut du mode maquette ou normal, respectivement
    public Transform position_maquette;
    public Transform position_defaut;
    //booléen gérant limitant le nombre de transitions entre les vues maquette et normal
    private bool off;
    //gère l'affichage d'aide
    private bool helpbool=true;
    //constantes de temps entre chaque transition
    private float time;
    private float timer;
    // Start is called before the first frame update
    //initialisation des timer et booléens
    void Start()
    {
        time = 0f;
        timer = 0.1f;
        catalogue.SetActive(false);
        vue_maquette = false;
    }

    // Update is called once per frame
    //gère les inputs de la manette gauche pour l'affichage du catalogue, la vue maquette (limitée par un timer) et l'affichage de l'aide
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            catalogue.SetActive(!catalogue.activeInHierarchy);
        }
        if (OVRInput.GetDown(OVRInput.Button.Three) || Input.GetMouseButtonDown(1))
        {
            controller.enabled = false;
            off = true;
            time = 0f;
            if (!vue_maquette)
            {
                controller.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                controller.transform.position = position_maquette.position;
                controller.transform.rotation = position_maquette.rotation;
                vue_maquette = true;
            }
            else
            {
                controller.transform.localScale = new Vector3(1, 1, 1);
                controller.transform.position = position_defaut.position;
                controller.transform.rotation = position_defaut.rotation;
                vue_maquette = false;
            }

        }
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            if (!helpbool)
            {
                Help.enabled = true;
                helpbool = true;
                Reset.SetActive(false);
            }
            else
            {
                Help.enabled = false;
                helpbool = false;
                Reset.SetActive(true);
            }

        }
        if (off)
        {
            time += Time.deltaTime;
            if (time > timer)
            {
                controller.enabled = true;
                off = false;
                time = 0f;
            }
        }
    }
}
