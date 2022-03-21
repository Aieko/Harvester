using UnityEngine;

public class SaleZone : MonoBehaviour
{
    [SerializeField] Transform coinGiver;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "TradeBale")
        {
            GameManager.instance.EnqueueTradeBale(other.gameObject);
            other.gameObject.SetActive(false);
            UIManager.instance.AddCoins(coinGiver.position, 1);
            Debug.Log($"Bales in bag : {GameManager.instance.balesNum}");
        }
    }
}
