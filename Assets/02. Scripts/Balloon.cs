using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    float hapiness = 50;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Play();
    }

    void Eat()
    {

    }



    void Walk()
    {
    }

    void Shower()
    {

    }

    void Exercise()
    {

    }

    bool isPlay = false;
    float walkDistance = 0;
    Vector3 prevPos;
    float timer = 0.1f;

    const float playTimeDistance = 100f;
    void Play()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && !isPlay)
        {
            prevPos = transform.position;
            isPlay = true;
        }

        if (isPlay)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                walkDistance += Vector3.Distance(prevPos, transform.position);
                Debug.Log("�̵� �Ÿ� : " + Vector3.Distance(prevPos, transform.position));
                timer = 0f;
                prevPos = transform.position;
            }
            Debug.Log("walkDistance : " + walkDistance);
            Debug.Log("��å ��");

            if(walkDistance >= playTimeDistance)
            {
                walkDistance = 0;
                isPlay = false;
                Debug.Log("����ֱ� ��.");
                Debug.Log("��ſ� ũ�� ����.");
                Debug.Log("û�ᵵ �۰� ����.");
                Debug.Log("������ �߰� ����.");
            }
        }


    }

    void Idle()
    {

    }
}
