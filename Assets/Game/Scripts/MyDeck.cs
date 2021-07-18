using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyDeck : MonoBehaviour
{
    
    public static List<GameObject> DeckP1 = new List<GameObject>();

    public static List<GameObject> DeckP2 = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //Starting Deck Test 1
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Anchor"));//d6
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Anchor"));//d6
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/PowderKeg")); //d8
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/BlackPowder")); //d4
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/EarthShaker")); //d10
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/CannonBall")); //d12

        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Anchor"));//d6
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Anchor"));//d6
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/PowderKeg")); //d8
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/BlackPowder")); //d4
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/EarthShaker")); //d10
        DeckP2.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/CannonBall")); //d12

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDie()
    {
        DeckP1.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/BlackPowder")); //d4
        Debug.Log("addedDie");
    }
}
