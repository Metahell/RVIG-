using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mur : MonoBehaviour
{
    // Start is called before the first frame update
    public bool newmur=true;
    public bool isindic = false;
    public bool can_place=true;
    private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        if (isindic)
        {
            mesh = GetComponent<MeshRenderer>();
            canplace();
        }
    }

    public void canplace()
    {
        foreach (Material material in mesh.materials)
        {
            material.color = Color.green;
        }
    }
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
