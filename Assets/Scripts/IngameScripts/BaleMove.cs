using UnityEngine;

public class BaleMove : MonoBehaviour
{
    [SerializeField]private Transform target;
    [SerializeField]private float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        if (target)
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
    }

    public void SetTargetAndSpeed(Transform target, float speed)
    {
        this.target = target;
        moveSpeed = speed;
    }
}
