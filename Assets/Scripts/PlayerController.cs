using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName = "player01";
    int position;
    public bool ready = false;
    public bool dead = false;
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
    Vector2 aimingDirection;
    private Animator menuAnimator;
   

   // private Vector3 originalPositionQrCode;

    // Start is called before the first frame update
    void Start()
    {
        menuAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(menuState < 2)
        {
            MenuInputHandler();
        }else if(menuState == 3)
        {
            menuAnimator.SetInteger("MenuState", menuState);
            menuState++;
            
        }else if (menuState == 4)
        {
            InputHandler();
        }
        Bleeding();
        
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
            //Debug.Log("Hitchance: " + accuracy);
            float tmpRandomValue = Random.Range(0f, 100f);
            if (tmpRandomValue <= accuracy)
            {
                
                Debug.Log("hit");

                foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
                {
                    
                    if(item.name == player)
                    {
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
                            tmpV3.x *= 2.5f;
                        }else
                        {
                            tmpV3 = item.GetComponent<PlayerController>().animator.gameObject.transform.position;
                            tmpV3.x += Random.Range(-2f, 2f);
                            tmpV3.y *= 2.25f;
                        }

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
            menuAnimator.SetInteger("MenuState", menuState);

        }
        if(menuState == 1)
        {
            aimingDirection.y = -Input.GetAxis(gameObject.name + "_Horizontal");
            aimingDirection.x = Input.GetAxis(gameObject.name + "_Vertical");
            //Debug.Log("Aims at: " + aimingDirection);
            if (qrCode == null)
            {
                qrCode = transform.GetChild(0).gameObject;
            }
            qrCode.transform.localScale = Vector3.one * Mathf.Clamp01(aimingDirection.magnitude);
            qrCode.gameObject.GetComponent<RectTransform>().anchoredPosition = aimingDirection * 25; 
        }


    }

    void Bleeding ()
    {
        health -= woundCount * bleedingPerWound * Time.deltaTime;
        if(health <= 0)
        {
            Destroy(animator.gameObject);
            Destroy(gameObject);


        }



    }
}
