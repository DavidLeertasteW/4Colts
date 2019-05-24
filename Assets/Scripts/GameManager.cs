using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
    
{
    bool gameRunning = false;
    public List<PlayerController> playerControllers;
    public GameObject informationText;
    private int descriptionIndex = 0;
    public Scenario scenario;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerControllers.Add(item.GetComponent<PlayerController>());

        }
        LoadStage();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

        }
        if (Input.GetButton("P1_Special") && Input.GetButton("P1_Fire"))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);

        }
        if (Input.GetButton("P2_Special") && Input.GetButton("P2_Fire"))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);

        }
        if (!gameRunning)
        {
            foreach (var item in playerControllers)
            {
                if (item.menuState != 1)
                {
                    return;
                }


            }
            foreach (var item in playerControllers)
            {
                item.menuState = 2;
                gameRunning = true;
                if (informationText != null)
                {
                    
                }
            }
            StartCoroutine("UpdateDescriptionText");



        }  
    }

    private void LoadStage ()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().backgroundColor = scenario.stageBackground;

        for (int i = 0; i < scenario.players.Count; i++)
        {
            Player temp = scenario.players[i];
            int randomIndex = Random.Range(i, scenario.players.Count);
            scenario.players[i] = scenario.players[randomIndex];
            scenario.players[randomIndex] = temp;
        }
        for (int i = 0; i < scenario.players.Count; i++)
        {
            AssignPlayer(i, scenario.players[i]); 
        }






    }

    void AssignPlayer(int playerObjectIndex, Player player)
    {
        playerControllers[playerObjectIndex].playerName = player.playerNames[Random.Range(0, player.playerNames.Length - 1)];
        playerControllers[playerObjectIndex].accuracyIncreasePerSec = player.accuracyIncreasePerSecond;
        playerControllers[playerObjectIndex].initialAccuracy = player.initialAccuracy;
        playerControllers[playerObjectIndex].accuracyLossperShot = player.accuracyLossPerShot;
        playerControllers[playerObjectIndex].maxAccuracy = player.maxAccuracy;
        playerControllers[playerObjectIndex].maxAmmoinMag = player.maxAmmoinMag;
        playerControllers[playerObjectIndex].ammo = player.ammo;
        playerControllers[playerObjectIndex].SetAmmoInMag();
        playerControllers[playerObjectIndex].transform.Find("QrCode").gameObject.GetComponent<Image>().sprite = player.qrCode;

        
    }

    IEnumerator UpdateDescriptionText ()
    {
        Debug.Log("RunningEnum");

        while (descriptionIndex < scenario.desciriptions.Length)
        {
            informationText.GetComponent<TextMeshProUGUI>().text = scenario.desciriptions[descriptionIndex];
            
            
            float waitfor = Mathf.Sqrt(  scenario.desciriptions[descriptionIndex].Length);
            Debug.Log(waitfor);
            descriptionIndex++;
            yield return new WaitForSeconds(waitfor);
            





        }
        informationText.SetActive(false);


    }

}
