using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EnemyManager enemyManager;

    void Start()
    {
        player.StartTurn();
    }

    public void EndTurn(Turn turn)
    {
        if (turn is Turn.Player)
            enemyManager.StartTurn();

        if (turn is Turn.Enemy)
            player.StartTurn();
    }
}

public enum Turn
{
    Player,
    Enemy
}