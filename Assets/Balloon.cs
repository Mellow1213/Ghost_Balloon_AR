using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    float hapiness = 50;

    bool isWalk = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            prevPos = transform.position;
            isWalk = !isWalk;
        }

        if (isWalk)
            Walk();
    }

    void Eat()
    {

    }



    float walkDistance = 0;
    Vector3 prevPos;
    float timer = 0.1f;
    void Walk()
    {
        timer += Time.deltaTime;
        if(timer > 0.1f)
        {
            walkDistance += Vector3.Distance(prevPos, transform.position);
            Debug.Log("이동 거리 : " + Vector3.Distance(prevPos, transform.position));
            timer = 0f;
            prevPos = transform.position;
        }
        Debug.Log("walkDistance : " + walkDistance);
        Debug.Log("산책 중");
    }

    void Shower()
    {

    }

    void Exercise()
    {

    }

    void Play()
    {

    }

    void Idle()
    {

    }
}
