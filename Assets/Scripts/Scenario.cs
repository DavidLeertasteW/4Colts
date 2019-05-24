using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Scenario", menuName = "Levels/Scenario", order = 1)]
public class Scenario : ScriptableObject
{
    
    
    public List<Player> players = new List<Player>();
    public Color stageBackground;
    [TextArea(5,10)]
    public string [] desciriptions = new string[] { };

    public GameObject objectSpawnedAtFrequency;
    public float spawnAfter = 60;
    public int ammoAtInterval = 0;

   
}
