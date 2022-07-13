using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPocket : MonoBehaviour
{
    public Ball currentBall;
    public Ball nextBall;

    [SerializeField] Ball ballPrefab;
    [SerializeField] Transform ballParent;

    [SerializeField] Transform currentBallPosition;
    [SerializeField] Transform nextBallPosition;

    WaitForSeconds generateDelay = new WaitForSeconds(0.5f);

    public bool readyToBeThrown = true;

    private void Start()
    {
        currentBall = GenerateNewBall(ballPrefab, currentBallPosition.position, Quaternion.identity, ballParent);
        nextBall = GenerateNewBall(ballPrefab, nextBallPosition.position, Quaternion.identity, ballParent);
    }

    public IEnumerator ReplaceThrownBall()
    {
        yield return generateDelay;

        Ball oldBall = nextBall;
        Ball newBall = GenerateNewBall(ballPrefab, nextBallPosition.position, Quaternion.identity, ballParent);

        currentBall = oldBall;
        nextBall = newBall;

        currentBall.transform.position = currentBallPosition.position;
        nextBall.transform.position = nextBallPosition.position;

        readyToBeThrown = true;
    }

    public void ChangeBalls()
    {
        BallColor currentBallColor = currentBall.color;
        BallColor nextBallColor = nextBall.color;

        currentBall.color = nextBallColor;
        nextBall.color = currentBallColor;

        currentBall.GetComponent<Ball>().ChangeColor(currentBall.color);
        nextBall.GetComponent<Ball>().ChangeColor(nextBall.color);
    }


    public Ball GenerateNewBall(Ball prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        Ball newBall = Instantiate(prefab, position, rotation, parent);
        newBall.color = CheckNonEmptyColors()[Random.Range(0, CheckNonEmptyColors().Count)];

        return newBall;
    }


    //oyun içindeki ayný renkli son top yok edilirse
    //cepteki topun rengi oyun içinde bulunan toplarýnkiyle deðiþtirilir
    public void ChangeEmtyColor(BallColor emtyColor)
    {
        if (CheckNonEmptyColors() == null)
        {
            Destroy(currentBall);
            Destroy(nextBall);

            Time.timeScale = 0;
        }

        if (currentBall.color == emtyColor)
        {
            currentBall.color = CheckNonEmptyColors()[Random.Range(0, CheckNonEmptyColors().Count)];
            currentBall.GetComponent<Ball>().ChangeColor(currentBall.color);
        }
        if (nextBall.color == emtyColor)
        {
            nextBall.color = CheckNonEmptyColors()[Random.Range(0, CheckNonEmptyColors().Count)];
            nextBall.GetComponent<Ball>().ChangeColor(nextBall.color);
        }
    }


    List<BallColor> colors = new List<BallColor>(5);
    List<BallColor> CheckNonEmptyColors()
    {
        colors.Clear();

        if (BallManager.redBalls.Count > 0) colors.Add(BallColor.red);
        if (BallManager.blueBalls.Count > 0) colors.Add(BallColor.blue);
        if (BallManager.yelloBalls.Count > 0) colors.Add(BallColor.yellow);
        if (BallManager.greenBalls.Count > 0) colors.Add(BallColor.green);
        if (BallManager.purpleBalls.Count > 0) colors.Add(BallColor.purple);

        return colors;
    }
}