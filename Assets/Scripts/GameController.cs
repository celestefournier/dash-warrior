using UnityEngine;

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public bool playerTurn = true;

    public void EndTurn(Turn turn)
    {
        print("fim do turno");
        playerTurn = !(turn is Turn.Player);
    }
}

public enum Turn
{
    Player,
    Enemy
}
