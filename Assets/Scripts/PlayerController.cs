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

    Vector2 aimingDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
    }

    void InputHandler ()
    {
        aimingDirection.x = Input.GetAxis(gameObject.name + "_Horizontal");
        aimingDirection.y = Input.GetAxis(gameObject.name + "_Vertical");
        Debug.Log("Aims at: " + aimingDirection);
        if(qrCode == null)
        {
            qrCode = transform.GetChild(0).gameObject;
        }
        qrCode.transform.localScale = Vector3.one * Mathf.Clamp01( aimingDirection.magnitude);
    }
}
