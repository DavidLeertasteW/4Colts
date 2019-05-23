using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Player", menuName = "Levels/Player", order = 2)]
public class Player : ScriptableObject
{
    public string[] playerNames = new string[] { };
    public int ammo = 6;
    public Sprite qrCode;

    public float initialAccuracy = 30, maxAccuracy = 90, accuracyIncreasePerSecond = 15, accuracyLossPerShot = 15;

}
