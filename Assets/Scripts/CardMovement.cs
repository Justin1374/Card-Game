using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPositon; //Mouse pointer postion
    private Vector3 originalPanelLocalPositon; //Original locaton of card
    private Vector3 originalScale;
    private int currentState = 0; //0 is default state, as in state before any changes were made
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    HandManager playerHand;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    private BattleSystem battleSystem;

    void Start()
    {
        playerHand = FindObjectOfType<HandManager>();
        battleSystem = FindObjectOfType<BattleSystem>();

    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
        
    }

    void Update()
    {
        
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;

            case 2:
                //HandleDragState();
                if (Input.GetMouseButton(0) && battleSystem.state == BattleState.PLAYERTURN) //Check if mouse button is released
                {
                    HandleDragState();
                    Debug.Log("Selected Card: " + rectTransform.gameObject.GetComponent<CardDisplay>().cardData.cardName);
                    battleSystem.OnAttackButton(rectTransform.gameObject, rectTransform.gameObject.GetComponent<CardDisplay>().cardData.damageMax, 0);
                    //playerHand.PlayCard(rectTransform.gameObject);
                    TransitionToState0();
                }
                else if(Input.GetMouseButton(1) && battleSystem.state == BattleState.PLAYERTURN)
                {
                    Debug.Log("Play Ability");
                    HandleDragState();
                    Debug.Log("Selected Card: " + rectTransform.gameObject.GetComponent<CardDisplay>().cardData.cardName);
                    battleSystem.OnAttackButton(rectTransform.gameObject, rectTransform.gameObject.GetComponent<CardDisplay>().cardData.damageMax, 1);
                    //playerHand.PlayCard(rectTransform.gameObject);
                    TransitionToState0();
                }
                
                break;

            case 3:
                HandlePlayState();
                break;
        }
    }

    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = originalScale; //Reset scale
        rectTransform.localRotation = originalRotation; //Reset Rotation
        rectTransform.localPosition = originalPosition; //Reset Positon
        glowEffect.SetActive(false); //Disable highlight
        playArrow.SetActive(false); //Disable playArrow
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            currentState = 1;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if(currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(currentState == 1 && battleSystem.state == BattleState.PLAYERTURN) 
        {
            currentState = 2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPositon);
            originalPanelLocalPositon = rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.position = Input.mousePosition;

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = playPosition;
                }
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localPosition = new Vector3(originalPosition.x,originalPosition.y + 100,0f);
    }

    private void HandleDragState()
    {
        //Set the card's rotation to 0
        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;



        if(Input.mousePosition.y > cardPlay.y)
        {
            currentState = 2;
            playArrow.SetActive(false);
        }
    }

   


}
