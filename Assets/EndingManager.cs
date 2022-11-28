using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public Balloon _balloon;
    float affection;
    // Update is called once per frame
    void Update()
    {
        affection = _balloon.getAffection();
        if (affection <= 10)
            Debug.Log("배드 엔딩");
        else if (affection > 75 && affection < 92)
            Debug.Log("노말 엔딩. 분기점 등장하지 않음");
        else if(affection >= 92)
        {
            Debug.Log("진엔딩 등장 가능. 기다리면 분기점 등장");
        }
    }
}
