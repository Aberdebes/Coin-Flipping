using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{

    public float triggerStartTime;
    public CoinSpawnerController coinSpawner;

    private void OnDestroy()
    {
        //print(name);
        coinSpawner.SignalCoinDestroyed(gameObject);
    }
}
