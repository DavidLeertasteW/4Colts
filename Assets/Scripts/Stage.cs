using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Stage", menuName = "Levels/Stage", order = 0)]
public class Stage : ScriptableObject
{
    public string headline = "Last Gunslinger";
    public Sprite sprite;
    [TextArea(5, 10)]
    public string descriptionText = "Enter neat descritpion here";
    public int sceneIndex;
    
}
