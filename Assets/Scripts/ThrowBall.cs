using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    [SerializeField] BallPocket ballPocket;
    [SerializeField] float throwForce;
    [SerializeField] CanvasRenderer touchPad;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float rotateSensitivity;

    float touchPadUpperBound;
    float touchPadLowerBound;

    Rigidbody rb;

    bool canThrow = true;
    Vector2 firstTouchPos;
    Vector2 lastTouchPos;
    Vector2 touchDir;

    int screenWidht;
    int screenHeight;



    private void Start()
    {
        screenWidht = Screen.width;
        screenHeight = Screen.height;

        touchPadUpperBound = touchPad.transform.position.y + (screenHeight * (touchPad.transform.localScale.y / 2));
        touchPadLowerBound = touchPad.transform.position.y - (screenHeight * (touchPad.transform.localScale.y / 2));
    }

    private void Update()
    {
        ControllTouch();
    }



    void ControllTouch()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            //sadece touchpad sýnýrlarý içinde fýrlatma yapýlabilir
            if (touch.position.y > touchPadUpperBound || touch.position.y < touchPadLowerBound)
            {
                lineRenderer.positionCount = 0;
                return;
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    firstTouchPos = Vector2.zero;
                    lastTouchPos = Vector2.zero;
                    touchDir = Vector2.zero;

                    canThrow = true;
                    firstTouchPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    lastTouchPos = touch.position;
                    touchDir = (lastTouchPos - firstTouchPos);
                    RotateShooter(touchDir);
                    CastPreviewRay(transform.position, transform.up);
                    break;
                case TouchPhase.Stationary:
                    CastPreviewRay(transform.position, transform.up);
                    break;
                case TouchPhase.Ended:
                    if (canThrow) Shoot();
                    lineRenderer.positionCount = 0;
                    break;
            }
        }
    }

    int layerMask = 1 << 6 | 1 << 7;

    void CastPreviewRay(Vector3 startPoint, Vector3 direction)
    {
        RaycastHit raycastHit;
        bool hasHit = Physics.Raycast(transform.position, direction, out raycastHit, 30, layerMask);

        if (hasHit)
        {
            lineRenderer.positionCount = 3;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, raycastHit.point);
            lineRenderer.SetPosition(2, raycastHit.point);

            if (raycastHit.transform.GetComponent<Ball>()) return;

            Vector3 reflect = Vector3.Reflect(transform.up * throwForce, raycastHit.normal);

            Physics.Raycast(raycastHit.point, reflect, out raycastHit, 30, layerMask);

            lineRenderer.SetPosition(2, raycastHit.point);
        }
    }

    void RotateShooter(Vector2 touchDir)
    {
        float rotZ = 0;
        float xAxis = touchDir.x / screenWidht;
        rotZ -= transform.rotation.z + xAxis * rotateSensitivity;
        if (rotZ < 60 && rotZ > -60)
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        else
        {
            canThrow = false;
            transform.rotation = Quaternion.identity;
        }
    }

    void Shoot()
    {
        if (!ballPocket.readyToBeThrown) return;

        Ball ball = ballPocket.currentBall;
        ball.GetComponent<SphereCollider>().radius = 0.15f;
        ball.gameObject.AddComponent<StopMoving>();

        rb = ball.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * throwForce, ForceMode.VelocityChange);
        ballPocket.currentBall = null;
        StartCoroutine(ballPocket.ReplaceThrownBall());
    }
}