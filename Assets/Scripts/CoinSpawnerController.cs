using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnerController : MonoBehaviour
{

    public GameObject coinPrefab;

    public float sideLengthRatio;
    public float forceMultiplier;
    public float torqueRandomMax;
    public float spawnDelay;
    public int desiredCoinQuantity;
    private int coinSpawnedCount;
    private List<GameObject> coins = new List<GameObject>();
    public TestingUnitSpawnerController testingUnitSpawner;


    void Start()
    {
        //StartCoroutine(TriggerSpawning());
    }
    
	void Update ()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    CreateCoin();
        //}
	}

    void CreateCoin()
    {
        GameObject thisCoin;
        thisCoin = Instantiate(coinPrefab, transform.position, Random.rotation);
        thisCoin.transform.localScale = new Vector3(coinPrefab.transform.localScale.x, 
                                                    coinPrefab.transform.localScale.y, 
                                                    coinPrefab.transform.localScale.z * sideLengthRatio);


        Rigidbody thisCoinRB = thisCoin.GetComponent<Rigidbody>();
        thisCoinRB.AddTorque(Random.onUnitSphere * Random.Range(0.0f, torqueRandomMax), ForceMode.Impulse);
        thisCoinRB.AddForce(Random.onUnitSphere * forceMultiplier, ForceMode.Impulse);

        thisCoin.name = transform.parent.name + " Coin" + coinSpawnedCount++.ToString();
        thisCoin.GetComponent<CoinController>().coinSpawner = this;
        //print(string.Format("Creating coin: {0}", thisCoin.name));
        coins.Add(thisCoin);
        //print("CoinCount: " + coins.Count.ToString());
    }

    public IEnumerator TriggerSpawning()
    {
        if (coinSpawnedCount < desiredCoinQuantity)
        {
            CreateCoin();
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(TriggerSpawning());
        }
        //else
        //{

        //    //send message to testing unit spawner controller
        //    GameObject testingUnit = transform.parent.gameObject;
        //    GameObject testingUnitSpawner = testingUnit.transform.parent.gameObject;
        //    TestingUnitSpawnerController testingUnitSpawnerController = testingUnitSpawner.GetComponent<TestingUnitSpawnerController>();
        //    //testingUnitSpawnerController.SignalUnitComplete(gameObject);

        //    //print(string.Format("Signalling complete: {0}", testingUnit.name));
        //    //print(string.Format("TEST TEST TEST {0}", testingUnit.transform.parent.name));

        //}
    }

    public void SignalCoinDestroyed(GameObject coin)
    {
        coins.Remove(coin);
        //print("SignalCoinDestroyed  coincount: " + coins.Count.ToString());
        if (coins.Count == 0 && coinSpawnedCount >= desiredCoinQuantity)
        {
            testingUnitSpawner.SignalUnitComplete(transform.parent.gameObject);
        }
    }
}
