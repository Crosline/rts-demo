using System;
using UnityEngine;

public class Grid<T> {

    public event EventHandler<GridEventArgs> OnGridValueChanged;
    public class GridEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int _width;
    private int _height;
    private Vector3Int _offset;

    private float _cellSize;

    private T[,] _gridArray;


    public Grid(int width, int height, Vector3Int offset, float cellSize = 32f, bool debug = false) {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._offset = offset;

        _gridArray = new T[_width, _height];

        if (debug)
            DrawGrid();
    }

    private void DrawGrid() {
        for (int i = 0; i < _gridArray.GetLength(0); i++) {
            for (int j = 0; j < _gridArray.GetLength(1); j++) {
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, 100f);
    }

    public int GetWidth() {
        return _width;
    }

    public int GetHeight() {
        return _height;
    }

    public float GetCellSize() {
        return _cellSize;
    }

    public bool WithinBounds(int x, int y) {
        return !(x < 0 || y < 0 || x >= _width || y >= _height);
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * _cellSize + _offset;
    }

    public bool GetXY(Vector3 worldPos, out int x, out int y) {
        x = Mathf.FloorToInt((worldPos - _offset).x / _cellSize);
        y = Mathf.FloorToInt((worldPos - _offset).y / _cellSize);


        return WithinBounds(x, y);
    }

    private void SetValue(int x, int y, T value, bool check = false) {
        if (!check)
            if (!WithinBounds(x, y)) return;

        _gridArray[x, y] = value;
        TriggerGridChanged(x, y);
    }
    public void TriggerGridChanged(int x, int y) {
        OnGridValueChanged?.Invoke(this, new GridEventArgs { x = x, y = y });
    }

    private void SetValue(Vector3 worldPos, T value) {

        if (GetXY(worldPos, out int x, out int y))
            SetValue(x, y, value, true);

    }

    public T GetValue(int x, int y) {
        if (WithinBounds(x, y)) {
            return _gridArray[x, y];
        } else {
            return default(T);
        }
    }


    public void SetValueWithSize(int x, int y, int width, int height, T value, bool check = false) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                SetValue(i + x, j + y, value, true);
            }
        }
    }

    public bool isFull(int x, int y, int width, int height) {

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                var temp = GetValue(i + x, j + y);
                if (temp != null || !WithinBounds(i + x, j + y))
                    return true;
            }
        }

        return false;
    }



}
