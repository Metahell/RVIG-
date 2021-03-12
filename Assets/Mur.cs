using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mur : MonoBehaviour
{
    // Start is called before the first frame update
    public bool newmur;
    public bool can_place;
    private List<Color> colors = new List<Color>();
    public bool has_child;
    private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
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
    public void canplace()
    {
        foreach (Material material in mesh.materials)
        {
            material.color = Color.red;
        }
    }
    public void cantplace()
    {
        foreach (Material material in mesh.materials)
        {
            material.color = Color.green;
        }
    }
}
