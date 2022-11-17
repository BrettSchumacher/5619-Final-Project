using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEndToggler : MonoBehaviour
{
    public GameObject deadEndFront;
    public GameObject deadEndBack;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("deactivated wall");
        deadEndBack.SetActive(false);
        
    }

    void OnTriggerEnter(Collider other)
    {
        //activate wall preventing from going back
        Debug.Log("activate wall? " + other.gameObject.name);
        if (other.gameObject.name == "OVRWrapper")
        {
            deadEndBack.SetActive(true);

            //GetComponent<Collider>().enabled = false;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "OVRWrapper")
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
