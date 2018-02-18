using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDetectionController : MonoBehaviour
{

    private int topCount, bottomCount, sideCount;
    public float killTime = 3.0f;

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        CoinController coin = other.GetComponentInParent<CoinController>();
        coin.triggerStartTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        CoinController coin = other.GetComponentInParent<CoinController>();
        if (Time.time - coin.triggerStartTime > killTime)
        {
            if (other.name == "Top Face collision") topCount++;
            if (other.name == "Bottom Face collision") bottomCount++;
            if (other.name == "Side Face collision") sideCount++;

            Destroy(coin.gameObject);

            //Debug//
            //PrintResults();
        }
                
    }

    public void PrintResults()
    {
        print(string.Format("Top: {0},   Bottom: {1},   Side: {2}", topCount, bottomCount, sideCount));
    }

    public int getTopCount() { return topCount; }
    public int getBottomCount() { return bottomCount; }
    public int getSideCount() { return sideCount; }


}
