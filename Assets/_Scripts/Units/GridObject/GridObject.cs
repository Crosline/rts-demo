using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridObject : PathNode, IHealth {

    private int health;


    [SerializeField]
    private GridObjectScriptable objectDetails;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer => spriteRenderer;

    public string ObjectName => objectDetails.objName;
    public Sprite ObjectSprite => objectDetails.sprite;
    public int MaxHealth => objectDetails.maxHealth;
    public string ObjectInfo => objectDetails.info;
    public int Width => objectDetails.width;
    public int Height => objectDetails.height;

    private Transform tTransform;

    private void Init() {
        health = MaxHealth;
        spriteRenderer.sprite = ObjectSprite;
        isWalkable = false;
    }

    public void SetGridObjectCoord(int x, int y) {
        this.x = x;
        this.y = y;
    }
    public void SetTransform(Transform transform) {
        this.tTransform = transform;
    }
    public void ClearTransform(int x, int y) {
        tTransform = null;
    }
    public bool CanBuild() {
        return tTransform == null;
    }

    public void SetObjectDetails(GridObjectScriptable objectDetails) {
        this.objectDetails = objectDetails;
        Init();
    }

    public void Heal(int healAmount) {
        if (healAmount <= 0) return;

        health += healAmount;

        if (health > MaxHealth)
            health = MaxHealth;
    }
    public void Die() {
        if (this.transform.CompareTag("Soldier"))
            return;
        Destroy(this.gameObject);
    }

    public void Damage(int damageAmount) {
        if (damageAmount <= 0) return;

        health -= damageAmount;

        if (health < 0)
            health = 0;

        Die();
    }


}
