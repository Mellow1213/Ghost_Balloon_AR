using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{
    public GameObject blink;             // ���� ���� �� ������ ��

    bool isCoroutinePlaying;             // �ڷ�ƾ �ߺ�����

    // ���� �ҷ��� �� �ʿ�
    string albumName = "Ghost";           // ������ �ٹ��� �̸�


    // ĸ�� ��ư�� ������ ȣ��
    public void Capture_Button()
    {
        // �ߺ����� bool
        if (!isCoroutinePlaying)
        {
            StartCoroutine("captureScreenshot");
        }
    }

    IEnumerator captureScreenshot()
    {
        isCoroutinePlaying = true;

        yield return new WaitForEndOfFrame();

        // ��ũ���� + ����������
        ScreenshotAndGallery();

        yield return new WaitForEndOfFrame();

        // ��ũ
        blink.SetActive(true);

        yield return new WaitForSecondsRealtime(0.3f);

        blink.SetActive(false);

        isCoroutinePlaying = false;
    }

    // ��ũ���� ��� �������� ����
    void ScreenshotAndGallery()
    {
        // ��ũ����
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // ����������
        Debug.Log("" + NativeGallery.SaveImageToGallery(ss, albumName,
            "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));

        // To avoid memory leaks.
        // ���� �Ϸ�Ʊ� ������ ���� �޸� ����
        Destroy(ss);

    }
}