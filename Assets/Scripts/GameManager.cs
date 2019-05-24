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

    public Scenario[] alternativeScenarios = new Scenario[] { };
    private float timeofLastSpawn = 0;

    private bool firstTimeObjectSpawn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if(alternativeScenarios.Length > 0)
        {
            int index = Random.Range(0, alternativeScenarios.Length+1);
            index = Mathf.Clamp(index, 0, 2);
            if(index == alternativeScenarios.Length)
            {
                
            }else
            {
                scenario = alternativeScenarios[index];
            }
            Debug.Log(index);
        }
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
            SceneManager.LoadScene(0, LoadSceneMode.Single);

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

            timeofLastSpawn = Time.time;

        }

        if (gameRunning)
        {
            
            if(scenario.objectSpawnedAtFrequency != null || scenario.ammoAtInterval != 0)
            {
                if (firstTimeObjectSpawn)
                {
                    if (Time.time - timeofLastSpawn >= (scenario.spawnAfter/4))
                    {
                        if (scenario.objectSpawnedAtFrequency != null)
                        {
                            GameObject.Instantiate(scenario.objectSpawnedAtFrequency);
                        }
                        foreach (var item in playerControllers)
                        {
                            item.ammo += scenario.ammoAtInterval;
                        }
                        timeofLastSpawn = Time.time;
                        firstTimeObjectSpawn = false;
                        Debug.Log("Ran For the First time");
                    }
                    
                }else
                {
                    if (Time.time - timeofLastSpawn >= scenario.spawnAfter)
                    {
                        if (scenario.objectSpawnedAtFrequency != null)
                        {
                            GameObject.Instantiate(scenario.objectSpawnedAtFrequency);
                        }
                        foreach (var item in playerControllers)
                        {
                            item.ammo += scenario.ammoAtInterval;
                        }
                        timeofLastSpawn = Time.time;
                    }
                }


            }


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
        playerControllers[playerObjectIndex].endAfterthisplayerDied = player.endAfterThisPlayerDied;
        playerControllers[playerObjectIndex].showWhenthisPlayerDies = player.showWhenThisConditionIsMet;
        playerControllers[playerObjectIndex].showWhenSurvived = player.showWhenSurvived;
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
            string tmpText = informationText.GetComponent<TextMeshProUGUI>().text;


            yield return new WaitForSeconds(waitfor);

            if(informationText.GetComponent<TextMeshProUGUI>().text != tmpText)
            {
                Debug.Log("Coroutine Canceled");
                yield break;
            }
            





        }
        informationText.SetActive(false);


    }

}
