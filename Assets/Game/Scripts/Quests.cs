using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Quests : MonoBehaviour
{
    public Flowchart flowchart;
    public Flowchart bilgewater;
    public List<string> AvailableQuests = new List<string>();
    public List<string> RelatedQuests = new List<string>();
    public List<string> ActiveQuests = new List<string>();
    //All Quests
    public string DivingForShillings, CoinCollectors, TheUndercurrent, RelicWashedAshore, NoOneForgotten, SkeletonCrew, ExtraQuest1, ExtraQuest2;

    // Start is called before the first frame update
    void Start()
    {

        //flowchart.SetStringVariable("myString", "pog");

        //if (playerCharacter == Pirate)... starterquests
       AvailableQuests.AddRange(new string[]{ DivingForShillings, CoinCollectors, TheUndercurrent,
       RelicWashedAshore, NoOneForgotten, SkeletonCrew});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickAvailableQuest()
    {

    }

    public void PickRelatedQuest()
    {
        int diaOp =  flowchart.GetIntegerVariable("DialogueOption");
        string speaker = flowchart.GetStringVariable("Speaker");
        int numberOfQuests;
        int selectedQuest;


        /*  //Get random number from the list and execute that block
                numberOfQuests = RelatedQuests.Count;
                selectedQuest = Random.Range(0, numberOfQuests); //IDK IF THIS INCLUDES THEM ALL 
                bilgewater.ExecuteBlock(RelatedQuests[selectedQuest]);

                ActiveQuests.Add(RelatedQuests[selectedQuest]);
                AvailableQuests.Remove(RelatedQuests[selectedQuest]);
                RelatedQuests.Remove(RelatedQuests[selectedQuest]);
        */

        //if Bilgewater TavernKeeper = questgiver
        if (speaker == "BilgewaterTavernkeep")
        {
            if (diaOp == 1)
            {
                CheckQuestAvailability(new string[] { DivingForShillings, CoinCollectors });

                //Get random number from the list and execute that block
                numberOfQuests = RelatedQuests.Count;
                selectedQuest = Random.Range(0, numberOfQuests); //IDK IF THIS INCLUDES THEM ALL 
                bilgewater.ExecuteBlock(RelatedQuests[selectedQuest]);

                ActiveQuests.AddRange(new string[] {DivingForShillings, CoinCollectors});
                RemoveAvailableQuests(new string[] {DivingForShillings, CoinCollectors});
                RelatedQuests.Clear();

            } else if (diaOp == 2)
            {
                CheckQuestAvailability(new string[] { TheUndercurrent, RelicWashedAshore });

                //Get random number from the list and execute that block
                numberOfQuests = RelatedQuests.Count;
                selectedQuest = Random.Range(0, numberOfQuests); //IDK IF THIS INCLUDES THEM ALL 
                bilgewater.ExecuteBlock(RelatedQuests[selectedQuest]);

                ActiveQuests.AddRange(new string[] { TheUndercurrent, RelicWashedAshore });
                RemoveAvailableQuests(new string[] { TheUndercurrent, RelicWashedAshore });
                RelatedQuests.Clear();

            } else
            {
                CheckQuestAvailability(new string[] { NoOneForgotten, SkeletonCrew });
                
                // Get random number from the list and execute that block
                 numberOfQuests = RelatedQuests.Count;
                selectedQuest = Random.Range(0, numberOfQuests); //IDK IF THIS INCLUDES THEM ALL 
                bilgewater.ExecuteBlock(RelatedQuests[selectedQuest]);

                ActiveQuests.AddRange(new string[] { NoOneForgotten, SkeletonCrew });
                RemoveAvailableQuests(new string[] { NoOneForgotten, SkeletonCrew });
                RelatedQuests.Clear();
            }
        }
        //Next Character

    }


    void CheckQuestAvailability(string[] str)
    {
        foreach(string s in str)
        {
            if (AvailableQuests.Contains(s))
            {
                RelatedQuests.Add(s);
            }
        }
    }

    void RemoveAvailableQuests(string[] str)
    {
        foreach (string s in str)
        {
            AvailableQuests.Remove(s);
        }
       
    }

    public void ActivateDivingForShillings()
    {

    }

    public void ActivateCoinCollectors()
    {

    }

}
