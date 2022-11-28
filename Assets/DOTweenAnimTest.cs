using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenAnimTest : MonoBehaviour
{
    public Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.DOJump(new Vector3(0, 2, 0), 2f, 1, 2f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetEase(Ease.OutCubic).SetLoops(4, LoopType.Yoyo);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(Spin());
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.DORotate(new Vector3(30f, 0, 0), 0.5f).SetEase(Ease.Linear).SetLoops(24, LoopType.Incremental);
        }
    }
    
    IEnumerator Spin()
    {
        transform.DORotate(new Vector3(0, 0, 30f), 0.5f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(1f);
        transform.DORotate(new Vector3(0, 0, -30f), 1f).SetEase(Ease.OutQuart).SetLoops(4, LoopType.Yoyo);
        yield return new WaitForSeconds(4f);
        transform.DORotate(Vector3.zero, 1f);
    }
}
