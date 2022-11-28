using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Balloon : MonoBehaviour
{
    float hapiness = 50;
    bool isIdle = true;
    public Transform homeTransform;
    public Transform rotateAnchor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //CalculateWatchVector(eatTransform);


        if (Input.GetKey(KeyCode.K))
            CalculateWatchVector(Camera.main.transform);
        else
            rotateAnchor.DOLocalRotate(Vector3.zero, 2f); 




        Play();
        Eat();
        Walk();
    }


    public Transform eatTransform;
    bool doEat = false;
    public GameObject food;
    void Eat()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            doEat = true;
        }

        if (doEat)
        {
            doEat = false;
            StartCoroutine(DoEat());
        }
    }

    IEnumerator DoEat()
    {
        transform.DOMove(eatTransform.position, 2f);

        yield return new WaitForSeconds(4f);
        transform.DOLocalRotate(new Vector3(62, 0, 0), 0.5f);
        transform.DOLocalRotate(new Vector3(20, 0, 0), 0.5f).SetEase(Ease.Linear).SetLoops(4, LoopType.Yoyo);
        yield return new WaitForSeconds(4f);
        food.SetActive(false);
        transform.DOMove(homeTransform.position, 2f);
        Debug.Log("음식 섭취");
        Debug.Log("포만도 크게 증가");
        Debug.Log("청결도 작게 감소");
    }

    public LineRenderer _lineRenderer;
    public Transform linePos;
    public Transform BalloonLead;
    void Walk()
    {
        _lineRenderer.SetPosition(0, linePos.position);
        _lineRenderer.SetPosition(1, BalloonLead.position);
        if(Vector3.Distance(rotateAnchor.transform.position, linePos.position) > 1f)
        {
            rotateAnchor.position = Vector3.MoveTowards(rotateAnchor.transform.position, linePos.position, 5 * Time.deltaTime);
        }
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
                Debug.Log("이동 거리 : " + Vector3.Distance(prevPos, transform.position));
                timer = 0f;
                prevPos = transform.position;
            }
            Debug.Log("walkDistance : " + walkDistance);
            Debug.Log("산책 중");

            if (walkDistance >= playTimeDistance)
            {
                walkDistance = 0;
                isPlay = false;
                Debug.Log("놀아주기 끝.");
                Debug.Log("즐거움 크게 증가.");
                Debug.Log("청결도 작게 감소.");
                Debug.Log("포만도 중간 감소.");
            }
        }


    }

    void Idle()
    {
        if (isIdle)
        {
            Debug.Log("일정 간격으로 모든 수치 감소");
            Debug.Log("피로도 소폭 감소");
        }
    }

    //샤워 시스템
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("청결도 +1");
    }

    void CalculateWatchVector(Transform targetTransform)
    {
        Vector3 direction = (targetTransform.position - rotateAnchor.position).normalized;

        rotateAnchor.rotation = Quaternion.Lerp(rotateAnchor.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2f);
        rotateAnchor.localEulerAngles = new Vector3(rotateAnchor.localEulerAngles.x, rotateAnchor.localEulerAngles.y, 0);
    }
}
