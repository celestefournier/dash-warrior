using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] Vector2 spawnStartRange;
    [SerializeField] Vector2 spawnEndRange;

    List<Enemy> enemies = new();

    void Start()
    {
        for (int i = 0; i < 6; i++)
            SpawnEnemies();
    }

    void SpawnEnemies()
    {
        int antibug = 0;

        Vector2 randomPos = new Vector2(
            Random.Range(spawnStartRange.x, spawnEndRange.x),
            Random.Range(spawnStartRange.y, spawnEndRange.y)
        );
        RaycastHit2D hit = Physics2D.CircleCast(randomPos, 1.8f, Vector2.zero);

        while (hit.collider != null)
        {
            randomPos = new Vector2(
                Random.Range(spawnStartRange.x, spawnEndRange.x),
                Random.Range(spawnStartRange.y, spawnEndRange.y)
            );
            hit = Physics2D.CircleCast(randomPos, 1.8f, Vector2.zero);

            if (++antibug > 20)
                break;
        }

        var enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        enemy.Init(() => enemies.Remove(enemy));
        enemies.Add(enemy);
    }

    public void StartTurn()
    {
        int enemiesFinished = 0;

        foreach (var enemy in enemies)
        {
            if (enemy == null)
            {
                enemies.Remove(enemy);
                continue;
            }

            enemy.Attack(() =>
            {
                enemiesFinished++;

                if (enemiesFinished >= enemies.Count)
                    gameController.EndTurn(Turn.Enemy);
            });
        }
    }
}