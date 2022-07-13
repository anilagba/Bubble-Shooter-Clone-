using UnityEngine;
using System.Collections;

public class StopMoving : MonoBehaviour
{
    Rigidbody rb;
    bool isTriggered = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    //fýrlatýlan topu çarptýðý topun konumuna göre yerleþtirir
    //ayný hizada ya da daha yukarda ise topun yanýnda
    //daha aþaðýda ise sað ya da sol çaprazda konumlanýr
    void PlaceBall(Collider other)
    {
        if (Vector3.Dot(transform.up, transform.position - other.transform.position) < -0.1f)
        {
            if (Mathf.Sign(Vector3.Dot(transform.right, transform.position - other.transform.position)) == -1)
                rb.position = new Vector3(other.transform.position.x - 0.5f, other.transform.position.y - 0.9f, 0);
            else
                rb.position = new Vector3(other.transform.position.x + 0.5f, other.transform.position.y - 0.9f, 0);
        }
        else
        {
            if (Mathf.Sign(Vector3.Dot(transform.right, transform.position - other.transform.position)) == -1)
                rb.position = new Vector3(other.transform.position.x - 1, other.transform.position.y, 0);
            else
                rb.position = new Vector3(other.transform.position.x + 1, other.transform.position.y, 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall")) return;

        else
        {
            rb.isKinematic = true;

            PlaceBall(other);

            if (!isTriggered) StartCoroutine(AddComponents());

            isTriggered = true;
        }
    }


    //tüm komþularýn ayný anda listeye eklenebilmesi için 1 frame bekler
    IEnumerator AddComponents()
    {
        yield return null;

        gameObject.AddComponent<NeighbourController>();

        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = 0.55f;

        gameObject.AddComponent<DynamicBall>();

        Destroy(this);
    }
}