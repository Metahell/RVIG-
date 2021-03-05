using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meuble : MonoBehaviour
{
    public bool can_place;
    public bool mural;
    private List<Color> colors = new List<Color>();
    // Start is called before the first frame update
    void Start()
    {
        can_place = true;
        foreach (Material material in gameObject.GetComponent<MeshRenderer>().materials)
        {
            colors.Add(material.color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mural)
        {
            Collider[] hit_colliders = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (Collider col in hit_colliders)
            {
                if (col.transform.gameObject.CompareTag("mur"))
                {
                    Vector3 mur_position = col.ClosestPoint(transform.position);
                    transform.position = mur_position;
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!mural)
        {
            can_place = false;
            Debug.Log("collision");
            foreach (Material material in gameObject.GetComponent<MeshRenderer>().materials)
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
            foreach (Material material in gameObject.GetComponent<MeshRenderer>().materials)
            {
                material.color = colors[index];
                index++;
            }
        }
    }
}
