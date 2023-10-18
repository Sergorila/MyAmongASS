using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTask : MonoBehaviour
{
    public List<SwipePoint> swipePoints = new List<SwipePoint>();

    public float countDownMax = 0.5f;

    public float timeSpend = 0f;

    private int currentSwipePointIndex = 0;

    private float countDown = 0;

    public GameObject greenOn;

    public GameObject redOn;

    public GameObject textNeutral;
    public GameObject textSlow;
    public GameObject textFast;
    public GameObject textSuccess;

    private void Update()
    {
        if (currentSwipePointIndex != 0)
        {
            timeSpend += Time.deltaTime;
        }
        //if (currentSwipePointIndex != 0 && timeSpend > 10)
        //{
        //    currentSwipePointIndex = 0;
        //    StartCoroutine(FinishTask(false));
        //}
    }

    public IEnumerator FinishTask(bool isSuccessful)
    {
        textNeutral.SetActive(false);

        if (isSuccessful && timeSpend >= 1f && timeSpend <= 1.5f)
        {
            greenOn.SetActive(true);
            textSuccess.SetActive(true);
        }
        else
        {
            redOn.SetActive(true);

            if (timeSpend > 1.5f)
            {
                textSlow.SetActive(true);
            }
            else if (timeSpend < 1f)
            {
                textFast.SetActive(true);
            }
        }

        yield return new WaitForSeconds(2f);

        timeSpend = 0;
        greenOn.SetActive(false);
        redOn.SetActive(false);
        textSlow.SetActive(false);
        textFast.SetActive(false);
        textSuccess.SetActive(false);
        textNeutral.SetActive(true);
    }

    public void SwipePointTrigger(SwipePoint swipePoint)
    {
        if (swipePoint == swipePoints[currentSwipePointIndex])
        {
            currentSwipePointIndex++;
            countDown = countDownMax;
        }

        if (currentSwipePointIndex > swipePoints.Count - 1)
        {
            currentSwipePointIndex = 0;
            StartCoroutine(FinishTask(true));
        }
    }
}
