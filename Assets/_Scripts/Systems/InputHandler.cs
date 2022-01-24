using _Utilities;
using System;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public static Action OnPressEscapeEvent;

    public bool isActive = false;

    public bool isProductSelected = false;
    public bool isGridObjectSelected = false;

    string selectedProduct;
    GridObject selectedGridObject;


    public SpriteRenderer ghostSprite;

    void Awake() => isActive = false;
    void Start() => ghostSprite.gameObject.SetActive(false);

    void OnEnable() {
        UIManager.OnProductSelectEvent += SelectProduct;
        GridManager.OnGridObjectSelectEvent += SelectGridObject;
        GridManager.OnDeployPositionSet += DeployLocationSet;
    }
    void OnDisable() {
        UIManager.OnProductSelectEvent -= SelectProduct;
        GridManager.OnGridObjectSelectEvent -= SelectGridObject;
        GridManager.OnDeployPositionSet -= DeployLocationSet;
    }

    void Update() {
        if (!isActive) return;

        if (Utils.IsPointerOverUI())
            return;

        var pos = Utils.GetMouseWorldPosition(Camera.main);

        if (isProductSelected) {
            SpawningObject(pos);
        } else {
            if (ghostSprite.gameObject.activeSelf)
                ghostSprite.gameObject.SetActive(false);

        }

        CheckGridObject();

        if (isGridObjectSelected) {
            CheckGridObjectInput(pos);
        }

        if (Input.GetButtonDown("Cancel")) {
            OnPressEscapeEvent?.Invoke();
        }
    }

    /* I was gonna do this as an event but I wont
    private void CheckIsActive(GameManager.GameState gameState) {
        if (gameState == GameManager.GameState.Selecting || !isActive) {
            isActive = true;
        } else if (isActive) {
            isActive = false;
        }
    }
    */

    private void CheckGridObject() {
        if (!Input.GetMouseButtonDown(0)) return;

        if (GridManager.Instance.GetGridObject(Utils.GetMouseWorldPosition())) {
            GameManager.Instance.ChangeState(GameManager.GameState.ExecutingInput);

        } else {
            GameManager.Instance.ChangeState(GameManager.GameState.Selecting);
        }


    }
    private void CheckGridObjectInput(Vector3 pos) {
        if (!Input.GetMouseButtonDown(1)) return;

        if (selectedGridObject.CompareTag("Barracks")) {

            GridManager.Instance.SetDeployLocation(pos);
        } else if (selectedGridObject.CompareTag("Soldier")) {

            GridManager.Instance.SetDeployLocation(pos);
        } else {

            GridManager.Instance.DestroyDeployPointer();
        }
    }

    private void SpawningObject(Vector3 pos) {
        Grid<GridObject> _grid = GridManager.Instance.grid;
        if (_grid.GetXY(pos, out var x, out var y)) {
            var gridObject = GridObjectFactory.Instance.GetGridObject(selectedProduct, x, y);

            Debug.Log("GridObj: " + gridObject);
            var gridObj = gridObject.GetComponent<GridObject>();

            ghostSprite.sprite = gridObj.ObjectSprite;
            ghostSprite.transform.position = _grid.GetWorldPosition(x, y);
            ghostSprite.gameObject.SetActive(true);

            if (_grid.isFull(x, y, gridObj.Width, gridObj.Height)) {
                ghostSprite.color = Color.red;
                ghostSprite.Fade(.5f);
                return;
            } else {
                ghostSprite.color = Color.green;
            }

            if (!Input.GetMouseButtonDown(0)) return;

            isProductSelected = false;

            var obj = Instantiate(gridObject);

            obj.transform.position = _grid.GetWorldPosition(x, y);

            _grid.SetValueWithSize(x, y, gridObj.Width, gridObj.Height, obj.GetComponent<GridObject>());
        }
    }

    private void SelectProduct(string productName) {

        isProductSelected = true;
        selectedProduct = productName;
    }
    private void SelectGridObject(GridObject gridObject) {
        isGridObjectSelected = true;
        selectedGridObject = gridObject;
    }

    private void DeployLocationSet(Vector3 pos) {
        if (selectedGridObject.CompareTag("Barracks")) {
            Barracks barracks = (Barracks)selectedGridObject;

            barracks.SetDeployPos(pos);
        } else if (selectedGridObject.CompareTag("Soldier")) {
            Soldier soldier = (Soldier)selectedGridObject;

            if (GridManager.Instance.grid.GetXY(pos, out int x, out int y)) {
                GridObject hitobj = GridManager.Instance.grid.GetValue(x, y);
                if (hitobj == null)
                    soldier.Move(new Vector2Int(x, y));
                else {
                    soldier.Hit(hitobj);
                }
            }
        }
    }


}
