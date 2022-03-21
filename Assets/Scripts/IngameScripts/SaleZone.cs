using UnityEngine;

public class SaleZone : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "TradeBale")
        {
            Destroy(other.gameObject);
            UIManager.instance.AddCoins(transform.position, 1);
            Debug.Log($"Bales in bag : {GameManager.instance.balesNum}");
        }
    }
}
