using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ActionMenu : MonoBehaviour
{
    public static UI_ActionMenu instance;

    public GameObject ActionMenu;
    public GameObject TargetMenu;
    public GameObject BlockMenu;
    public GameObject BlockMenu_Counteraction;
    public GameObject RevealMenu;
    public GameObject ExchangeMenu;
    public GameObject LoseMenu;
    public GameObject RestartMenu;

    List<GameObject> menus;

    public string currentPlayer { get; private set; }
    Action currentAction;

    int opportunityNum;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        menus = new List<GameObject>
        { ActionMenu, TargetMenu, BlockMenu, BlockMenu_Counteraction, RevealMenu, ExchangeMenu, LoseMenu, RestartMenu };
    }

    ///Human Event Readers//////////////////////////////////
    void HumanAction(string playerName)
    {
        currentPlayer = playerName;
        ActivateMenu(ActionMenu);
    }

    void ChooseTarget(string playerName)
    {
        currentPlayer = playerName;
        ActivateMenu(TargetMenu);
    }

    void ActionOver(string blocker)
    {
        if (!ActionMenu.activeSelf && !TargetMenu.activeSelf) return;
        ActionMenu.SetActive(false);
        TargetMenu.SetActive(false);
    }

    void HumanBlockOpportunity(string playerName)
    {
        currentPlayer = playerName;
        ActivateMenu(BlockMenu);
    }
    void HumanBlockOpportunity_Counteraction(string playerName)
    {
        currentPlayer = playerName;
        ActivateMenu(BlockMenu_Counteraction);
    }
    void BlockOpportunityOver(string blocker)
    {
        if (BlockMenu.activeSelf)
        {
            BlockMenu.SetActive(false);
            return;
        }
        if (BlockMenu_Counteraction.activeSelf)
        {
            BlockMenu_Counteraction.SetActive(false);
            return;
        }
    }

    void HumanExchange(string playerName)
    {
        currentPlayer = playerName;
        ActivateMenu(RevealMenu);
    }

    void HumanReveal(string playerName)
    {
        currentPlayer = playerName;
        ActivateMenu(RevealMenu);
    }

    void HumanLoseCard(string playerName)
    {
        currentPlayer = playerName;
        ActivateMenu(LoseMenu);
    }

    void HumanRestart(string playerName)
    {
        ActivateMenu(RestartMenu);
    }

    ///Button Functions//////////////////////////////////
    public void ButtonPress_Action(string actionName)
    {
        if (!ActionMenu.activeSelf) return;

        currentAction = ActionManager.instance.GetActionByName(actionName);
        if (currentAction.requiresTarget)
        {
            ChooseTarget(currentPlayer);
        }
        else ActionManager.instance.ChooseAction(currentAction, currentPlayer, "");
    }

    public void ButtonPress_Target(PlayerController target)
    {
        if (!TargetMenu.activeSelf) return;
        ActionManager.instance.ChooseAction(currentAction, currentPlayer, target.playerName);
    }

    public void ButtonPress_BlockOpportunity(string blockType)
    {
        if (!BlockMenu.activeSelf) return;
        if (blockType == "Challenge")
        {
            print("CHALLENGE CLICK");
            TurnManager.instance.Block_Action(currentPlayer, "Challenge");
        }
        else if(blockType == "Counteraction")
        {
            TurnManager.instance.Block_Action(currentPlayer, "Counteraction");
        }
    }

    public void ButtonPress_BlockOpportunity_Counteraction()
    {
        if (!BlockMenu_Counteraction.activeSelf) return;
        TurnManager.instance.Block_Counteraction(currentPlayer);
    }

    public void ButtonPress_RevealCard(CardController card)
    {
        if (!RevealMenu.activeSelf) return;
        card.revealed = true;
        TurnManager.instance.RevealCard(currentPlayer, card.cardName);
        RevealMenu.SetActive(false);
    }

    public void ButtonPress_LoseInfluence(CardController card)
    {
        if (!LoseMenu.activeSelf) return;
        card.active = false;
        PlayersManager.instance.GetPlayerController(currentPlayer).LostCard();
        LoseMenu.SetActive(false);
    }

    public void ButtonPress_RestartGame()
    {
        if (!RestartMenu.activeSelf) return;
        SceneManager.LoadScene(0);
    }

    void ActivateMenu(GameObject menu)
    {
        foreach(GameObject gO in menus)
        {
            if (gO == menu) gO.SetActive(true);
            else gO.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerController_Human.HumanAction += HumanAction;
        PlayerController_Human.HumanBlockOpportunity += HumanBlockOpportunity;
        PlayerController_Human.HumanBlockOpportunity_Counteraction += HumanBlockOpportunity_Counteraction;
        PlayerController_Human.HumanRevealCard += HumanReveal;
        PlayerController_Human.HumanLoseInfluence += HumanLoseCard;
        TurnManager.ChallengeIssued += BlockOpportunityOver;
        //TurnManager.CounteractionIssued += BlockOpportunityOver;
        TurnManager.PlayerSucceeded += BlockOpportunityOver;
        TurnManager.PlayerBlocked += BlockOpportunityOver;
        TurnManager.PlayerSucceeded += ActionOver;
        TurnManager.ActionPerformed += ActionOver;
        PlayersManager.PlayerWon += HumanRestart;
    }

    private void OnDisable()
    {
        PlayerController_Human.HumanAction -= HumanAction;
        PlayerController_Human.HumanBlockOpportunity -= HumanBlockOpportunity;
        PlayerController_Human.HumanBlockOpportunity_Counteraction -= HumanBlockOpportunity_Counteraction;
        PlayerController_Human.HumanRevealCard -= HumanReveal;
        PlayerController_Human.HumanLoseInfluence -= HumanLoseCard;
        TurnManager.ChallengeIssued -= BlockOpportunityOver;
        //TurnManager.CounteractionIssued -= BlockOpportunityOver;
        TurnManager.PlayerSucceeded -= BlockOpportunityOver;
        TurnManager.PlayerBlocked -= BlockOpportunityOver;
        TurnManager.PlayerSucceeded -= ActionOver;
        TurnManager.ActionPerformed -= ActionOver;
        PlayersManager.PlayerWon -= HumanRestart;
    }
}
