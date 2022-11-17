using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWalls : MonoBehaviour
{
    public GameObject straightWalls1;
    public GameObject straightWalls2;
    public GameObject horizontalWalls1;
    public GameObject horizontalWalls2;
    
    // Start is called before the first frame update
    void Start()
    {
        horizontalWalls1.SetActive(false);
        horizontalWalls2.SetActive(false);
    }

    void OnCollisionEnter()
    {
        Debug.Log("Set the trap!");
        straightWalls1.SetActive(false);
        straightWalls2.SetActive(false);
        horizontalWalls1.SetActive(true);
        horizontalWalls2.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
