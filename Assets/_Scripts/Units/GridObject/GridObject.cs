using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridObject : MonoBehaviour, IHealth {

    public int x;
    public int y;

    public int health;


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

    private void Init() {
        health = MaxHealth;
        spriteRenderer.sprite = ObjectSprite;
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

        GridManager.Instance.grid.SetValueWithSize(x, y, Width, Height, null);

        if (transform.CompareTag("Soldier")) {
            PoolManager.Instance.ReleaseObject(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    public void Damage(int damageAmount) {
        if (damageAmount <= 0) return;


        health -= damageAmount;

        if (health <= 0) {
            health = 0;
            Die();
        }

    }


}
