using System.Collections.Generic;
using UnityEngine;

public class DynamicBall : MonoBehaviour
{
    public List<Ball> neighbours;

    bool hasSameColorNeighbour = false;

    private void Awake()
    {
        neighbours = GetComponent<NeighbourController>().neighboursList;
    }

    //listeye komþu eklenir eklenmez renk kontrolü yapýlýr
    private void Update()
    {
        if (neighbours.Count > 0) DestroySameColor();
    }


    void DestroySameColor()
    {
        foreach (Ball neighbour in neighbours)
        {
            if (neighbour.GetComponent<Ball>().color == GetComponent<Ball>().color)
            {
                //ayný renkte komþu top varsa yok edilmeden önce
                //kendini komþularýn listesinden çýkarýr
                foreach (Ball ball in neighbours)
                {
                    ball.GetComponent<NeighbourController>().neighboursList.Remove(GetComponent<Ball>());
                }
                hasSameColorNeighbour = true;
                Destroy(neighbour.gameObject, 0.1f);
            }
        }
        if (hasSameColorNeighbour) Destroy(gameObject, 0.1f);
        else Destroy(this);
    }
}