using _Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : GridObject {

    public int damage;

    private bool isMoving;

    public void Move(Vector2Int vector2) {

        StopAllCoroutines();

        List<PathNode> paths;

        GridObject temp = GridManager.Instance.grid.GetValue(vector2.x, vector2.y);
        if (temp != null) {
            Vector2Int vec2 = GridManager.Instance.pathfinding.GetClosestNode(temp.x, temp.y);
            paths = GridManager.Instance.pathfinding.FindPath(x, y, vec2.x, vec2.y);
        } else {

            paths = GridManager.Instance.pathfinding.FindPath(x, y, vector2.x, vector2.y);
        }

        StartCoroutine(MoveTarget(paths));
    }


    public void Hit(GridObject gridObject) {

        StopAllCoroutines();

        if (gridObject == this) return;

        if (damage == 0)
            damage = 10;

        Move(GridManager.Instance.pathfinding.GetClosestNode(gridObject.x, gridObject.y));

        StartCoroutine(HitCoroutine(gridObject));
    }

    IEnumerator MoveTarget(List<PathNode> paths) {

        if (paths != null) {

            GridManager.Instance.grid.SetValue(x, y, null);

            this.x = paths.Last().x;
            this.y = paths.Last().y;

            GridManager.Instance.grid.SetValue(x, y, this);


            isMoving = true;

            for (int i = 1; i < paths.Count; i++) {
                Vector3 newPos = GridManager.Instance.grid.GetWorldPosition(paths[i].x, paths[i].y);

                float distance = Vector3.Distance(newPos, transform.position);

                Vector3 startPos = transform.position;

                if (i != paths.Count - 1)
                    newPos = new Vector3(newPos.x - 0.5f, newPos.y + Random.Range(-1f, 1f), newPos.z - 0.5f);

                for (float j = 0; j < distance; j += distance * 10f * Time.deltaTime) {
                    transform.position = Vector3.Lerp(startPos, newPos, j / distance);
                    yield return null;

                }


                yield return null;
            }


            isMoving = false;

        }

    }

    IEnumerator HitCoroutine(GridObject gridObject) {
        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(IsMoving);

        while (gridObject != null) {
            yield return null;

            gridObject.Damage(damage);
            //animator?.Play("Attack");

            if (gridObject == null)
                break;
            gridObject.SpriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            gridObject.SpriteRenderer.color = Color.white;

            yield return new WaitForSeconds(1f);
        }


        bool IsMoving() => !isMoving;
    }
}
