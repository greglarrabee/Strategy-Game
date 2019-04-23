using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/*
public class TurnHandler : MonoBehaviour
{
    // Game variables
    // whose turn it is
    private static bool playerTurn;
    // whether the AI has decided its moves
    private bool enemyDecision;
    // The player's unit handler
    GameObject UnitHandle;
    UnitHandler handler;
    // Buttons to pass to it
    Button move;
    Button march;
    Button done;


    void Awake()
    {
        playerTurn = true;
        enemyDecision = false;
        UnitHandle = new GameObject();
        handler = UnitHandle.AddComponent<UnitHandler>();
        handler.giveButtons(move, march, done);
    }

    // For now, just wait a second and return control to the player
    private IEnumerator enemyAI()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        yield return wait;
        enemyDecision = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn)
        {
            handler.playerInput();
            UnitHandler.setUIvis(true);
        }
        else
        {
            UnitHandler.setUIvis(false);
            if(!enemyDecision)
            {
                StartCoroutine(enemyAI());
            }
            else
            {
                enemyDecision = false;
                playerTurn = true;
            }
        }
    }

    // Called when the player's turn is done
    public static void playerEndTurn()
    {
        playerTurn = false;
    }
}
*/