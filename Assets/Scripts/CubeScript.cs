using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    Dictionary<Vector3, int> D6 = new Dictionary<Vector3, int>();

    private Rigidbody rb;
    public bool exploaded;

    GameObject self;
    GameObject d6;

    public int rollNumber;

    void Awake()
    {
         d6 = Resources.Load<GameObject>("Prefabs/d6");

        rb = gameObject.GetComponent<Rigidbody>();
        self = this.gameObject;

        exploaded = false;

        D6[Vector3.up] = 1;
        D6[Vector3.down] = 6;

        D6[Vector3.left] = 2;
        D6[Vector3.right] = 5;

        D6[Vector3.forward] = 3;
        D6[Vector3.back] = 4;

        BattleManager.RolledDiceP1.Add(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current number: " + getNumber());

       // Debug.Log(d6.name);
        //if die has stopped rolling, do an effect
        if (Roll.rolledDice && rb.velocity == Vector3.zero)
        {
            rollNumber = getNumber();
           
            //exploading die
            if (getNumber() == 6 && !exploaded && gameObject.CompareTag("ExploadingDie"))
            {
                GameObject newExpDie = Instantiate(d6, this.transform.position, Quaternion.identity);
                newExpDie.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(50, 100), Random.Range(50, 100), Random.Range(50, 100)), ForceMode.Impulse);

                StartCoroutine(GiveNewDieTag("ExploadingDie", newExpDie));      
                exploaded = true;
            }
        }

    }

    public int getNumber() {
        // here I would assert lookup is not empty, epsilon is positive and larger than smallest possible float etc
        // Transform reference up to object space
        float epsilonDeg = 45f;
        Vector3 referenceVectorUp = Vector3.up;
        Vector3 referenceObjectSpace = transform.InverseTransformDirection(referenceVectorUp);
    
        // Find smallest difference to object space direction
        float min = float.MaxValue;
        Vector3 minKey = new Vector3(0,0,0);
        foreach (Vector3 key in D6.Keys) {
            float a = Vector3.Angle(referenceObjectSpace, key);
            if (a <= epsilonDeg && a < min) {
            min = a;
            minKey = key;
            }
        }
        return (min < epsilonDeg) ? D6[minKey] : -1; // -1 as error code for not within bounds
    }

    IEnumerator GiveNewDieTag(string tagName, GameObject newDie)
    {
        yield return new WaitForSeconds(.1f);
        newDie.tag = tagName;
        Debug.Log(tagName);

    }

    IEnumerator GiveTag(string tagName)
    {
        yield return 0;
        gameObject.tag = tagName;
    }




}



