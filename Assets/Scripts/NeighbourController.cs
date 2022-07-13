using System.Collections.Generic;
using UnityEngine;

public class NeighbourController : MonoBehaviour
{
    public List<Ball> neighboursList = new List<Ball>(6);

    private void Start()
    {
        BallManager.AddNewBall(GetComponent<Ball>());
    }

    //bir birine kom�u b�t�n toplar listeye eklenir
    //ilk s�rada bulunan toplardan birine rastlan�rsa true de�er d�nd�r�r
    public bool CheckForNeighbourBalls(List<Ball> checkedBalls, bool hasUpperSideBall)
    {
        if (GetComponent<Ball>().isAtMostUpperSide) hasUpperSideBall = true;

        foreach (Ball ball in neighboursList)
        {
            if (!checkedBalls.Contains(ball))
            {
                checkedBalls.Add(ball);
                hasUpperSideBall = ball.GetComponent<NeighbourController>().CheckForNeighbourBalls(checkedBalls, hasUpperSideBall);
            }
        }
        //sadece kom�usu olmayan tek top var ise silinmek i�in listeye eklenir
        if (neighboursList.Count == 0) checkedBalls.Add(GetComponent<Ball>());

        return hasUpperSideBall;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall")) return;

        Ball triggeredBall = other.GetComponent<Ball>();

        if (!neighboursList.Contains(triggeredBall))
        {
            neighboursList.Add(triggeredBall);
        }
    }


    //yok olmadan �nce havada as�l� kalmamas� i�in kom�u toplar da kontrol edilir
    private void OnDestroy()
    {
        foreach (Ball ball in neighboursList)
        {
            if (GetComponent<Ball>()) ball.GetComponent<NeighbourController>().neighboursList.Remove(GetComponent<Ball>());
        }

        if (GetComponent<DynamicBall>()) return;

        foreach (Ball ball in neighboursList)
        {
            try
            {
                if (ball.color == GetComponent<Ball>().color)
                {
                    Destroy(ball.gameObject, 0.1f);
                }
                else
                {
                    List<Ball> checkedBalls = new List<Ball>();
                    bool hasUpperSideBall = false;

                    hasUpperSideBall = ball.GetComponent<NeighbourController>().CheckForNeighbourBalls(checkedBalls, hasUpperSideBall);

                    //ilk s�rada bulunan toplardan birine ba�l� top yoksa
                    //listedeki b�t�n bir birine kom�u toplar silinir
                    if (!hasUpperSideBall)
                    {
                        foreach (Ball neighbour in checkedBalls)
                        {
                            Destroy(neighbour.gameObject, 0.1f);
                        }
                    }
                }
            }
            catch { }
        }
    }
}