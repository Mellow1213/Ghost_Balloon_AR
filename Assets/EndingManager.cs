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
            Debug.Log("��� ����");
        else if (affection > 75 && affection < 92)
            Debug.Log("�븻 ����. �б��� �������� ����");
        else if(affection >= 92)
        {
            Debug.Log("������ ���� ����. ��ٸ��� �б��� ����");
        }
    }
}
