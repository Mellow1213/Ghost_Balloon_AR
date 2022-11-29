using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonManager : MonoBehaviour
{
    bool toggleOn = false;
    public GameObject statusPanel;
    public void ToggleStatusPanel()
    {
        toggleOn = !toggleOn;
    }

    public void ChangeCameraScene()
    {
        Debug.Log("카메라 씬 변경");
    }

    private void Update()
    {
        if (toggleOn)
        {
            statusPanel.transform.DOMoveY(1600, 3f).SetEase(Ease.OutQuad);
        }
        else
        {
            statusPanel.transform.DOMoveY(3000, 3f).SetEase(Ease.OutQuad);
        }
    }
}
