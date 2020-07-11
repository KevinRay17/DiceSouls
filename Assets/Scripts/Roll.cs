using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{

    public List<GameObject> LoadedDice = new List<GameObject>();
   // public List<Rigidbody> DiceRB = new List<Rigidbody>();

    int handDiceMask = 1 << 9;
    int setBoardMask = 1 << 10;
    int loadBoardMask = 1 << 11;

    bool holdingDice;
    public static bool rolledDice;

//     public List<GameObject> LoadedDice = new List<GameObject>();

    public Transform throwPoint;
    public AnimationCurve animCurve;
    GameObject heldDie;
    Vector3 lastDieLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!rolledDice)
        {   
            if (Input.GetMouseButtonDown(2))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    holdingDice = true;

                    foreach (GameObject d in LoadedDice)
                    {
                        d.GetComponent<Rigidbody>().useGravity = false;
                    }

                }
            }

            if (Input.GetMouseButtonUp(2))
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
                    d.GetComponent<Rigidbody>().AddForce((v3 - d.transform.position) * 1.75f, ForceMode.Impulse);
                    d.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(50, 100), Random.Range(50, 100), Random.Range(50, 100)), ForceMode.Impulse);
                }

                StartCoroutine(Wait(.1f));

            }

            if (holdingDice)
            {
                foreach (GameObject d in LoadedDice)
                {
                    d.GetComponent<Rigidbody>().AddForce((throwPoint.position - d.transform.position) / 8, ForceMode.Impulse);
                    d.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(50, 100), Random.Range(50, 100), Random.Range(50, 100)), ForceMode.Impulse);
                }
            }

        }


        //Pick Up Dice From Hand
        if (Input.GetMouseButtonDown(0) && heldDie == null)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, handDiceMask))
            {
                hit.transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
                hit.transform.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(50, 100), Random.Range(50, 100), Random.Range(50, 100)), ForceMode.Impulse);
                heldDie = hit.transform.gameObject;
                lastDieLocation = heldDie.transform.position;
            }
        } else if (Input.GetMouseButtonDown(1) && heldDie != null)
        {
            StartCoroutine(ReturnToPrevPos(heldDie));
        }else if (heldDie != null)
        {
            var v3 = Input.mousePosition;
            v3.z = 7f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            heldDie.transform.position = v3;


            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, setBoardMask)){
                StartCoroutine(SetDie(heldDie, hit.transform));
            } else if(Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, loadBoardMask)){
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
                 setTransform.position+Vector3.up/2, animCurve.Evaluate(t));
            t += Time.deltaTime * 2;
            yield return 0;
        }
        setDie.GetComponent<Rigidbody>().useGravity = true;
        setDie.GetComponent<Rigidbody>().velocity = Vector3.zero;
        setDie.transform.position = setTransform.position + Vector3.up/2;


        //Resolve Set Effects



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

   
}
