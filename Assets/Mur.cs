using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mur : MonoBehaviour
{
    // Start is called before the first frame update
    public bool newmur;
    public bool can_place=true;
    private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        canplace();
    }
    private void Awake()
    {
        canplace();
    }
    // Update is called once per frame
    void Update()
    {
        if (can_place)
        {
            canplace();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        cantplace();
        can_place = false;
    }
    private void OnTriggerStay(Collider other)
    {
        cantplace();
        can_place = false;
    }
    private void OnTriggerExit(Collider other)
    {
        canplace();
        can_place = true;
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
}
