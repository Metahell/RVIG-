using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meuble : MonoBehaviour
{
    //détermine si le meuble peut être placé (ie pas de collision)
    public bool can_place;
    //détermine si l'objet est à accrocher au mur (déterminé de base)
    public bool mural;
    //gère l'état de l'objet mural
    public bool sur_un_mur;
    //liste des couleurs des materials de l'objet
    private List<Color> colors = new List<Color>();
    private Transform pour_mural;
    public bool has_child;
    private MeshRenderer mesh;
    // Start is called before the first frame update
    //initialisation des booléens et sauvegarde des materials respectifs des objets
    void Start()
    {
        can_place = !mural;
        sur_un_mur = false;
        if (has_child)
        {
            mesh = GetComponentInChildren<MeshRenderer>();
        }
        else
        {
            mesh = GetComponent<MeshRenderer>();
        }
        foreach (Material material in mesh.materials)
        {
            colors.Add(material.color);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    //collision : le meuble devient rouge
    void OnTriggerEnter(Collider other)
    {
        if (!mural)
        {
            can_place = false;
            Debug.Log("collision");

            foreach (Material material in mesh.materials)
            {
                material.color = Color.red;
            }
        }
    }
    //fin de collision : le meuble reprend ses materials initiaux
    void OnTriggerExit(Collider other)
    {
        if (!mural)
        {
            can_place = true;
            Debug.Log("collision sortie");
            int index = 0;
            foreach (Material material in mesh.materials)
            {
                material.color = colors[index];
                index++;
            }
        }
    }
}
