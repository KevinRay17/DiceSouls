using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public AnimationCurve easeInCurve;

    public static List<GameObject> RolledDiceP1 = new List<GameObject>();


    public  List<GameObject> P1DicePool = new List<GameObject>();
    public static List<GameObject> P2DicePool = new List<GameObject>();
    public  List<GameObject> P1Hand = new List<GameObject>();
    public static List<GameObject> P2Hand = new List<GameObject>();


    Transform P1DieSpawnPoint;
    Transform P2DieSpawnPoint;

    //die hand positions
    Transform[] P1DieHandPos = new Transform[10];

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
    // Start is called before the first frame update
    void Start()
    {

        //Search for spawn
        P1DieSpawnPoint = GameObject.Find("P1DieSpawnPoint").transform;
        // P2DieSpawnPoint = GameObject.Find("P2DieSpawnPoint").transform;

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




        for (int i = 0; i < MyDeck.Deck.Count; i++)
        {
            P1DicePool.Add(MyDeck.Deck[i]);
        }
        P1CurrentHealth = 50;
        P2CurrentHealth = 50;


        StartCoroutine(FirstDraw());
    }

    // Update is called once per frame
    void Update()
    {
       




    }

    public void Resolve()
    {
        foreach (GameObject d in RolledDiceP1)
        {
           P1AttackTotal+= d.GetComponent<CubeScript>().rollNumber;
        }

        StartCoroutine(DamageP2());

    }

    IEnumerator DamageP2()
    {
        float newHealth = P2CurrentHealth - P1AttackTotal;
        float oldHealth = P2CurrentHealth;
        float t = 0;
        while(t < 1)
        {
            P2CurrentHealth = Mathf.LerpUnclamped(oldHealth, newHealth, easeInCurve.Evaluate(t));
            P2HealthBar.fillAmount = P2CurrentHealth / 50;
            t += Time.deltaTime * 2;
            yield return 0;
        }

        P2CurrentHealth = newHealth;
        P1AttackTotal = 0;
    }


    private void DrawDie()
    {

        //if I have dice remaining
        if (P1DicePool.Count > 0) {
            //if I dont exceed max hand size
            if (P1Hand.Count < 10) {
                //pull a random card from deck
                int pull = Random.Range(0, P1DicePool.Count);
                //P1Hand.Add(P1DicePool[pull]);
                StartCoroutine(RollToHand(P1DicePool[pull]));
               
                
                P1DicePool.RemoveAt(pull);
            }
        }


    }


    IEnumerator RollToHand(GameObject newDie)
    {
        GameObject newHandDie = Instantiate(newDie, P1DieSpawnPoint.position, Quaternion.identity);
        P1Hand.Add(newHandDie); 
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

    IEnumerator FirstDraw()
    {
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
        yield return new WaitForSeconds(.1f);
        DrawDie();
    }

}
