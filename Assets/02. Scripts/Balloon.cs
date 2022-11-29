using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    [SerializeField] float fun = 40;
    [SerializeField] float clean = 40;
    [SerializeField] float healthy = 40;
    [SerializeField] float full = 40;

    [SerializeField] float affection = 20;
    [SerializeField] float fatigue = 0;

    public Image _image;
    public Slider[] silderBar;
    public Image[] sliderImage;

    public TrackingTarget[] targets;
    public TrackingTarget BalloonTarget;

    public Transform homeTransform;
    public Transform rotateAnchor;
    Animator _animator;

    public float getAffection()
    {
        return affection;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fun = Mathf.Clamp(fun, 0, 100);
        clean = Mathf.Clamp(clean, 0, 100);
        healthy = Mathf.Clamp(healthy, 0, 100);
        full = Mathf.Clamp(full, 0, 100);
        affection = Mathf.Clamp(affection, 0, 100);
        fatigue = Mathf.Clamp(fatigue, 0, 100);

        affection = fun * 0.25f + clean * 0.25f + healthy * 0.25f + full * 0.25f;

        silderBar[0].value = fun / 100;
        silderBar[1].value = clean / 100;
        silderBar[2].value = healthy / 100;
        silderBar[3].value = full / 100;
        silderBar[4].value = affection / 100;
        silderBar[5].value = fatigue / 100;

        for(int i = 0; i< 5; i++)
        {
            if (silderBar[i].value > 0.65f)
            {
                sliderImage[i].DOColor(Color.green, 3f);
            }
            else if (silderBar[i].value > 0.30f)
            {
                sliderImage[i].DOColor(Color.yellow, 3f);
            }
            else 
            {
                sliderImage[i].DOColor(Color.red, 3f);
            }
        }
        if (silderBar[5].value > 0.65f)
        {
            sliderImage[5].DOColor(Color.red, 3f);

        }else if(silderBar[5].value > 0.30f)
        {
            sliderImage[5].DOColor(Color.yellow, 3f);
        }
        else
        {
            sliderImage[5].DOColor(Color.green, 3f);
        }

        if (BalloonTarget.getIsTracked())
        {
            //Debug.Log("FoodTarget = " + targets[0].getIsTracked());
            if (targets[0].getIsTracked())
            {
                if(fatigue < 100)
                    Eat();
                else
                {
                    Debug.Log("체력이 부족하여 수행 불가능");
                }
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
                if (fatigue < 100)
                    Exercise();
                else
                {
                    Debug.Log("체력이 부족하여 수행 불가능");

                }
            }


            if (targets[3].getIsTracked())
            {
                if (fatigue < 100)
                {
                    Walk();
                    Debug.Log("산책 시작");
                }
                else
                    Debug.Log("체력 부족 실행 불가능");
            }
            else
            {
                if (isWalkStarted)
                {
                    rotateAnchor.DOMove(homeTransform.position, 2f);
                    if (Vector3.Distance(homeTransform.position, rotateAnchor.position) < 0.5f)
                    {
                        Debug.Log("산책 끝");

                        if (walkTimer > 20)
                        {
                            fun += walkTimer * 0.5f;
                            clean -= walkTimer * 0.75f;
                            healthy += walkTimer * 0.5f;
                            full -= walkTimer * 0.75f;
                            fatigue += walkTimer * 0.5f;
                        }
                        isWalkStarted = false;
                    }
                }
            }

            Play();

            if (!targets[0].getIsTracked() && !targets[1].getIsTracked() && !targets[2].getIsTracked() && !targets[3].getIsTracked() && !doWashing)
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
        LookState(4);
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
        rotateAnchor.DOMove(eatTransform.position, 2f);

        yield return new WaitForSeconds(4f);
        rotateAnchor.DOLocalRotate(new Vector3(62, 0, 0), 0.5f);
        rotateAnchor.DOLocalRotate(new Vector3(20, 0, 0), 0.5f).SetEase(Ease.Linear).SetLoops(4, LoopType.Yoyo);
        yield return new WaitForSeconds(4f);
        food.SetActive(false);
        rotateAnchor.DOMove(homeTransform.position, 2f);
        Debug.Log("음식 섭취");
        Debug.Log("포만도 크게 증가");
        full += 20;
        Debug.Log("청결도 작게 감소");
        clean -= 5;
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
        clean += 0.1f;
        fatigue -= 0.2f;
    }



    public LineRenderer _lineRenderer;
    public Transform linePos;
    public Transform BalloonLead;
    bool isWalkStarted = false;
    float walkTimer = 0f;
    void Walk()
    {
        LookState(2);
        isWalkStarted = true;
        _lineRenderer.SetPosition(0, linePos.position);
        _lineRenderer.SetPosition(1, BalloonLead.position);
        if (Vector3.Distance(rotateAnchor.transform.position, linePos.position) > 0.5f)
        {
            walkTimer += Time.deltaTime;
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
                clean -= 1;
                full -= 1;
                fatigue += 2;
                healthy += 3;
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
    bool startPlay = false;

    public void DoPlay()
    {
        startPlay = true;
    }

    const float playTimeDistance = 40f;
    void Play()
    {
        if (startPlay && !isPlay) // 버튼 누르기로 바꾸기
        {
            startPlay = false;
            Debug.Log("놀아주기 수행 시작");
            if (fatigue < 100)
            {
                prevPos = transform.position;
                isPlay = true;
            }
            else
            {
                Debug.Log("체력이 부족 수행 불가");
            }
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
            _image.fillAmount = walkDistance / playTimeDistance;

            if (walkDistance >= playTimeDistance)
            {
                _image.fillAmount = 1;
                walkDistance = 0;
                isPlay = false;
                Debug.Log("놀아주기 끝.");
                Debug.Log("즐거움 크게 증가.");
                fun += 15;
                Debug.Log("청결도 작게 감소.");
                clean -= 10;
                Debug.Log("포만도 중간 감소.");
                full -= 10;
                fatigue += 10;
            }
        }


    }

    bool isIdle = true;
    float idleTimer = 0f;
    void Idle()
    {
        if (isIdle)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer >= 20f)
            {
                idleTimer = 0f;
                Debug.Log("일정 간격으로 모든 수치 감소");
                Debug.Log("피로도 소폭 감소");
                fun -= 2;
                clean -= 2;
                healthy -= 2;
                full -= 2;
                fatigue -= 1;
            }
            if (affection > 65f) // 애정도 일정 수준 이상일 때로 변경
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
            case 4:
                CalculateWatchVector(food.transform);
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
