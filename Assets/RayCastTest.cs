using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    public string la_main;
    private float timer;
    private float ping;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        ping = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (timer > ping) {
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Debug.Log(la_main + " hit objet en " +hit.transform.position);
        }
            timer = 0f;
        }
        timer += Time.deltaTime;
    }
}
