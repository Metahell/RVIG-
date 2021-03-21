using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meuble : MonoBehaviour
{
    public bool can_place;    //détermine si le meuble peut être placé (ie pas de collision)

    public bool mural;    //détermine si l'objet est à accrocher au mur (déterminé de base)

    public bool sur_un_mur;    //gère l'état de l'objet mural

    private List<Color> colors = new List<Color>();    //liste des couleurs des materials de l'objet

    public bool has_child;  //si l'objet sur lequel est ce script a un enfant ou non, déterminé de base. Les meubles avec enfant avaient des problèmes au niveau de la position du pivot qui se répercutait sur les rotations.

    private MeshRenderer mesh; //mesh de l'asset meuble

    // Start is called before the first frame update
    //initialisation des booléens et sauvegarde des materials respectifs des objets
    void Start()
    {
        can_place = !mural; //si le meuble est destiné à être accroché sur un mur on ne peut pas le placer tout de suite
        sur_un_mur = false; //pas sur un mur par défaut
        if (has_child)  //le mesh qui nous intéresse est celui de l'enfant si l'objet en a un
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
