using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDeck : MonoBehaviour
{

    public static List<GameObject> Deck = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Dynamite"));//d6
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/Gangplanks"));//d4
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/PowderKeg")); //d8
        Deck.Add(Resources.Load<GameObject>("Prefabs/Demolitionist/BlackPowder")); //d4

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
