using UnityEngine;
using System.Collections;

public class WheatScript : MonoBehaviour
{
    [SerializeField] private GameObject wheatPrefab;

    private void Start()
    {
        if(!wheatPrefab)
        {
            wheatPrefab = GameManager.instance.wheatPrefab;
        }
    }

    private IEnumerator Respawn()
    {
        var bale = Instantiate(GameManager.instance.balePrefab, transform.position, transform.rotation);
        bale.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);

        yield return new WaitForSeconds(10f);

        var wheat = Instantiate(wheatPrefab, transform.parent, false);
        wheat.transform.localPosition = new Vector3(transform.localPosition.x, wheatPrefab.transform.position.y, transform.localPosition.z);
        wheat.transform.rotation = transform.rotation;


        Destroy(gameObject);
    }

    public void StartRespawn()
    {
        StartCoroutine(Respawn());
    }

}
