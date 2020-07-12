using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DieDescription : MonoBehaviour
{
    public static DieDescription instance;

    int handDiceMask = 1 << 9;
    public GameObject DescriptorPanel;

    public TMP_Text Title, Description, DieSize, Property, ActionCost, PlayerTurn, Actions;

    CubeScript viewedScript;
    public Camera cam;
    public RectTransform CanvasRect;
    public Image cardImage, dieType, dieSet, pTurnColor, rollColor,endTurn,actionButton;

    public Color p1Col, p2Col;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DescriptorPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (BattleManager.playerTurn == "P1")
        {
            PlayerTurn.text = "Player 1";
            pTurnColor.color = p1Col;
            rollColor.color = p1Col;
            endTurn.color = p1Col;
            actionButton.color = p1Col;
        }
        else
        {
            PlayerTurn.text = "Player 2";
            pTurnColor.color = p2Col;
            rollColor.color = p2Col;
            endTurn.color = p2Col;
            actionButton.color = p2Col;
        }

        Actions.text = BattleManager.playerActions + " A";
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, handDiceMask) && hit.transform.gameObject.CompareTag(BattleManager.playerTurn) && Roll.instance.heldDie == null)
        {
            viewedScript = hit.transform.gameObject.GetComponent<CubeScript>();
           
            DescriptorPanel.SetActive(true);
            WritePanel();

            if (BattleManager.playerTurn == "P1")
            {
                DescriptorPanel.transform.position = cam.WorldToScreenPoint(hit.transform.position) +(Vector3.right*190);
            } else
            {
                DescriptorPanel.transform.position = cam.WorldToScreenPoint(hit.transform.position) + (Vector3.left*190);
            }

        } else
        {
            DescriptorPanel.SetActive(false);
        }

    }

    public void Deactivate()
    {
       
        DescriptorPanel.SetActive(false);
    }
    void WritePanel()
    {
        string newDescrip = viewedScript.Description.Replace("___", "\n");
        Title.text = viewedScript.Title;
        Description.text = newDescrip;
        DieSize.text = viewedScript.DieSize;
        Property.text = viewedScript.Property;
        ActionCost.text = viewedScript.ActionCost;

        cardImage.sprite = viewedScript.cardImage;
        dieType.sprite = viewedScript.dieType;
        //dieSet.sprite = viewedScript.dieSet;
    }
    private void OnMouseExit()
    {
        
    }
}
