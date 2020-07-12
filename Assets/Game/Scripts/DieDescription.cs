using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DieDescription : MonoBehaviour
{
    public static DieDescription instance;

    int handDiceMask = 1 << 9;
    public GameObject DescriptorPanel;

    public TMP_Text Title, Description, DieSize, Property, ActionCost;

    bool viewing;
    CubeScript viewedScript;
    public Camera cam;
    public RectTransform CanvasRect;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DescriptorPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, handDiceMask) && hit.transform.gameObject.CompareTag(BattleManager.playerTurn) && Roll.instance.heldDie == null)
        {
            viewedScript = hit.transform.gameObject.GetComponent<CubeScript>();
            viewing = true;
            WritePanel();
            DescriptorPanel.SetActive(true);

            Vector2 ViewportPosition = cam.WorldToViewportPoint(hit.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

           // DescriptorPanel.transform.position =hit.transform.position;
            DescriptorPanel.transform.position = WorldObject_ScreenPosition;

        } else
        {
            DescriptorPanel.SetActive(false);
        }


       

    }

    public void Deactivate()
    {
        viewing = false;
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
    }
    private void OnMouseExit()
    {
        
    }
}
