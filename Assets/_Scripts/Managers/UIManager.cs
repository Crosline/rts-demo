using _Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static Action<string> OnProductSelectEvent;

    [SerializeField]
    private GameObject informationCanvas;

    [SerializeField]
    private GameObject productionCanvas;

    private GridObject selectedBuilding;

    [SerializeField]
    private Text buildingText;
    [SerializeField]
    private Text buildingInfoText;
    [SerializeField]
    private Image buildingImage;

    [SerializeField]
    private Text productText;
    [SerializeField]
    private Text productInfoText;
    [SerializeField]
    private Image productImage;

    void OnEnable() {
        GameManager.OnBeforeStateChanged += CloseInfoCanvas;
        GridManager.OnGridObjectSelectEvent += OpenInfoCanvas;
        InputHandler.OnPressEscapeEvent += CloseInfoCanvas;
    }

    void OnDisable() {
        GameManager.OnBeforeStateChanged -= CloseInfoCanvas;
        GridManager.OnGridObjectSelectEvent -= OpenInfoCanvas;
        InputHandler.OnPressEscapeEvent -= CloseInfoCanvas;
    }

    #region Private Methods
    private void CloseInfoCanvas(GameManager.GameState gameState) {

        if (gameState != GameManager.GameState.Selecting) return;

        CloseInfoCanvas();
    }
    private void CloseInfoCanvas() {
        if (!informationCanvas.activeSelf) return;

        ResetCanvas();
        informationCanvas?.SetActive(false);
    }

    private void OpenInfoCanvas(GridObject building) {
        selectedBuilding = building;

        if (selectedBuilding == null) {
            ResetCanvas();
            return;
        }

        informationCanvas?.SetActive(true);

        SetCanvasValues();


    }

    private void SetCanvasValues() {


        buildingText.text = selectedBuilding.ObjectName;
        buildingInfoText.text = selectedBuilding.ObjectInfo;
        buildingImage.sprite = selectedBuilding.ObjectSprite;
        buildingImage.Fade(1f);

        if (selectedBuilding.CompareTag("Barracks")) {

            Barracks building = (Barracks)selectedBuilding;
            if (building.soldiers.Length > 0) {

                SetProduct(building.soldiers[building.selectedSoldier]);
            } else {

                productionCanvas?.SetActive(false);
            }


        } else {

            productText.text = "";
            productInfoText.text = "";
            productImage.sprite = null;
            productImage.Fade(0f);

            productionCanvas?.SetActive(false);
        }

    }

    private void SetProduct(GridObject product) {

        if (!product.CompareTag("Soldier")) return;

        productionCanvas?.SetActive(true);

        productText.text = product.ObjectName;

        productInfoText.text = product.ObjectInfo;

        productImage.sprite = product.ObjectSprite;

        productImage.Fade(1f);

    }

    private void ResetCanvas() {

        buildingText.text = "";
        buildingInfoText.text = "";
        productText.text = "";
        productInfoText.text = "";


        buildingImage.Fade(0f);
        productImage.Fade(0f);
        informationCanvas?.SetActive(false);
    }

    #endregion

    #region Button Methods

    #region Infinite Scrollview
    public void SelectProduct(string productName) {
        OnProductSelectEvent?.Invoke(productName);

        ResetCanvas();
        informationCanvas?.SetActive(false);
    }
    #endregion

    public void ChangeSoldier(int i) {
        i = Mathf.Clamp(i, -1, 1);

        Barracks building = (Barracks)selectedBuilding;

        building.selectedSoldier += i;

        if (building.selectedSoldier == building.soldiers.Length)
            building.selectedSoldier = 0;
        if (building.selectedSoldier < 0)
            building.selectedSoldier = building.soldiers.Length - 1;

        SetProduct(building.soldiers[building.selectedSoldier]);

    }

    public void SpawnSoldierButton() {

        if (selectedBuilding.CompareTag("Barracks")) {

            Barracks building = (Barracks)selectedBuilding;

            building.SpawnSoldier();
        }
    }

    #endregion
}
