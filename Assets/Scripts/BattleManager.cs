using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public AnimationCurve easeInCurve;

    public static List<GameObject> RolledAttackDiceP1 = new List<GameObject>();
    public static List<GameObject> RolledAttackDiceP2 = new List<GameObject>();

    public static List<GameObject> RolledDefenseDiceP1 = new List<GameObject>();
    public static List<GameObject> RolledDefenseDiceP2 = new List<GameObject>();

    public static List<GameObject> RolledHealDiceP1 = new List<GameObject>();
    public static List<GameObject> RolledHealDiceP2 = new List<GameObject>();


    public static List<GameObject> P1DicePool = new List<GameObject>();
    public static List<GameObject> P2DicePool = new List<GameObject>();
    public static List<GameObject> P1Hand = new List<GameObject>();
    public static List<GameObject> P2Hand = new List<GameObject>();
    public static List<GameObject> P1DiscardPool = new List<GameObject>();
    public static List<GameObject> P2DiscardPool = new List<GameObject>();




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

    public static string playerTurn = "P1";
    int turnCounter = 0;
    int roundCounter = 1;
    int startPlayer;
    // Start is called before the first frame update
    void Start()
    {

        //Randomize PlayerStart
        startPlayer = Random.Range(1, 3);
        if (startPlayer <= 1)
        {
            playerTurn = "P1";
        } else
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




        for (int i = 0; i < MyDeck.Deck.Count; i++)
        {
            P1DicePool.Add(MyDeck.Deck[i]);
        }

        for (int i = 0; i < MyDeck.Deck.Count; i++)
        {
            P2DicePool.Add(MyDeck.Deck[i]);
        }
        P1CurrentHealth = 50;
        P2CurrentHealth = 50;


        StartCoroutine(FirstDraw());
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(P1AttackTotal);
        Debug.Log(P2AttackTotal);



    }

    public void Resolve()
    {
        if (playerTurn == "P2") 
        {
            //Player 1 Attacks
            foreach (GameObject d in RolledAttackDiceP1)
            {
                P1AttackTotal += d.GetComponent<CubeScript>().rollNumber;
            }
            foreach(GameObject d in RolledDefenseDiceP2)
            {
                P2DefenseTotal += d.GetComponent<CubeScript>().rollNumber;
            }
            //Player 2 Attacks
            foreach (GameObject d in RolledAttackDiceP2)
            {
                P2AttackTotal += d.GetComponent<CubeScript>().rollNumber;
            }
            foreach (GameObject d in RolledDefenseDiceP1)
            {
                P1DefenseTotal += d.GetComponent<CubeScript>().rollNumber;
            }


            //Player 1 Attack Animation
            StartCoroutine(DamageP2ToP1());          
        } else
        {
            //Player 2 Attacks
            foreach (GameObject d in RolledAttackDiceP2)
            {
                P2AttackTotal += d.GetComponent<CubeScript>().rollNumber;
            }
            foreach (GameObject d in RolledDefenseDiceP1)
            {
                P1DefenseTotal += d.GetComponent<CubeScript>().rollNumber;
            }
            //Player 1 Attacks
            foreach (GameObject d in RolledAttackDiceP1)
            {
                P1AttackTotal += d.GetComponent<CubeScript>().rollNumber;
            }
            foreach (GameObject d in RolledDefenseDiceP2)
            {
                P2DefenseTotal += d.GetComponent<CubeScript>().rollNumber;
            }


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
        float newHealth = P2CurrentHealth - Mathf.Max((P1AttackTotal - P2DefenseTotal),0);
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

    }


    private void DrawDie()
    {

        //if I have dice remaining
        if (P1DicePool.Count > 0) {
            //if I dont exceed max hand size
            if (P1Hand.Count < 10) {
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
    }


    IEnumerator RollToHand(GameObject newDie)
    {
        GameObject newHandDie = Instantiate(newDie, P1DieSpawnPoint.position, Quaternion.identity);
        P1Hand.Add(newHandDie);
        newHandDie.tag = "P1";
        float t = 0; 
        while (t < 1)
        {

            newHandDie.transform.position = Vector3.LerpUnclamped(P1DieSpawnPoint.position,
                P1DieHandPos[P1Hand.IndexOf(newHandDie)].position, easeInCurve.Evaluate(t));
            t += Time.deltaTime * 2;
                yield return 0;
        }
        newHandDie.layer = 9;
    }

    IEnumerator RollToHand2(GameObject newDie)
    {
        GameObject newHandDie = Instantiate(newDie, P2DieSpawnPoint.position, Quaternion.identity);
        P2Hand.Add(newHandDie);
        newHandDie.tag = "P2";
        float t = 0;
        while (t < 1)
        {

            newHandDie.transform.position = Vector3.LerpUnclamped(P2DieSpawnPoint.position,
                P2DieHandPos[P2Hand.IndexOf(newHandDie)].position, easeInCurve.Evaluate(t));
            t += Time.deltaTime * 2;
            yield return 0;
        }
        newHandDie.layer = 9;
    }

    IEnumerator FirstDraw()
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
        yield return new WaitForSeconds(.1f);
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
        foreach (GameObject d in Roll.LoadedDice)
        {
            if (playerTurn == "P1")
            {
                P1DiscardPool.Add(d);
            } else
            {
                P2DiscardPool.Add(d);
            }
        }
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
        } else
        {
            playerTurn = "P1";
            turnCounter++;
        }


       
    }

    private void Reset()
    {
        //Reset
        RolledAttackDiceP1.Clear();
        RolledAttackDiceP2.Clear();
        RolledDefenseDiceP1.Clear();
        RolledDefenseDiceP2.Clear();
        RolledHealDiceP1.Clear();
        RolledHealDiceP2.Clear();

        
    }


}
