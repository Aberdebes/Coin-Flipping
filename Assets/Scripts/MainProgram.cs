using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainProgram : MonoBehaviour
{
    public float upperLimit, lowerLimit, limitScaler;
    public GameObject testingUnitSpawner;
    private List<GameObject> currentTestRun = new List<GameObject>();
    private bool runningTest = false;
    private int iterationCount;
    private string outputFileName;
    public float startTime;
    public Text upperLimitUIText, lowerLimitUIText, runTimeUIText;

    void Start()
    {
        startTime = Time.time;
        upperLimit = 1;
        lowerLimit = 0.3f;
        limitScaler = 0.01f;


        outputFileName = string.Format("./Output Data {0:yyyy-MM-dd_hh-mm-ss-tt}.txt", System.DateTime.Now);
        //outputFileName = "./Output Data.txt";
        using (StreamWriter writer = new StreamWriter(outputFileName))
        {
            writer.WriteLine("Open in excel");
            writer.WriteLine("Iteration \t ratio \t side-percentage \t offset from 33%");
        }
        //StartCoroutine(SetUpTestRun());

        upperLimitUIText.fontSize = 30;
        lowerLimitUIText.fontSize = 30;
        runTimeUIText.fontSize = 30;
    }

    private void Update()
    {
        upperLimitUIText.text = "Upper Limit: " + upperLimit.ToString();
        lowerLimitUIText.text = "Lower Limit: " + lowerLimit.ToString();
        runTimeUIText.text = "Run Time: " + (Time.time - startTime).ToString();

        if (!runningTest)
        {
            StartCoroutine(SetUpTestRun());
        }

    }

    IEnumerator SetUpTestRun()
    {
        runningTest = true;

        StartCoroutine(RunTestProfile(10, 10, 10, 0, lowerLimit));
        TestingUnitSpawnerController newTestRunSpawnerController = currentTestRun[currentTestRun.Count - 1].GetComponent<TestingUnitSpawnerController>();
        yield return new WaitUntil(() => newTestRunSpawnerController.TestingComplete());

        float lowerSideDropPercentage = newTestRunSpawnerController.CalculateSideDropPercent();
        float lowerSideDropOffset = CalculatePercentageOffset(lowerSideDropPercentage);
        WriteData(lowerLimit, lowerSideDropPercentage, lowerSideDropOffset);
        if (lowerSideDropPercentage > 1f / 3f)
        {
            lowerLimit -= lowerSideDropOffset * limitScaler;
        }
        else { lowerLimit += lowerSideDropOffset * limitScaler; }
        

        //StartCoroutine(RunTestProfile(100, 10, 10, 0, upperLimit));

        //newTestRunSpawnerController = currentTestRun[currentTestRun.Count - 1].GetComponent<TestingUnitSpawnerController>();
        //yield return new WaitUntil(() => newTestRunSpawnerController.TestingComplete());

        //float upperSideDropPercentage = newTestRunSpawnerController.CalculateSideDropPercent();
        //float upperSideDropOffset = CalculatePercentageOffset(upperSideDropPercentage);
        //WriteData(upperLimit, upperSideDropPercentage, upperSideDropOffset);
        //if(upperSideDropPercentage < 1f / 3f)
        //{
        //    upperLimit += upperSideDropOffset * limitScaler;
        //}
        //else { upperLimit -= upperSideDropOffset * limitScaler; }
        

        //if (upperLimit < lowerLimit)
        //{
        //    print("*********************************swapping limits");
        //    print("upperLimit: " + upperLimit);
        //    print("lowerLimit: " + lowerLimit);
        //    float temp = upperLimit;
        //    upperLimit = lowerLimit;
        //    lowerLimit = temp;
        //    limitScaler /= 2; //half scaling every cross over event
        //    //next time i'm trying limitScaler = 1 for the whole time and run for an hour.
        //    print("*********************************new limits");
        //    print("upperLimit: " + upperLimit);
        //    print("lowerLimit: " + lowerLimit);
        //}
        runningTest = false;
    }




    /// <summary>
    /// Every time RunTestProfile is run a new testing field will be spawned and added to a list.
    /// </summary>
    /// <param name="desiredCoinQuantityPerUnit"></param>
    /// <param name="unitsDeep"></param>
    /// <param name="unitsWide"></param>
    /// <param name="startDelay"></param>
    /// <param name="sideRatio"></param>
    /// <returns></returns>
    IEnumerator RunTestProfile(int desiredCoinQuantityPerUnit,
                                int unitsDeep, int unitsWide,
                                float startDelay, float sideRatio)
    {
        print("Side ratio testing: " + sideRatio);
        currentTestRun.Add(Instantiate(testingUnitSpawner, Vector3.zero, Quaternion.identity, transform));
        TestingUnitSpawnerController newTestRunSpawnerController = currentTestRun[currentTestRun.Count - 1].GetComponent<TestingUnitSpawnerController>();

        newTestRunSpawnerController.desiredCoinQuantityPerUnit = desiredCoinQuantityPerUnit;
        newTestRunSpawnerController.unitsDeep = unitsDeep;
        newTestRunSpawnerController.unitsWide = unitsWide;
        newTestRunSpawnerController.startDelay = startDelay;
        newTestRunSpawnerController.sideRatio = sideRatio;
        
        StartCoroutine(newTestRunSpawnerController.SetUpUnits());


        yield return new WaitUntil(() => newTestRunSpawnerController.TestingComplete());
        //print("Testing complete? " + newTestRunSpawnerController.TestingComplete());

        //float sideDropPercentage = newTestRunSpawnerController.CalculateSideDropPercent();
        //float sideDropOffset = CalculatePercentageOffset(sideDropPercentage);
        //print("side drop percent: " + sideDropPercentage);
        //print("offset: " + sideDropOffset);
        print("SetUpTestRun Complete");

    }



    /// <summary>
    /// Distance from 33.33% (calculated from (1f/3f))
    /// </summary>
    /// <param name="currentPercentage"></param>
    /// <returns></returns>
    float CalculatePercentageOffset(float currentPercentage)
    {
        return Mathf.Abs((1f / 3f) - currentPercentage);
    }

    /// <summary>
    /// Writes data into the file named from outputFileName variable
    /// </summary>
    /// <param name="ratio"></param>
    /// <param name="percentage"></param>
    /// <param name="offset"></param>
    void WriteData(float ratio, float percentage, float offset)
    {
        using (StreamWriter writer = new StreamWriter(outputFileName, true))
        {
            writer.WriteLine(++iterationCount + "\t" + ratio + "\t" + percentage + "\t" + offset);
        }
    }

}
