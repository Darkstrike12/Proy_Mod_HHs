using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BarrierTile : GridTile
{
    [SerializeField] bool destroyAll;
    [SerializeField] float destroyDelay;
    [SerializeField] EnemyData.EnemyCategories[] destroyCategories;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (IsEnemyOnTile(collision, out Base_Enemy enemy))
        //{
        //    //switch (destroyAll)
        //    //{
        //    //    case false:
        //    //        if (destroyCategories.Contains(collidingEnemy.EnemyData.EnemyCategory))
        //    //        {
        //    //            Destroy(collidingEnemy, destroyDelay);
        //    //            GameManager.Instance.UpdateStatsOnEnemyOutOfGameArea();
        //    //        }
        //    //        break;
        //    //    case true:
        //    //        Destroy(collidingEnemy, destroyDelay);
        //    //        GameManager.Instance.UpdateStatsOnEnemyOutOfGameArea();
        //    //        break;
        //    //}
        //    Destroy(enemy, destroyDelay);
        //    GameManager.Instance.UpdateStatsOnEnemyOutOfGameArea();
        //}
        if (collision.gameObject.TryGetComponent(out Base_Enemy enemy))
        {
            Destroy(enemy.gameObject, destroyDelay);
            GameManager.Instance.UpdateStatsOnEnemyOutOfGameArea();
            Debug.LogWarning("Enemy Out Of Game Are", gameObject);
        }
    }

}
