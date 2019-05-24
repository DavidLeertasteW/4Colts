using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public string playerName = "player01";
    int position;
    public bool ready = false;
    public bool dead = false;
    public bool isOver = false;
    public GameObject qrCode;
    private string previousTraget = "";
    private float accuracy = 0;
    public float initialAccuracy = 30, accuracyIncreasePerSec = 45, accuracyLossperShot = 15, maxAccuracy = 95;
    public Animator animator;
    public GameObject splatter, splitter;
    public int woundCount = 0;
    private float health = 100;
    public float bleedingPerWound = 5;

    public int menuState = 0;
    public int ammo = 6;
    Vector2 aimingDirection;

    private int ammoInMagazine;
    public int maxAmmoinMag;
    
    AudioManager audioManager;

    private float timeOfLastReload = 0;
    public bool endAfterthisplayerDied;
    public string showWhenthisPlayerDies;
    public string showWhenSurvived;

    
   

   // private Vector3 originalPositionQrCode;

    // Start is called before the first frame update
    void Start()
    {
        animator.gameObject.SetActive(false);
        audioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();
        

        if (qrCode == null)
        {
            qrCode = transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead && !isOver)
        {
            if (menuState < 2)
            {
                MenuInputHandler();
            }
            else if(menuState == 2)
            {
                
                menuState++;
                animator.gameObject.SetActive(true);

                transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = playerName;
                transform.GetChild(2).gameObject.SetActive(false);
            }
            else 
            {
                InputHandler();
            }
             
            Bleeding();
        }
    }

    void InputHandler ()
    {
        aimingDirection.y = -Input.GetAxis(gameObject.name + "_Horizontal");
        aimingDirection.x = Input.GetAxis(gameObject.name + "_Vertical");
        //Debug.Log("Aims at: " + aimingDirection);
        
        if (animator != null)
        {
            animator.SetBool("X", false);
            animator.SetInteger("Y", 0);
        }

        if (aimingDirection.magnitude > 0.1f)
        {

            if (gameObject.name == "P1")
            {
                if (aimingDirection.x == 0 && aimingDirection.y <= -0.5f)
                {
                    AimingAt("P3");
                }
                if (aimingDirection.x >= 0.5f && aimingDirection.y == 0)
                {
                    AimingAt("P2");
                }
                if (aimingDirection.x >= 0.5f && aimingDirection.y <= -0.5f)
                {
                    AimingAt("P4");
                }
            }
            if (gameObject.name == "P2")
            {
                if (aimingDirection.x == 0 && aimingDirection.y <= -0.5f)
                {
                    AimingAt("P4");
                }
                if (aimingDirection.x <= -0.5f && aimingDirection.y == 0)
                {
                    AimingAt("P1");
                }
                if (aimingDirection.x <= -0.5f && aimingDirection.y <= -0.5f)
                {
                    AimingAt("P3");
                }
            }
            if (gameObject.name == "P3")
            {
                if (aimingDirection.x == 0 && aimingDirection.y >= 0.5f)
                {
                    AimingAt("P1");
                }
                if (aimingDirection.x >= 0.5f && aimingDirection.y == 0)
                {
                    AimingAt("P4");
                }
                if (aimingDirection.x >= 0.5f && aimingDirection.y >= 0.5f)
                {
                    AimingAt("P2");
                }
            }
            if (gameObject.name == "P4")
            {
                if (aimingDirection.x == 0 && aimingDirection.y >= 0.5f)
                {
                    AimingAt("P2");

                }
                if (aimingDirection.x <= -0.5f && aimingDirection.y == 0)
                {
                    AimingAt("P3");
                }
                if (aimingDirection.x <= -0.5f && aimingDirection.y >= 0.5f)
                {
                    AimingAt("P1");
                }
            }
        }
        else
        {
            previousTraget = "";
            if(Input.GetButtonDown(gameObject.name + "_Special"))
            {
                Reload();



            }
        }
            

        
        
    }
    private void AimingAt (string player)
    {
        if (animator != null)
        {
            //Debug.Log(aimingDirection.y + "Y");
            // Y dirction
            if (aimingDirection.y > 0)
            {
                animator.SetInteger("Y", 1);
                //animator.SetInteger("Y", 1);
                //Debug.Log("settign Y");
            }
            else if (aimingDirection.y < 0)
            {
                animator.SetInteger("Y", -1);
            }
            else
            {
                animator.SetInteger("Y", 0);
            }
            //X direction
            if (aimingDirection.x > 0)
            {
                animator.SetBool("X", true);

                animator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (aimingDirection.x < 0)
            {
                animator.SetBool("X", true);

                animator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                animator.SetBool("X", false);

                animator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        if (previousTraget == player)
        {
            accuracy += accuracyIncreasePerSec * Time.deltaTime;
        }else
        {
            accuracy = initialAccuracy;
            //Debug.Log("resetting accuracy");
           
        }

        accuracy = Mathf.Clamp(accuracy, initialAccuracy, maxAccuracy );
        


        if(Input.GetButtonDown(gameObject.name + "_Fire"))
        {
            if(ammoInMagazine <= 0)
            {
                audioManager.PlaySFX("Empty");
                return;
            }
            //Debug.Log("Hitchance: " + accuracy);
            float tmpRandomValue = Random.Range(0f, 100f);
            ammoInMagazine -= 1;
            if (tmpRandomValue <= accuracy)
            {
                
                Debug.Log("hit");

                foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
                {
                    
                    if(item.name == player)
                    {
                        audioManager.PlaySFX("Shot Hit");
                        item.GetComponent<PlayerController>().woundCount += 1;
                        GameObject tmpObj =  GameObject.Instantiate(splatter);
                        tmpObj.transform.position = item.GetComponent<PlayerController>().animator.gameObject.transform.position;
                        tmpObj.transform.up = animator.transform.position - tmpObj.transform.position;
                        //tmpObj.transform.eulerAngles = (Vector2.Angle(tmpObj.transform.position, animator.transform.position) * Vector3.forward) - new Vector3(0, 0, 0);
                    }

                }
            }else
            {
                Debug.Log("Missed");
                foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
                {

                    if (item.name == player)
                    {
                        GameObject tmpObj = GameObject.Instantiate(splitter);
                        Vector3 tmpV3 = new Vector3(0, 0, 0);
                        if (Random.Range(0, 100) < 50)
                        {
                            tmpV3 = item.GetComponent<PlayerController>().animator.gameObject.transform.position;
                            tmpV3.y += Random.Range(-1f, 1f);
                            tmpV3.x *= 2f;
                            
                        }else
                        {
                            tmpV3 = item.GetComponent<PlayerController>().animator.gameObject.transform.position;
                            tmpV3.x += Random.Range(-2f, 2f);
                            tmpV3.y *= 1.75f;
                        }
                        audioManager.PlaySFX("Shot Miss");
                        tmpObj.transform.position = tmpV3;
                        tmpObj.transform.up = animator.transform.position - tmpObj.transform.position;
                       // tmpObj.transform.eulerAngles = (Vector2.Angle( animator.transform.position ,tmpObj.transform.position) * Vector3.forward) - new Vector3(0,0,-90);

                    }

                }
            }
            accuracy -= accuracyLossperShot;
            accuracy = Mathf.Clamp(accuracy, initialAccuracy, maxAccuracy);
            if (animator != null)
            {
                animator.SetTrigger("Fire");
            }
        }
        
        
        previousTraget = player;


    }
    void MenuInputHandler ()
    {
        if(Input.GetButtonDown(gameObject.name + "_Fire"))
        {
            menuState += 1;
            menuState = Mathf.Clamp(menuState, 0, 1);

        }
        if (Input.GetButtonDown(gameObject.name + "_Special")) {
            menuState -= 1;
            menuState = Mathf.Clamp(menuState, 0, 1);
        }

            if (menuState == 0)
        {
            aimingDirection.y = -Input.GetAxis(gameObject.name + "_Horizontal");
            aimingDirection.x = Input.GetAxis(gameObject.name + "_Vertical");
            //Debug.Log("Aims at: " + aimingDirection);
            if (qrCode == null)
            {
                qrCode = transform.GetChild(0).gameObject;
            }
            qrCode.gameObject.SetActive(true);
            qrCode.transform.localScale = Vector3.one * Mathf.Clamp01(aimingDirection.magnitude);
            qrCode.gameObject.GetComponent<RectTransform>().anchoredPosition = aimingDirection * 25;
            transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Ready?";
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = gameObject.name;

        }
        else if(menuState == 1)
        {
            transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "READY!";
            qrCode.gameObject.SetActive(false);
            
        }
        else
        {
            
        }


    }

    void Bleeding ()
    {
        int livingPCount = 0;
        foreach (var item in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (!item.dead)
            {
                livingPCount++;
            }
            if(item.dead && item.endAfterthisplayerDied)
            {
                return;
            }

        }
        if (livingPCount == 1)
        {


            //Time.timeScale = 0.01f;
            //transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = playerName + " " + "Survived!";
            GameObject.FindObjectOfType<GameManager>().informationText.SetActive(true);
            if (showWhenSurvived != "")
            {
                GameObject.FindObjectOfType<GameManager>().informationText.GetComponent<TextMeshProUGUI>().text = playerName + " Survived!";
            }else
            {
                GameObject.FindObjectOfType<GameManager>().informationText.GetComponent<TextMeshProUGUI>().text = showWhenSurvived;
            }
            
            
            transform.GetChild(1).gameObject.SetActive(false);
            Destroy(this);
            return;
        }
        
        if(ammo == 0 && ammoInMagazine == 0)
        {
            bool isOver = true;
            foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (item.GetComponent<PlayerController>().ammo > 0 || item.GetComponent<PlayerController>().ammoInMagazine > 0)
                {
                    isOver = false;
                }
                if (item.GetComponent<PlayerController>().woundCount > 0)
                {
                    isOver = false;
                }

            }
            if (isOver)
            {
                //Time.timeScale = 0.01f;
                transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Its Over!";
                Destroy(this);
                return;
            }
        }
        //health -= (woundCount * woundCount * 2) * bleedingPerWound * Time.deltaTime;
        health = 100 -( woundCount * 50);
        if(health <= 0)
        {
            
            animator.SetBool("IsDead", true);
            //Destroy(gameObject);
            dead = true;
            transform.GetChild(1).gameObject.SetActive(false);
            if (endAfterthisplayerDied)
            {
                foreach (var item in GameObject.FindObjectsOfType<PlayerController>())
                {
                    item.isOver = true;
                    item.transform.GetChild(1).gameObject.SetActive(false);
                }
                GameObject.FindObjectOfType<GameManager>().informationText.SetActive(true);
                GameObject.FindObjectOfType<GameManager>().informationText.GetComponent<TextMeshProUGUI>().text = showWhenthisPlayerDies;

            }

        }



    }
    public void SetAmmoInMag()
    {

        if(ammo < maxAmmoinMag)
        {
            ammoInMagazine = ammo;
            ammo = 0;
        }else
        {
            ammo -= maxAmmoinMag;
            ammoInMagazine = maxAmmoinMag;
        }

        


    }
    void Reload()
    {
        if (timeOfLastReload < Time.time - 0.3f)
        {
            if (ammo > 0 && ammoInMagazine < maxAmmoinMag)
            {
                ammoInMagazine++;
                ammo--;
                audioManager.PlaySFX("Reload_Bullet");
                if (ammoInMagazine == maxAmmoinMag)
                {
                    audioManager.PlaySFX("Reload_Full");
                }


            }
            else if (ammoInMagazine >= maxAmmoinMag)
            {
                audioManager.PlaySFX("No_Reload_Because_Full");


            }
            else if (ammo <= 0)
            {
                audioManager.PlaySFX("No_Reload_Because_No_Bullets");
            }
            timeOfLastReload = Time.time;

        }
    }
}
