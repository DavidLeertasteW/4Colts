using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
    
{
    bool gameRunning = false;
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
        if (!gameRunning)
        {
            foreach (var item in playerControllers)
            {
                if (item.menuState != 2)
                {
                    return;
                }


            }
            foreach (var item in playerControllers)
            {
                item.menuState = 3;
                gameRunning = true;

            }
            


        }  
    }

}
