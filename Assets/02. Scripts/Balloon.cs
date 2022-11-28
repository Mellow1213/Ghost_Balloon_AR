using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Balloon : MonoBehaviour
{


    float hapiness = 50;

    public TrackingTarget[] targets;
    public TrackingTarget BalloonTarget;

    public Transform homeTransform;
    public Transform rotateAnchor;
    Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BalloonTarget.getIsTracked())
        {
            //Debug.Log("FoodTarget = " + targets[0].getIsTracked());
            if (targets[0].getIsTracked())
            {
                Eat();
            }
            else
            {
                isEat = false;
                food.SetActive(true);
            }

            //Debug.Log("ShowerTarget = " + targets[1].getIsTracked());
            if (targets[1].getIsTracked())
            {
                rainParticle.SetActive(true);
            }
            else
            {
                rainParticle.SetActive(false);
            }

            if (doWashing)
            {
                showerTimer += Time.deltaTime;
                Debug.Log("샤워하고 난 대기 시간 = " + showerTimer);
                if (showerTimer >= 5f)
                {
                    Debug.Log("몸털기");
                    Debug.Log("청결도 올라감");
                    doWashing = false;
                }
            }

            if (targets[2].getIsTracked())
            {
                Exercise();
            }


            if (targets[3].getIsTracked())
            {
                Walk();
                Debug.Log("산책 시작");
            }
            else
            {
                if (isWalkStarted)
                {
                    rotateAnchor.DOMove(homeTransform.position, 2f);
                    if (Vector3.Distance(homeTransform.position, rotateAnchor.position) < 0.5f)
                    {
                        Debug.Log("산책 끝");

                        isWalkStarted = false;
                    }
                }
            }


            Play();

            if (!targets[0].getIsTracked() && !targets[1].getIsTracked() && !targets[2].getIsTracked() && !targets[3].getIsTracked())
            {
                Idle();
            }
        }




        //CalculateWatchVector(eatTransform);

        //Debug.Log("ExerciseTarget = " + targets[2].getIsTracked());
        //Debug.Log("WalkTarget = " + targets[3].getIsTracked());

        //Play();
        //Eat();
        //Walk();
    }


    public Transform eatTransform;
    public GameObject food;
    bool isEat = false;
    float EatTimer = 0f;
    void Eat()
    {
        if (!isEat)
        {
            EatTimer += Time.deltaTime;

            if (EatTimer >= 3f)
            {
                EatTimer = 0f;
                isEat = true;
                StartCoroutine(DoEat());
            }
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

    bool doWashing = false;
    float showerTimer = 0f;
    public GameObject rainParticle;
    //샤워 시스템
    private void OnParticleCollision(GameObject other)
    {
        doWashing = true;
        showerTimer = 0f;
        Debug.Log("청결도+");
    }



    public LineRenderer _lineRenderer;
    public Transform linePos;
    public Transform BalloonLead;
    bool isWalkStarted = false;
    void Walk()
    {
        LookState(2);
        isWalkStarted = true;
        _lineRenderer.SetPosition(0, linePos.position);
        _lineRenderer.SetPosition(1, BalloonLead.position);
        if (Vector3.Distance(rotateAnchor.transform.position, linePos.position) > 0.5f)
        {
            rotateAnchor.position = Vector3.Lerp(rotateAnchor.transform.position, linePos.position, Time.deltaTime);
        }
    }

    public GameObject punchingBag;
    float exerciseTimer = 0f;
    float exerciseDistance;
    void Exercise()
    {
        LookState(3);
        exerciseDistance = Vector3.Distance(punchingBag.transform.position, rotateAnchor.position);
        if (exerciseDistance < 1.8f && exerciseDistance > 0.3f)
        {
            exerciseTimer += Time.deltaTime;
            if (exerciseTimer > 1f)
            {
                exerciseTimer = 0f;
                _animator.SetTrigger("Exercise");
                Debug.Log("운동중!");
                Debug.Log("건강도 올라감!");
            }
        }
        else
        {
            exerciseTimer = 0f;
        }
    }

    bool isPlay = false;
    float walkDistance = 0;
    Vector3 prevPos;
    float timer = 0.1f;

    const float playTimeDistance = 100f;
    void Play()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && !isPlay) // 버튼 누르기로 바꾸기
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

    bool isIdle = true;
    void Idle()
    {
        if (isIdle)
        {
            Debug.Log("일정 간격으로 모든 수치 감소");
            Debug.Log("피로도 소폭 감소");
            if (Input.GetKey(KeyCode.K)) // 애정도 일정 수준 이상일 때로 변경
                LookState(1);
            else
            {
                LookState(0);
            }
        }
    }


    void LookState(int state)
    {
        switch (state)
        {
            case 0: // 대기상태
                rotateAnchor.DOLocalRotate(Vector3.zero, 2f);
                break;
            case 1: // 카메라를 보고 있음
                CalculateWatchVector(Camera.main.transform);
                break;
            case 2: // 산책중
                CalculateWatchVector(linePos);
                break;
            case 3: // 운동중
                CalculateWatchVector(punchingBag.transform);
                break;
        }
    }
    void CalculateWatchVector(Transform targetTransform)
    {
        Vector3 direction = (targetTransform.position - rotateAnchor.position).normalized;
        rotateAnchor.rotation = Quaternion.Lerp(rotateAnchor.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2f);
        rotateAnchor.localEulerAngles = new Vector3(rotateAnchor.localEulerAngles.x, rotateAnchor.localEulerAngles.y, 0);
    }
}
