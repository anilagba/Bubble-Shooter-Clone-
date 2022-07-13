using System.Collections.Generic;
using UnityEngine;

public class NeighbourController : MonoBehaviour
{
    public List<Ball> neighboursList = new List<Ball>(6);

    private void Start()
    {
        BallManager.AddNewBall(GetComponent<Ball>());
    }

    //bir birine komþu bütün toplar listeye eklenir
    //ilk sýrada bulunan toplardan birine rastlanýrsa true deðer döndürür
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
        //sadece komþusu olmayan tek top var ise silinmek için listeye eklenir
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


    //yok olmadan önce havada asýlý kalmamasý için komþu toplar da kontrol edilir
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

                    //ilk sýrada bulunan toplardan birine baðlý top yoksa
                    //listedeki bütün bir birine komþu toplar silinir
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