using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meuble : MonoBehaviour
{
    public bool can_place;
    public bool mural;
    public bool sur_un_mur;
    private List<Color> colors = new List<Color>();
    private Transform pour_mural;
    public bool has_child;
    private MeshRenderer mesh;
    // Start is called before the first frame update
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
