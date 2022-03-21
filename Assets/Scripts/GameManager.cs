using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private UIManager userInterfaceManager;

    [Header("Prefabs")]
    public GameObject wheatPrefab;
    public GameObject balePrefab;
    public GameObject tradeBalePrefab;
    [Header("Trading Settings")]
    [SerializeField] private Transform baleTargetTransform;
    [SerializeField] private float baleMoveSpeed;
    [SerializeField] float tradeRate = 1f;
    [Header("Player GameObject")]
    [SerializeField] private GameObject playerGO;
    [Header("Bag Settings")]
    [SerializeField] private Transform bagPos;
    [SerializeField] GameObject[] bagBale;
    public int balesNum;

    [SerializeField] private int _maxBales = 40;

    public int maxBales => _maxBales;

    public Camera mainCamera { get; private set; }

    public int coins { get; private set; }

    private bool isTrading;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        userInterfaceManager = UIManager.instance;
       
        coins = 0;
        balesNum = 0;
        userInterfaceManager.UpdateBales(balesNum);
        for (int i = 0; i < bagBale.Length; i++)
        {
            bagBale[i].SetActive(false);
        }
    }

    public void AddBale()
    {
        if (balesNum < _maxBales)
        {
            balesNum++;
            userInterfaceManager.UpdateBales(balesNum);
            Debug.Log($"Bales in bag : {balesNum}");
            CheckBagContent();
        }
        else Debug.Log("Your bag is full! Can't carry anymore bales.");

        
    }

    private void CheckBagContent(bool isAdding = true)
    {
        var four = _maxBales / 1.3;
        var three = (int)_maxBales / 2;
        var two = (int)_maxBales / 3;

        if(isAdding)
        {
            if (balesNum == 1)
            {
                bagBale[0].SetActive(true);
            }
            else if( balesNum == two)
            {
                bagBale[1].SetActive(true);
            }
            else if(balesNum == three)
            {
                bagBale[2].SetActive(true);
            }
            else if(balesNum == (int)four)
            {
                bagBale[3].SetActive(true);
            }
         }
        else
        {
            if (balesNum == 0)
            {
                bagBale[0].SetActive(false);
            }
            else if (balesNum == two)
            {
                bagBale[1].SetActive(false);
            }
            else if (balesNum == three)
            {
                bagBale[2].SetActive(false);
            }
            else if (balesNum == (int)four)
            {
                bagBale[3].SetActive(false);
            }
        }
    }

    private int IntLerp(int a, int b, float t)
    {
        if (t > 0.9999f)
        {
            return b;
        }

        return a + (int)(((float)b - (float)a) * t);
    }

    public void StartTrade()
    {
        if (balesNum > 0)
        {
            isTrading = true;
            StartCoroutine(TradeBales());
        }
        else Debug.Log("Your bag is empty. Go back to work!");
        
    }

    private IEnumerator TradeBales()
    {
        while(isTrading)
        {
            if (balesNum == 0)
            {
                Debug.Log("You sold all bales!");
                StopTrade();
                break;
            }

            balesNum--;
            userInterfaceManager.UpdateBales(balesNum);
            var bale = Instantiate(tradeBalePrefab, bagPos.position,bagPos.rotation);
            CheckBagContent(false);

            yield return new WaitForSeconds(tradeRate);
        }
       
    }

    public void AddCoins(int value)
    {
        coins += value;
    }

    public void StopTrade()
    {
        isTrading = false;
    }

   
}
