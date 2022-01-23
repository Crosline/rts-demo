using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : GridObject {

    [SerializeField]
    private int damage;

    [SerializeField]
    private Animator animator;

    public void Move(GridObject gridObject) {

    }

    public void Hit(GridObject gridObject) {

        if (damage == 0)
            damage = 10;

        Move(gridObject);
        StartCoroutine(HitCoroutine(gridObject));
    }


    IEnumerator HitCoroutine(GridObject gridObject) {

        while (gridObject != null) {
            yield return new WaitUntil(IsSoldierClose);


            gridObject.Damage(damage);
            animator?.Play("Attack");
            gridObject.SpriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.5f);

            gridObject.SpriteRenderer.color = Color.white;

            yield return new WaitForSeconds(2f);
        }


        bool IsSoldierClose() => Vector3.Distance(transform.position, gridObject.transform.position) < GridManager.Instance.CellSize;
    }
}
