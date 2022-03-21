using UnityEngine;

public class BaleScript : MonoBehaviour, IPickable
{
    private void Start()
    {
        Destroy(gameObject, 15f);
    }

    public void Pickup()
    {
        GameManager.instance.AddBale();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            
        }

        if(other.tag == "Player")
        {
            Pickup();
            Destroy(gameObject);
        }
    }

}
