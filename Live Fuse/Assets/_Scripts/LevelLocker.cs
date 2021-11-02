using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLocker : MonoBehaviour
{
    public static LevelLocker ll;

    static int _Level01;
    static int _Level02;
    static int _Level03;

    int num;


    void Awake()
    {
        if(ll == null)
            ll = this.GetComponent<LevelLocker>();
    }
    public void UnlockLevel(string levelName)
    {
        Debug.Log("Level name " + levelName);
        switch(levelName)
        {
            case "Level1":
                _Level01 = 1;
                break;
            case "Level2":
                _Level02 = 1;
                break;
            case "Level3":
                _Level03 = 1;
                break;
            default:
                Debug.Log("That is not a current level.");
                break;
        }
    }
    public int GiveNumber(string levelName)
    {
        switch(levelName)
        {
            case "Level1":
                num = _Level01;
                break;
            case "Level2":
                num = _Level02;
                break;
            case "Level3":
                num = _Level03;
                break;
            default:
                Debug.Log("That is not a current level.");
                break;
        }
        return num;
    }
}
