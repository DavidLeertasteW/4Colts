using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
    
{
    public List<PlayerController> playerControllers;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerControllers.Add(item.GetComponent<PlayerController>());

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
