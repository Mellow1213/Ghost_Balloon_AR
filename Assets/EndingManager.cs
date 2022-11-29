using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public Balloon _balloon;
    float affection;
    // Update is called once per frame
    void Update()
    {
        affection = _balloon.getAffection();
        if(affection >= 95)
        {
            SceneManager.LoadScene(2);
            Debug.Log("진엔딩 등장 가능. 기다리면 분기점 등장");
        }
    }
}
