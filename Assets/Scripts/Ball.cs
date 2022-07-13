using UnityEngine;

public enum BallColor { blue, red, yellow, green, purple }

public class Ball : MonoBehaviour
{
    public BallColor color;
    public bool isAtMostUpperSide;
    public bool isStatic = false;


    MeshRenderer mesh;
    Material material;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        material = mesh.material;
        ChangeColor(color);
    }

    public void ChangeColor(BallColor color)
    {
        switch (color)
        {
            case BallColor.blue:
                material.color = Color.blue;
                break;
            case BallColor.green:
                material.color = Color.green;
                break;
            case BallColor.red:
                material.color = Color.red;
                break;
            case BallColor.yellow:
                material.color = Color.yellow;
                break;
            case BallColor.purple:
                material.color = new Color(0.55f, 0f, 1f, 1f);
                break;
            default:
                material.color = Color.white;
                break;
        }
    }

    private void OnDestroy()
    {
        BallManager.RemoveBall(this);
    }
}