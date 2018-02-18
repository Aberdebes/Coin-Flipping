using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingUnitSpawnerController : MonoBehaviour
{

    public GameObject testingUnit;
    public int unitsDeep, unitsWide;
    public int unitSize;
    public int desiredCoinQuantityPerUnit;
    public float sideRatio;
    public float startDelay;
    private List<GameObject> currentUnits = new List<GameObject>();
    private List<GameObject> finishedUnits = new List<GameObject>();
    private bool testingStarted = false;
    private bool testingComplete = false;
    private int ID;

    public bool TestingStarted() { return testingStarted; }
    public bool TestingComplete() { return testingComplete; }

    void Start()
    {
        //SetUpUnits(unitsDeep, unitsWide);
    }

    public IEnumerator SetUpUnits()
    {
        for (int i = 0; i < unitsDeep; i++)
        {
            for (int j = 0; j < unitsWide; j++)
            {
                Vector3 offset = new Vector3(j * unitSize, 0, i * unitSize);
                GameObject newUnit = Instantiate(testingUnit, transform.position + offset, Quaternion.identity, transform);
                CoinSpawnerController newUnitCoinSpawner = newUnit.GetComponentInChildren<CoinSpawnerController>();
                newUnit.name = "Spawner" + ID++.ToString();
                newUnitCoinSpawner.testingUnitSpawner = this;
                newUnitCoinSpawner.desiredCoinQuantity = desiredCoinQuantityPerUnit;
                newUnitCoinSpawner.sideLengthRatio = sideRatio;
                //newUnit.GetComponentInChildren<CoinSpawnerController>().testingUnitSpawner = this;
                currentUnits.Add(newUnit);
                StartCoroutine(newUnitCoinSpawner.TriggerSpawning());
                yield return new WaitForSeconds(startDelay);
            }
        }
        testingStarted = true;

    }

    void SumAndPrint()
    {
        int topSum = 0, bottomSum = 0, sideSum = 0;
        foreach (GameObject testingUnit in finishedUnits)
        {
            //print(string.Format("Summing for unit {0}", testingUnit.name));
            SideDetectionController detector = testingUnit.GetComponentInChildren<SideDetectionController>();
            topSum += detector.getTopCount();
            bottomSum += detector.getBottomCount();
            sideSum += detector.getSideCount();
        }
        print(string.Format("Top: {0},   Bottom: {1},   Side: {2}", topSum, bottomSum, sideSum));

    }

    public void SignalUnitComplete(GameObject coinSpawner)
    {
        currentUnits.Remove(coinSpawner);
        finishedUnits.Add(coinSpawner);
        //print("SignalUnitComplete: " + coinSpawner.name + "     CurrentUnits: " + currentUnits.Count.ToString());

        if (currentUnits.Count == 0 && testingStarted)
        {
            testingComplete = true;
            print("Testing sequence completed");
            SumAndPrint();
            print("Side drop percent: " + CalculateSideDropPercent().ToString());
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    public float CalculateSideDropPercent()
    {
        float result = 0, total = 0;
        foreach (GameObject testingUnit in finishedUnits)
        {
            SideDetectionController detector = testingUnit.GetComponentInChildren<SideDetectionController>();
            result += detector.getSideCount();
            total += detector.getTopCount() + detector.getSideCount() + detector.getBottomCount();
        }
        //print("checking result and total: " + result.ToString() + "   " + total.ToString());
        //print("checking result/total: " + (result / total).ToString());
        return  (result / total);
    }


}