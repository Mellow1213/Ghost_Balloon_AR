using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{
    public GameObject blink;             // 사진 찍을 때 깜빡일 것

    bool isCoroutinePlaying;             // 코루틴 중복방지

    // 파일 불러올 때 필요
    string albumName = "Ghost";           // 생성될 앨범의 이름


    // 캡쳐 버튼을 누르면 호출
    public void Capture_Button()
    {
        // 중복방지 bool
        if (!isCoroutinePlaying)
        {
            StartCoroutine("captureScreenshot");
        }
    }

    IEnumerator captureScreenshot()
    {
        isCoroutinePlaying = true;

        yield return new WaitForEndOfFrame();

        // 스크린샷 + 갤러리갱신
        ScreenshotAndGallery();

        yield return new WaitForEndOfFrame();

        // 블링크
        blink.SetActive(true);

        yield return new WaitForSecondsRealtime(0.3f);

        blink.SetActive(false);

        isCoroutinePlaying = false;
    }

    // 스크린샷 찍고 갤러리에 갱신
    void ScreenshotAndGallery()
    {
        // 스크린샷
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // 갤러리갱신
        Debug.Log("" + NativeGallery.SaveImageToGallery(ss, albumName,
            "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));

        // To avoid memory leaks.
        // 복사 완료됐기 때문에 원본 메모리 삭제
        Destroy(ss);

    }
}