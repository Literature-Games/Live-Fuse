using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPhotoSwap : MonoBehaviour
{
    public List<Image> originals = new List<Image>();
    public List<Sprite> replacements = new List<Sprite>();

    //private variables below
    int L1Complete;
    int L2Complete;
    int L3Complete;

    void Awake()
    {
        swapImage();
    }

    void Start()
    {
        swapImage();
    }

    void swapImage()
    {
        if(LevelLocker.ll)
        {
            L1Complete = LevelLocker.ll.GiveNumber("Level1");
            L2Complete = LevelLocker.ll.GiveNumber("Level2");
            L3Complete = LevelLocker.ll.GiveNumber("Level3");
        }
        if(L1Complete == 1)
            originals[0].sprite = replacements[0];
        if(L2Complete == 1)
            originals[1].sprite = replacements[1];
        if(L3Complete == 1)
            originals[2].sprite = replacements[2];
    }
}
