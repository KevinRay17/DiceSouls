using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    public static Roll instance;
    public static List<GameObject> LoadedDice = new List<GameObject>();
    // public List<Rigidbody> DiceRB = new List<Rigidbody>();

    int handDiceMask = 1 << 9;
    int setBoardMask = 1 << 10;
    int loadBoardMask = 1 << 11;

    bool holdingDice;
    public static bool rolledDice;

    //     public List<GameObject> LoadedDice = new List<GameObject>();

    Transform throwPoint;
    Transform throwPoint2;
    public AnimationCurve animCurve;
    public GameObject heldDie;
    Vector3 lastDieLocation;

    public static bool addExplode;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        throwPoint = GameObject.Find("ThrowPoint").transform;
        throwPoint2 = GameObject.Find("ThrowPoint2").transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!rolledDice)
        {

            if (Input.GetMouseButtonDown(0) && holdingDice)
            {
                holdingDice = false;
                var v3 = Input.mousePosition;
                v3.z = 7f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                // Debug.Log(v3.x + ", " + v3.y + ", " + v3.z);


                foreach (GameObject d in LoadedDice)
                {
                    d.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    d.GetComponent<Rigidbody>().useGravity = true;
                    d.GetComponent<Rigidbody>().AddForce((v3 - d.transform.position) * 1.8f, ForceMode.Impulse);
                    d.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(50, 100), Random.Range(50, 100), Random.Range(50, 100)), ForceMode.Impulse);
                }

                StartCoroutine(Wait(.1f));

            }

            if (holdingDice)
            {
                foreach (GameObject d in LoadedDice)
                {
                    if (BattleManager.playerTurn == "P1")
                        d.GetComponent<Rigidbody>().AddForce((throwPoint.position - d.transform.position) / 8, ForceMode.Impulse);
                    else
                        d.GetComponent<Rigidbody>().AddForce((throwPoint2.position - d.transform.position) / 8, ForceMode.Impulse);

                    d.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(50, 100), Random.Range(50, 100), Random.Range(50, 100)), ForceMode.Impulse);
                }
            }

        }


        //Pick Up Dice From Hand
        if (Input.GetMouseButtonDown(0) && heldDie == null)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, handDiceMask) && hit.transform.gameObject.CompareTag(BattleManager.playerTurn) && hit.transform.gameObject.GetComponent<CubeScript>().actions <= BattleManager.playerActions)
            {
                hit.transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
                hit.transform.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(50, 100), Random.Range(50, 100), Random.Range(50, 100)), ForceMode.Impulse);
                heldDie = hit.transform.gameObject;
                lastDieLocation = heldDie.transform.position;
                DieDescription.instance.Deactivate();

            }
        }
        else if (Input.GetMouseButtonDown(1) && heldDie != null)
        {
            StartCoroutine(ReturnToPrevPos(heldDie));
        }
        else if (heldDie != null)
        {
            var v3 = Input.mousePosition;
            v3.z = 7f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            heldDie.transform.position = v3;


            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, setBoardMask) &&
                hit.transform.gameObject.CompareTag(BattleManager.playerTurn) &&
                heldDie.transform.gameObject.GetComponent<CubeScript>().myTags.Contains(CubeScript.dieTags.Set))
            {
                StartCoroutine(SetDie(heldDie, hit.transform));

            }
            else if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, loadBoardMask) &&
              hit.transform.gameObject.CompareTag(BattleManager.playerTurn))
            {
                StartCoroutine(LoadDie(heldDie, hit.transform));
            }

        }


    }
    private void OnMouseOver()
    {
        //pop up ui over dice for info
    }




    IEnumerator LoadDie(GameObject loadDie, Transform loadTransform)
    {
        if (addExplode)
        {
            loadDie.GetComponent<CubeScript>().myTags.Add(CubeScript.dieTags.Explode);
        }
        heldDie = null;
        Vector3 currentPos = loadDie.transform.position;
        float t = 0;
        while (t < 1)
        {

            loadDie.transform.position = Vector3.LerpUnclamped(currentPos,
                 loadTransform.position + Vector3.up / 2, animCurve.Evaluate(t));
            t += Time.deltaTime * 2;
            yield return 0;
        }
        loadDie.GetComponent<Rigidbody>().useGravity = true;
        loadDie.GetComponent<Rigidbody>().velocity = Vector3.zero;
        loadDie.transform.position = loadTransform.position + Vector3.up / 2;
        LoadedDice.Add(loadDie);

        //Remove from Hand
        if (BattleManager.playerTurn == "P1")
        {
            BattleManager.P1Hand.Remove(loadDie);
        }
        else
        {
            BattleManager.P2Hand.Remove(loadDie);
        }

        BattleManager.playerActions -= loadDie.GetComponent<CubeScript>().actions;
        loadDie.GetComponent<CubeScript>().rolled = true;


    }




    //when you set a die
    IEnumerator SetDie(GameObject setDie, Transform setTransform)
    {
        heldDie = null;
        Vector3 currentPos = setDie.transform.position;
        float t = 0;
        while (t < 1)
        {

            setDie.transform.position = Vector3.LerpUnclamped(currentPos,
                 setTransform.position + Vector3.up / 2, animCurve.Evaluate(t));
            t += Time.deltaTime * 2;
            yield return 0;
        }
        setDie.GetComponent<Rigidbody>().useGravity = true;
        setDie.GetComponent<Rigidbody>().velocity = Vector3.zero;
        setDie.transform.position = setTransform.position + Vector3.up / 2;

        //Remove from Hand
        if (BattleManager.playerTurn == "P1")
        {
            BattleManager.P1Hand.Remove(setDie);
        }
        else
        {
            BattleManager.P2Hand.Remove(setDie);
        }


        BattleManager.playerActions -= setDie.GetComponent<CubeScript>().actions;


        //Resolve Set Effects
        setDie.GetComponent<CubeScript>().SetDie();



    }



    //When you put a die down
    IEnumerator ReturnToPrevPos(GameObject returningDie)
    {
        heldDie = null;
        Vector3 currentPos = returningDie.transform.position;
        float t = 0;
        while (t < 1)
        {

            returningDie.transform.position = Vector3.LerpUnclamped(currentPos,
                 lastDieLocation, animCurve.Evaluate(t));
            t += Time.deltaTime * 2;
            yield return 0;
        }
        returningDie.GetComponent<Rigidbody>().useGravity = true;
        returningDie.GetComponent<Rigidbody>().velocity = Vector3.zero;
        returningDie.transform.position = lastDieLocation;


    }


    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        rolledDice = true;
    }

    public void PressedRoll()
    {

        holdingDice = true;

        foreach (GameObject d in LoadedDice)
        {
            d.GetComponent<Rigidbody>().useGravity = false;

            if (d.CompareTag("P1"))
            {
                if (d.GetComponent<CubeScript>().myTags.Contains(CubeScript.dieTags.Attack))
                {
                    BattleManager.RolledAttackDiceP1.Add(d);
                }
                else if (d.GetComponent<CubeScript>().myTags.Contains(CubeScript.dieTags.Defense))
                {
                    BattleManager.RolledDefenseDiceP1.Add(d);
                }
            }
            else if (d.GetComponent<CubeScript>().myTags.Contains(CubeScript.dieTags.Attack))
            {
                BattleManager.RolledAttackDiceP2.Add(d);
            }
            else if (d.GetComponent<CubeScript>().myTags.Contains(CubeScript.dieTags.Defense))
            {
                BattleManager.RolledDefenseDiceP2.Add(d);
            }



        }

    }







}
