using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    static BallPocket ballPocket;

    public static List<Ball> redBalls = new List<Ball>();
    public static List<Ball> blueBalls = new List<Ball>();
    public static List<Ball> yelloBalls = new List<Ball>();
    public static List<Ball> greenBalls = new List<Ball>();
    public static List<Ball> purpleBalls = new List<Ball>();

    private void Start()
    {
        ballPocket = FindObjectOfType<BallPocket>();
    }

    //seçili topun rengindeki bütün toplarýn bulunduðu listeyi döndürür
    static List<Ball> GetBallList(Ball ball)
    {
        List<Ball> balls;
        switch (ball.color)
        {
            case BallColor.red:
                balls = redBalls;
                return balls;
            case BallColor.blue:
                balls = blueBalls;
                return balls;
            case BallColor.yellow:
                balls = yelloBalls;
                return balls;
            case BallColor.green:
                balls = greenBalls;
                return balls;
            case BallColor.purple:
                balls = purpleBalls;
                return balls;
            default:
                return null;
        }
    }

    public static void AddNewBall(Ball newBall)
    {
        GetBallList(newBall).Add(newBall);

        //print(newBall.color.ToString() + " ball number = " + GetBallList(newBall).Count.ToString());
    }

    public static void RemoveBall(Ball ballToRemove)
    {
        List<Ball> ballList = GetBallList(ballToRemove);
        ballList.Remove(ballToRemove);

        if (ballList.Count == 0) ballPocket.ChangeEmtyColor(ballToRemove.color);

        //print(ballToRemove.color.ToString() + " ball number = " + ballList.Count.ToString());
    }
}