using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mur : MonoBehaviour
{
    // Start is called before the first frame update
    //empêche de supprimer les murs originels de la maquette
    public bool newmur=true;
    //différencie les murs indicateurs des murs permanents
    public bool isindic = false;
    //conditionne la pose des murs
    public bool can_place=true;
    private MeshRenderer mesh;
    // Start is called before the first frame update
    //Le mur indicateur prend la couleur verte à l'initialisation
    void Start()
    {
        if (isindic)
        {
            mesh = GetComponent<MeshRenderer>();
            canplace();
        }
    }
    // le mur prend la couleur verte
    public void canplace()
    {
        foreach (Material material in mesh.materials)
        {
            material.color = Color.green;
        }
    }
    //le mur prend le couleur rouge
    public void cantplace()
    {
        foreach (Material material in mesh.materials)
        {
            material.color = Color.red;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isindic)
        {
            if (can_place)
            {
                canplace();
            }
        }
    }
    //collision : mur prend la couleur rouge, can_place devient false
    private void OnTriggerEnter(Collider other)
    {
        if (isindic)
        {
            if (!other.CompareTag("sol"))
            {
                cantplace();
                can_place = false;
            }
        }
    }
    //fin de collision : mur indicateur devient vert, can_place devient true
    private void OnTriggerExit(Collider other)
    {
        if (isindic)
        {
            if (!other.CompareTag("sol"))
            {
                canplace();
                can_place = true;
            }

        }
    }
    
}
