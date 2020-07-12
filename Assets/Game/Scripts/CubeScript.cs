using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeScript : MonoBehaviour
{
    Dictionary<Vector3, int> D6 = new Dictionary<Vector3, int>();

    private Rigidbody rb;
    public bool exploaded;

    GameObject self;
    GameObject d6;

    public int rollNumber;
    int maxRoll;

    public string Title, Description, DieSize, Property, ActionCost;
    public int actions;
    [HideInInspector]
    public Sprite cardImage, dieType, dieSet;

    GameObject explosion;

    public enum dieTags
    {
        Explode,
        Hide,
        Metal,
        Wood,
        d4,
        d6,
        d8,
        d10,
        d12,
        d20,
        Attack,
        Defense,
        Set,


    }

   public List<dieTags> myTags = new List<dieTags>();


    void Awake()
    {
        if (myTags.Contains(dieTags.d4))
        {
            cardImage = Resources.Load<Sprite>("Sprites/d4");
        } else if (myTags.Contains(dieTags.d6))
        {
            cardImage = Resources.Load<Sprite>("Sprites/d6");
        }
        else if (myTags.Contains(dieTags.d8))
        {
            cardImage = Resources.Load<Sprite>("Sprites/d8");
        }

        if (myTags.Contains(dieTags.Attack))
        {
            dieType = Resources.Load<Sprite>("Sprites/Attack");
        }
        else if (myTags.Contains(dieTags.Defense))
        {
            dieType = Resources.Load<Sprite>("Sprites/Defense");
        }

            //d6 = Resources.Load<GameObject>("Prefabs/d6");

        rb = gameObject.GetComponent<Rigidbody>();
        self = this.gameObject;

        exploaded = false;

        D6[Vector3.up] = 1;
        D6[Vector3.down] = 6;

        D6[Vector3.left] = 2;
        D6[Vector3.right] = 5;

        D6[Vector3.forward] = 3;
        D6[Vector3.back] = 4;

        if (myTags.Contains(dieTags.d4))
        {
            maxRoll = 4;
        } else if (myTags.Contains(dieTags.d6))
        {
            maxRoll = 6;
        } else if (myTags.Contains(dieTags.d8)){
            maxRoll = 8;
        }
        else if (myTags.Contains(dieTags.d10))
        {
            maxRoll = 10;
        }
        else if (myTags.Contains(dieTags.d12))
        {
            maxRoll = 12;
        }
        else if (myTags.Contains(dieTags.d20))
        {
            maxRoll = 20;
        }

        exploaded = false;

    }

    void Start()
    {
        explosion = Resources.Load<GameObject>("Prefabs/Demolitionist/Explosion");

        if (gameObject.tag == "P1")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Mats/Demo");
        }
        else
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Mats/Soul");
        }


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
            if (myTags.Contains(dieTags.Explode) && getNumber() == maxRoll && !exploaded)
            {
                GameObject newExpDie = Instantiate(self, this.transform.position, Quaternion.identity);
                newExpDie.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(50, 100), Random.Range(50, 100), Random.Range(50, 100)), ForceMode.Impulse);
                Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));


                if (myTags.Contains(dieTags.Attack))
                {
                    if (gameObject.tag == "P1")
                    {
                        BattleManager.BonusAttackDiceP1.Add(newExpDie);

                    } else
                    {
                        BattleManager.BonusAttackDiceP2.Add(newExpDie);
                    }
                } else if (myTags.Contains(dieTags.Defense))
                {
                    if (gameObject.tag == "P1")
                    {
                        BattleManager.BonusDefenseDiceP1.Add(newExpDie);

                    }
                    else
                    {
                        BattleManager.BonusDefenseDiceP2.Add(newExpDie);
                    }
                }
                    
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

   /* IEnumerator GiveNewDieTag(string tagName, GameObject newDie)
    {
        yield return new WaitForSeconds(.1f);
        newDie.tag = tagName;
        Debug.Log(tagName);

    }*/

    IEnumerator GiveTag(string tagName)
    {
        yield return 0;
        gameObject.tag = tagName;
    }


    //All Set actions
    public void SetDie()
    {
        if (Title == "Black Powder")
        {
            Roll.addExplode = true;Debug.Log("had");
        }


        if (BattleManager.playerTurn == "P1")
        {
            BattleManager.P1SetDice.Add(gameObject);
        }
        else
        {
            BattleManager.P2SetDice.Add(gameObject);
        }
    }





}



