using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    private Vector3 coinImagePosOrig;

    private bool isCoinImageVibrating;

    [Header("UI Reference")]
    [SerializeField] private TMP_Text coinUIText;
    [SerializeField] private TMP_Text baleUIText;
    [SerializeField] private GameObject animatedCoinPrefab;
    [SerializeField] private Transform coinImageTarget;

    [Space]
    [SerializeField] private int maxCoins;
    private Queue<GameObject> coinsQueue = new Queue<GameObject>();

    [Space]
    [Header("Animations Settings")]
    [SerializeField] [Range(0.5f, 0.9f)] private float minAnimDuration;
    [SerializeField] [Range(0.9f, 2f)] private float maxAnimDiration;
    [SerializeField] private Ease easeType;

    private Vector3 targetPosition;


    private GameManager gameManager;

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

        targetPosition = coinImageTarget.position;
        coinImagePosOrig = coinImageTarget.localPosition;
        PreparePool();
    }

    private void PreparePool()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
        
    }

    private void Animate(Vector3 startCoinPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(coinsQueue.Count > 0)
            {
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);

                coin.transform.position = startCoinPosition;

                coin.transform.LookAt(gameManager.mainCamera.transform, -Vector3.up);

                var duration = Random.Range(minAnimDuration, maxAnimDiration);

                coin.transform.DOMove(targetPosition, duration).SetEase(easeType).OnComplete(() => 
                {
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                    gameManager.AddCoins(15);

                    if(!isCoinImageVibrating)
                    {
                        var vibration = coinImageTarget.transform.DOShakePosition(0.5f, 10);
                        vibration.WaitForStart();
                        vibration.OnPlay(() => isCoinImageVibrating = true);
                        vibration.OnComplete(() => { isCoinImageVibrating = false; coinImageTarget.localPosition = coinImagePosOrig; });
                        vibration.Play();
                    }
                    
                    
                    coinUIText.text = gameManager.coins.ToString();
                    coinImageTarget.localPosition = coinImagePosOrig;
                });
            }
        }
    }

    public void AddCoins(Vector3 startCoinPosition, int amount)
    {
        Animate(startCoinPosition, amount);
    }

    public void UpdateBales(int value)
    {

        baleUIText.text = value.ToString() + "/" + gameManager.maxBales.ToString();

        if (value == gameManager.maxBales)
        {
            baleUIText.color = Color.red;
        }
        else if (value == gameManager.maxBales - 1)
        {
            baleUIText.color = Color.white;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }


}
