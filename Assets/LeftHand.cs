using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftHand : MonoBehaviour
{
    public GameObject catalogue;    //Le catalogue contenant tous les meubles

    [SerializeField]
    private GameObject Reset;    //Bouton reset qui apparaît ou disparait selon l'input du joueur (aide affichée ou non) 

    private float Testtimer = 0.0F;
    private bool TestStarted = false;
    private float FinalValue;

    public bool activateCatalog=false;
    [SerializeField]
    private Canvas Help;    //Affichage d'aide qui apparaît ou disparait selon l'input du joueur (bouton reset affichée ou non) 

    private bool vue_maquette;    //définit si le joueur est en vue maquette ou non

    public OVRPlayerController controller;  //controller du joueur

    //positions par défaut du mode maquette ou normal, respectivement
    public Transform position_maquette;
    public Transform position_defaut;

    private bool off;    //booléen limitant le nombre de transitions entre les vues maquette et normal

    private bool helpbool=true;    //gère l'affichage d'aide

    //constantes de temps entre chaque transition
    private float time;
    private float timer;

    // Start is called before the first frame update
    //initialisation des timer et booléens
    void Start()
    {
        //Initialisation des timers
        time = 0f;
        timer = 0.1f;

        catalogue.SetActive(false); //catalogue désactivé par défaut
        vue_maquette = false;   //le joueur est en vue normale
    }

    // Update is called once per frame
    //gère les inputs de la manette gauche pour l'affichage du catalogue, la vue maquette (limitée par un timer) et l'affichage de l'aide
    void Update()
    {
        if(TestStarted) Testtimer += Time.deltaTime;
        if (!WallTest.wallmode){
            if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))   //affchage catalogue
            {
                activateCatalog = !catalogue.activeInHierarchy;
                catalogue.SetActive(activateCatalog);
            }
        }
        if(WallTest.wallmode && activateCatalog)
        {
            activateCatalog = !catalogue.activeInHierarchy;
            catalogue.SetActive(activateCatalog);
        }
        if (OVRInput.GetDown(OVRInput.Button.Three) || Input.GetMouseButtonDown(1)) //changement de vue normale/maquette
        {
            if (Testtimer==0 && !TestStarted)
            {
                TestStarted = true;
            }
            else
            {
                Testtimer = FinalValue;
            }
            /*controller.enabled = false;
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
            */
        }
        if (OVRInput.GetDown(OVRInput.Button.Four)) //affichage menu d'aide/bouton resset
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
        if (off)    //désactive le controller du joueur pendant 0.1 secondes après changement de vue pour éviter des téléportations imprévues
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
