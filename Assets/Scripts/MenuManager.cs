using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Stage[] stages = new Stage[] { };
    public Image image;
    public TextMeshProUGUI headlineText, descriptionText;
    public Transform[] leftParticleSpawners = new Transform[] { };
    public Transform[] rightParticleSpawners = new Transform[] { };
    public GameObject particleObject;

    private int currentIndex = 0;

    bool sceneConfirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        Navigate(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!sceneConfirmed)
        {
            //P1
            if (Input.GetButtonDown("P1_Vertical") && Input.GetAxis("P1_Vertical") < 0)
            {
                Navigate(-1);
            }
            if (Input.GetButtonDown("P1_Vertical") && Input.GetAxis("P1_Vertical") > 0)
            {
                Navigate(1);
            }
            //P2
            if (Input.GetButtonDown("P2_Vertical") && Input.GetAxis("P2_Vertical") < 0)
            {
                Navigate(-1);
            }
            if (Input.GetButtonDown("P2_Vertical") && Input.GetAxis("P2_Vertical") > 0)
            {
                Navigate(1);
            }
            //P3
            if (Input.GetButtonDown("P3_Vertical") && Input.GetAxis("P3_Vertical") < 0)
            {
                Navigate(-1);
            }
            if (Input.GetButtonDown("P3_Vertical") && Input.GetAxis("P3_Vertical") > 0)
            {
                Navigate(1);
            }
            //P4
            if (Input.GetButtonDown("P4_Vertical") && Input.GetAxis("P4_Vertical") < 0)
            {
                Navigate(-1);
            }
            if (Input.GetButtonDown("P4_Vertical") && Input.GetAxis("P4_Vertical") > 0)
            {
                Navigate(1);
            }
        }
        if(Input.GetButtonDown("P1_Fire")|| Input.GetButtonDown("P2_Fire")|| Input.GetButtonDown("P3_Fire")|| Input.GetButtonDown("P4_Fire"))
        {
            StartCoroutine("LoadStageAfterEffect");
        }
        if(Input.GetButtonDown("P1_Special")|| Input.GetButtonDown("P2_Special")|| Input.GetButtonDown("P3_Special")|| Input.GetButtonDown("P4_Special"))
        {
            StopAllCoroutines();
            sceneConfirmed = false;
        }

    }

    void Navigate (int change)
    {
        currentIndex += change;

        if(currentIndex < 0 || currentIndex >= stages.Length)
        {
            GameObject.FindObjectOfType<AudioManager>().PlaySFX("Empty");
            currentIndex = Mathf.Clamp(currentIndex, 0, stages.Length - 1);
            return;
        }
        UpdateStage(stages[currentIndex]);
        GameObject.FindObjectOfType<AudioManager>().PlaySFX("Reload_Bullet");
        SpawnParticles(change);


    }

    void UpdateStage (Stage stage)
    {
        if(stage.image == null)
        {
            image.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        }else
        {
            image = stage.image;
        }
        
        headlineText.SetText(stage.headline);
        descriptionText.SetText(stage.descriptionText);

    }

    void SpawnParticles (int direction = 0)
    {
        if (direction == -1 || direction == 0)
        {
            foreach (var item in leftParticleSpawners)
            {
                Transform tmpTrans = GameObject.Instantiate(particleObject).GetComponent<Transform>();
                tmpTrans.rotation = Quaternion.Euler(0, 0, -90);
                tmpTrans.position = item.position;
            }
        }
        if (direction == 1 || direction == 0)
        {
            foreach (var item in rightParticleSpawners)
            {
                Transform tmpTrans = GameObject.Instantiate(particleObject).GetComponent<Transform>();
                tmpTrans.rotation = Quaternion.Euler(0, 0, 90);
                tmpTrans.position = item.position;
                
            }
        }

    }

    IEnumerator LoadStageAfterEffect ()
    {
        GameObject.FindObjectOfType<AudioManager>().PlaySFX("Shot Hit");
        sceneConfirmed = true;
        SpawnParticles(0);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(stages[currentIndex].sceneIndex, LoadSceneMode.Single);

    }
}
