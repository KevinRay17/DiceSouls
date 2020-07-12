using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public AnimationCurve easeInCurve;

    public static List<GameObject> RolledAttackDiceP1 = new List<GameObject>();
    public static List<GameObject> RolledAttackDiceP2 = new List<GameObject>();
    public static List<GameObject> BonusAttackDiceP1 = new List<GameObject>();
    public static List<GameObject> BonusAttackDiceP2 = new List<GameObject>();

    public static List<GameObject> RolledDefenseDiceP1 = new List<GameObject>();
    public static List<GameObject> RolledDefenseDiceP2 = new List<GameObject>();
    public static List<GameObject> BonusDefenseDiceP1 = new List<GameObject>();
    public static List<GameObject> BonusDefenseDiceP2 = new List<GameObject>();

    public static List<GameObject> RolledHealDiceP1 = new List<GameObject>();
    public static List<GameObject> RolledHealDiceP2 = new List<GameObject>();


    public  List<GameObject> P1DicePool = new List<GameObject>();
    public  List<GameObject> P2DicePool = new List<GameObject>();
    public static List<GameObject> P1Hand = new List<GameObject>();
    public static List<GameObject> P2Hand = new List<GameObject>();
    public  List<GameObject> P1DiscardPool = new List<GameObject>();
    public  List<GameObject> P2DiscardPool = new List<GameObject>();

    public static List<GameObject> P1FieldDice = new List<GameObject>();
    public static List<GameObject> P2FieldDice = new List<GameObject>();

    public static List<GameObject> P1SetDice = new List<GameObject>();
    public static List<GameObject> P2SetDice = new List<GameObject>();




    Transform P1DieSpawnPoint;
    Transform P2DieSpawnPoint;

    //die hand positions
    Transform[] P1DieHandPos = new Transform[10];
    Transform[] P2DieHandPos = new Transform[10];


    float P1HealthMax = 40;
    float P1CurrentHealth;
    float P1AttackTotal;
    float P1DefenseTotal;
    float P1HealTotal;

    float P2HealthMax = 40;
    float P2CurrentHealth;
    float P2AttackTotal;
    float P2DefenseTotal;
    float P2HealTotal;

    public Image P1HealthBar;
    public Image P2HealthBar;

    TMP_Text P1DmgText;
    TMP_Text P2DmgText;
    TMP_Text P1DefenseText;
    TMP_Text P2DefenseText;

    public static string playerTurn = "P1";
    int turnCounter = 0;
    int roundCounter = 1;
    int startPlayer;

    public static int playerActions = 3;



    // Start is called before the first frame update
    void Start()
    {
        //Randomize PlayerStart
        startPlayer = Random.Range(1, 3);
        if (startPlayer <= 1)
        {
            playerTurn = "P1";
        }
        else
        {
            playerTurn = "P2";
        }


        //Search for spawn
        P1DieSpawnPoint = GameObject.Find("P1DieSpawnPoint").transform;
        P2DieSpawnPoint = GameObject.Find("P2DieSpawnPoint").transform;

        P1DieHandPos[0] = GameObject.Find("P1DieHandPos1").transform;
        P1DieHandPos[1] = GameObject.Find("P1DieHandPos2").transform;
        P1DieHandPos[2] = GameObject.Find("P1DieHandPos3").transform;
        P1DieHandPos[3] = GameObject.Find("P1DieHandPos4").transform;
        P1DieHandPos[4] = GameObject.Find("P1DieHandPos5").transform;
        P1DieHandPos[5] = GameObject.Find("P1DieHandPos6").transform;
        P1DieHandPos[6] = GameObject.Find("P1DieHandPos7").transform;
        P1DieHandPos[7] = GameObject.Find("P1DieHandPos8").transform;
        P1DieHandPos[8] = GameObject.Find("P1DieHandPos9").transform;
        P1DieHandPos[9] = GameObject.Find("P1DieHandPos10").transform;


        P2DieHandPos[0] = GameObject.Find("P2DieHandPos1").transform;
        P2DieHandPos[1] = GameObject.Find("P2DieHandPos2").transform;
        P2DieHandPos[2] = GameObject.Find("P2DieHandPos3").transform;
        P2DieHandPos[3] = GameObject.Find("P2DieHandPos4").transform;
        P2DieHandPos[4] = GameObject.Find("P2DieHandPos5").transform;
        P2DieHandPos[5] = GameObject.Find("P2DieHandPos6").transform;
        P2DieHandPos[6] = GameObject.Find("P2DieHandPos7").transform;
        P2DieHandPos[7] = GameObject.Find("P2DieHandPos8").transform;
        P2DieHandPos[8] = GameObject.Find("P2DieHandPos9").transform;
        P2DieHandPos[9] = GameObject.Find("P2DieHandPos10").transform;

        P1DmgText = GameObject.Find("P1AttackText").GetComponent<TMP_Text>();
        P2DmgText = GameObject.Find("P2AttackText").GetComponent<TMP_Text>();
        P1DefenseText = GameObject.Find("P1DefenseText").GetComponent<TMP_Text>();
        P2DefenseText = GameObject.Find("P2DefenseText").GetComponent<TMP_Text>();


        for (int i = 0; i < MyDeck.Deck.Count; i++)
        {
            GameObject instancedDie = Instantiate(MyDeck.Deck[i]);
            P1DicePool.Add(instancedDie);
            instancedDie.SetActive(false);
         
        }

        for (int i = 0; i < MyDeck.Deck.Count; i++)
        {
            GameObject instancedDie = Instantiate(MyDeck.Deck[i]);
            P2DicePool.Add(instancedDie);
            instancedDie.SetActive(false);
        }
        P1CurrentHealth = 50;
        P2CurrentHealth = 50;


        StartCoroutine(DrawHand());
    }

    // Update is called once per frame
    void Update()
    {

        P1DmgText.text = "" + P1AttackTotal;
        P2DmgText.text = "" + P2AttackTotal;
        P1DefenseText.text = "" + P1DefenseTotal;
        P2DefenseText.text = "" + P2DefenseTotal;

    }

    public void Resolve()
    {
        if (playerTurn == "P2")
        {


            //Player 1 Attack Animation
            StartCoroutine(DamageP2ToP1());
        }
        else
        {
           


            //Player 2 Attack Animation
            StartCoroutine(DamageP1ToP2());
        }
        turnCounter = 0;
        roundCounter++;
    }



    //Damage PLayer 2 First then player 1
    IEnumerator DamageP2ToP1()
    {

        //Player 1 attacks player 2
        float newHealth = P2CurrentHealth - Mathf.Max((P1AttackTotal - P2DefenseTotal), 0);
        float oldHealth = P2CurrentHealth;
        float t = 0;
        while (t < 1)
        {
            P2CurrentHealth = Mathf.LerpUnclamped(oldHealth, newHealth, easeInCurve.Evaluate(t));
            P2HealthBar.fillAmount = P2CurrentHealth / 50;
            t += Time.deltaTime * 2;
            yield return 0;
        }
        P2CurrentHealth = newHealth;
        P1AttackTotal = 0;
        P2DefenseTotal = 0;
        //Player 2 attacks player 1
        float newHealth2 = P1CurrentHealth - Mathf.Max((P2AttackTotal - P1DefenseTotal), 0);
        float oldHealth2 = P1CurrentHealth;
        t = 0;
        while (t < 1)
        {
            P1CurrentHealth = Mathf.LerpUnclamped(oldHealth2, newHealth2, easeInCurve.Evaluate(t));
            P1HealthBar.fillAmount = P1CurrentHealth / 50;
            t += Time.deltaTime * 2;
            yield return 0;
        }

        P1CurrentHealth = newHealth2;
        P2AttackTotal = 0;
        P1DefenseTotal = 0;

        Reset();
    }


    //Damage Player 1 first then player 2
    IEnumerator DamageP1ToP2()
    {
        float newHealth = P1CurrentHealth - Mathf.Max((P2AttackTotal - P1DefenseTotal), 0);
        float oldHealth = P1CurrentHealth;
        float t = 0;
        while (t < 1)
        {
            P1CurrentHealth = Mathf.LerpUnclamped(oldHealth, newHealth, easeInCurve.Evaluate(t));
            P1HealthBar.fillAmount = P1CurrentHealth / 50;
            t += Time.deltaTime * 2;
            yield return 0;
        }

        P1CurrentHealth = newHealth;
        P2AttackTotal = 0;
        P1DefenseTotal = 0;
        //Player 1 attacks player 2
        float newHealth2 = P2CurrentHealth - Mathf.Max((P1AttackTotal - P2DefenseTotal), 0);
        float oldHealth2 = P2CurrentHealth;
        t = 0;
        while (t < 1)
        {
            P2CurrentHealth = Mathf.LerpUnclamped(oldHealth2, newHealth2, easeInCurve.Evaluate(t));
            P2HealthBar.fillAmount = P2CurrentHealth / 50;
            t += Time.deltaTime * 2;
            yield return 0;
            P2CurrentHealth = newHealth2;
            P1AttackTotal = 0;
            P2DefenseTotal = 0;
        }

        Reset();
        //startTurn();

    }


    private void DrawDie()
    {

        //if I have dice remaining
        if (P1DicePool.Count > 0)
        {
            //if I dont exceed max hand size
            if (P1Hand.Count < 10)
            {
                //pull a random card from deck
                int pull = Random.Range(0, P1DicePool.Count);
                StartCoroutine(RollToHand(P1DicePool[pull]));
                P1DicePool.RemoveAt(pull);
            }
        } else
        { //shuffle
            foreach (GameObject d in P1DiscardPool)
            {
                
                P1DicePool.Add(d);
            }
            P1DiscardPool.Clear();
            if (P1Hand.Count < 10)
            {
                
                //pull a random card from deck
                int pull = Random.Range(0, P1DicePool.Count);
                StartCoroutine(RollToHand(P1DicePool[pull]));
                P1DicePool.RemoveAt(pull);
            }
        }
    }
    private void DrawDie2()
    {

        //if I have dice remaining
        if (P2DicePool.Count > 0)
        {
            //if I dont exceed max hand size
            if (P2Hand.Count < 10)
            {
                //pull a random card from deck
                int pull = Random.Range(0, P2DicePool.Count);
                StartCoroutine(RollToHand2(P2DicePool[pull]));
                P2DicePool.RemoveAt(pull);
            }
        }
        else
        { //shuffle
            foreach (GameObject d in P2DiscardPool)
            {
                P2DicePool.Add(d);
            }
            P2DiscardPool.Clear();
            if (P2Hand.Count < 10)
            {
                
                //pull a random card from deck
                int pull = Random.Range(0, P2DicePool.Count);
                StartCoroutine(RollToHand2(P2DicePool[pull]));
                P2DicePool.RemoveAt(pull);
            }
        }
    }


    IEnumerator RollToHand(GameObject newDie)
    {
        //grab die and add to hand list, tag it 
        newDie.transform.position =  P1DieSpawnPoint.position;
        newDie.SetActive(true);
        P1Hand.Add(newDie);
        newDie.tag = "P1";
        float t = 0;
        while (t < 1)
        {
            newDie.transform.position = Vector3.LerpUnclamped(P1DieSpawnPoint.position,
                P1DieHandPos[P1Hand.IndexOf(newDie)].position, easeInCurve.Evaluate(t));
            t += Time.deltaTime * 2;
            yield return 0;
        }
        newDie.layer = 9;
    }

    IEnumerator RollToHand2(GameObject newDie)
    {
        //grab die and add to hand list, tag it 
        newDie.transform.position = P2DieSpawnPoint.position;
        newDie.SetActive(true);
        P2Hand.Add(newDie);
        newDie.tag = "P2";
        float t = 0;
        while (t < 1)
        {

            newDie.transform.position = Vector3.LerpUnclamped(P2DieSpawnPoint.position,
                P2DieHandPos[P2Hand.IndexOf(newDie)].position, easeInCurve.Evaluate(t));
            t += Time.deltaTime * 2;
            yield return 0;
        }
        newDie.layer = 9;
    }

    IEnumerator DiscardHand()
    {
        //fade objects
        float t = 0;
        Color lastColor = P1Hand[0].GetComponent<MeshRenderer>().material.color;
        while (t < 1)
        {
            foreach (GameObject d in P1Hand)
            {
                d.GetComponent<MeshRenderer>().material.color = Color.LerpUnclamped(lastColor,
                new Color(lastColor.r, lastColor.g, lastColor.b, 0), easeInCurve.Evaluate(t));
            }
            foreach (GameObject d in P1SetDice)
            {
                d.GetComponent<MeshRenderer>().material.color = Color.LerpUnclamped(lastColor,
                new Color(lastColor.r, lastColor.g, lastColor.b, 0), easeInCurve.Evaluate(t));
            }
            t += Time.deltaTime * 2;
            yield return 0;
        }

        //add to discard and destroy
        foreach (GameObject d in P1Hand)
        {
            P1DiscardPool.Add(d);
            d.SetActive(false);
            d.GetComponent<MeshRenderer>().material.color = lastColor;
        }
        foreach (GameObject d in P1SetDice)
        {
            P1DiscardPool.Add(d);
            d.SetActive(false);
            d.GetComponent<MeshRenderer>().material.color = lastColor;
        }


            P1Hand.Clear();
        yield return new WaitForSeconds(1);
        StartCoroutine(DrawHand());


    }

    IEnumerator DiscardHand2()
    {
        //Fade objects
        float t = 0;
        Color lastColor = P2Hand[0].GetComponent<MeshRenderer>().material.color;
        while (t < 1)
        {
            foreach (GameObject d in P2Hand)
            {
                d.GetComponent<MeshRenderer>().material.color = Color.LerpUnclamped(lastColor,
                 new Color(lastColor.r, lastColor.g, lastColor.b, 0), easeInCurve.Evaluate(t));
            }
            foreach (GameObject d in P2SetDice)
            {
                d.GetComponent<MeshRenderer>().material.color = Color.LerpUnclamped(lastColor,
                new Color(lastColor.r, lastColor.g, lastColor.b, 0), easeInCurve.Evaluate(t));
            }
            t += Time.deltaTime * 2;
            yield return 0;
        }
        //add to discard and destroy
        foreach (GameObject d in P2Hand)
        {
            P2DiscardPool.Add(d);
           d.SetActive(false);
            d.GetComponent<MeshRenderer>().material.color = lastColor;
        }
        foreach (GameObject d in P2SetDice)
        {
            P1DiscardPool.Add(d);
            d.SetActive(false);
            d.GetComponent<MeshRenderer>().material.color = lastColor;
        }
        P2Hand.Clear();

    }

    IEnumerator P1DiscardField()
    {
        new WaitForSeconds(.5f);

        float t = 0;
        
            Color lastColor = P1FieldDice[0].GetComponent<MeshRenderer>().material.color;
            while (t < 1)
            {
                foreach (GameObject d in P1FieldDice)
                {
                    d.GetComponent<MeshRenderer>().material.color = Color.LerpUnclamped(lastColor,
                         new Color(lastColor.r, lastColor.g, lastColor.b, 0), easeInCurve.Evaluate(t));
                }
                t += Time.deltaTime * 2;
                yield return 0;
            }

        foreach (GameObject d in P1FieldDice)
        {
            d.SetActive(false);
            d.GetComponent<MeshRenderer>().material.color = lastColor;
        }

            P1FieldDice.Clear();
    }
    IEnumerator P2DiscardField()
    {
        new WaitForSeconds(.5f);

        float t = 0;

        Color lastColor = P2FieldDice[0].GetComponent<MeshRenderer>().material.color;
        while (t < 1)
        {
            foreach (GameObject d in P2FieldDice)
            {
                d.GetComponent<MeshRenderer>().material.color = Color.LerpUnclamped(lastColor,
                     new Color(lastColor.r, lastColor.g, lastColor.b, 0), easeInCurve.Evaluate(t));
            }
            t += Time.deltaTime * 2;
            yield return 0;
        }

        foreach (GameObject d in P2FieldDice)
        {
            d.SetActive(false);
            d.GetComponent<MeshRenderer>().material.color = lastColor;
        }

        P2FieldDice.Clear();
    }


    IEnumerator DrawHand()
    {
        DrawDie(); DrawDie2();
        yield return new WaitForSeconds(.1f);
        DrawDie(); DrawDie2();
        yield return new WaitForSeconds(.1f);
        DrawDie(); DrawDie2();
        yield return new WaitForSeconds(.1f);
        DrawDie(); DrawDie2();
        yield return new WaitForSeconds(.1f);
        DrawDie(); DrawDie2();

    }




    public void endTurn()
    {
        P1AttackTotal = 0;
        P1DefenseTotal = 0;
        P2AttackTotal = 0;
        P2DefenseTotal = 0;
        P1FieldDice.Clear();
        P2FieldDice.Clear();

        playerActions = 3;

        //Player 1 
        foreach (GameObject d in RolledAttackDiceP1)
        {
            P1AttackTotal += d.GetComponent<CubeScript>().rollNumber;
            P1FieldDice.Add(d);
           
        }
        foreach (GameObject d in BonusAttackDiceP1)
        {
            P1AttackTotal += d.GetComponent<CubeScript>().rollNumber;
            P1FieldDice.Add(d);
        }
        foreach (GameObject d in RolledDefenseDiceP1)
        {
            P1DefenseTotal += d.GetComponent<CubeScript>().rollNumber;
            P1FieldDice.Add(d);
        }
        foreach (GameObject d in BonusDefenseDiceP1)
        {
            P1DefenseTotal += d.GetComponent<CubeScript>().rollNumber;
            P1FieldDice.Add(d);
        }

        //Player 2
        foreach (GameObject d in RolledDefenseDiceP2)
        {
            P2DefenseTotal += d.GetComponent<CubeScript>().rollNumber;
            P2FieldDice.Add(d);           
        }
        foreach (GameObject d in BonusDefenseDiceP2)
        {
            P2DefenseTotal += d.GetComponent<CubeScript>().rollNumber;
            P2FieldDice.Add(d);
        }
        foreach (GameObject d in BonusAttackDiceP2)
        {
            P2AttackTotal += d.GetComponent<CubeScript>().rollNumber;
            P2FieldDice.Add(d);
        }
        foreach (GameObject d in RolledAttackDiceP2)
        {
            P2AttackTotal += d.GetComponent<CubeScript>().rollNumber;
            P2FieldDice.Add(d);
        }
        
        

        /* foreach (GameObject d in Roll.LoadedDice)
         {
             if (playerTurn == "P1")
             {
                 P1DiscardPool.Add(d);
             }
             else
             {
                 P2DiscardPool.Add(d);
             }
         }*/
        Roll.rolledDice = false;
        Roll.LoadedDice.Clear();

        if (turnCounter == 1)
        {
            Resolve();
        }
        else if (playerTurn == "P1")
        {



            playerTurn = "P2";
            turnCounter++;
        }
        else
        {
            playerTurn = "P1";
            turnCounter++;
        }
    }
    /* void startTurn()
     {
         if (playerTurn == "P1")
             DiscardHand();
         else
             DiscardHand2();
     }*/

    private void Reset()
    {

        P1AttackTotal = 0; P1DmgText.text = "" + P1AttackTotal;
        P1DefenseTotal = 0; P1DefenseText.text = "" + P1DefenseTotal;
        P2AttackTotal = 0; P2DmgText.text = "" + P2AttackTotal;
        P2DefenseTotal = 0; P2DefenseText.text = "" + P2DefenseTotal;

        if (P1Hand.Count != 0)
            StartCoroutine(DiscardHand());
        if (P2Hand.Count != 0)
            StartCoroutine(DiscardHand2());
        if (P1FieldDice.Count != 0)
            StartCoroutine(P1DiscardField());
        if (P2FieldDice.Count != 0)
            StartCoroutine(P2DiscardField());

        foreach (GameObject d in RolledAttackDiceP1)
        {
            P1DiscardPool.Add(d);
        }
        foreach (GameObject d in RolledDefenseDiceP2)
        {
            P2DiscardPool.Add(d);
        }
        foreach (GameObject d in RolledAttackDiceP2)
        {
            P2DiscardPool.Add(d);
        }
        foreach (GameObject d in RolledDefenseDiceP1)
        {
            P1DiscardPool.Add(d);
        }

        Roll.addExplode = false;

        //Reset
        RolledAttackDiceP1.Clear();
        RolledAttackDiceP2.Clear();
        RolledDefenseDiceP1.Clear();
        RolledDefenseDiceP2.Clear();
        RolledHealDiceP1.Clear();
        RolledHealDiceP2.Clear();

    }


}
