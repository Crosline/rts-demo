using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils {
    public static Vector3 GetMouseWorldPosition() {
        Vector3 temp = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        temp.z = 0f;
        return temp;
    }

    public static Vector3 GetMouseWorldPosition(Camera cam) {
        Vector3 temp = GetMouseWorldPositionWithZ(Input.mousePosition, cam);
        temp.z = 0f;
        return temp;
    }

    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPos;
    }

    public static Vector3 GetDirToMouse(Vector3 fromPosition) {
        Vector3 mousePos = GetMouseWorldPosition();
        return (mousePos - fromPosition).normalized;
    }

    public static Vector3 GetMouseWorldPositionRay(Vector3 screenPosition, Camera worldCamera) {
        Ray ray = worldCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f)) {
            return hit.point;
        } else {
            return Vector3.zero;
        }
    }
    public static Vector3 GetMouseWorldPositionRay(Camera worldCamera) {
        return GetMouseWorldPositionRay(Input.mousePosition, worldCamera);
    }

    public static bool IsPointerOverUI() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        } else {
            PointerEventData pe = new PointerEventData(EventSystem.current);
            pe.position = Input.mousePosition;
            List<RaycastResult> hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pe, hits);
            return hits.Count > 0;
        }
    }

    public static float Parse_Float(string text, float _default) {
        float temp;
        if (!float.TryParse(text, out temp)) {
            temp = _default;
        }
        return temp;
    }

    // Parse a int, return default if failed
    public static int Parse_Int(string text, int _default) {
        int temp;
        if (!int.TryParse(text, out temp)) {
            temp = _default;
        }
        return temp;
    }

    public static int Parse_Int(string text) {
        return Parse_Int(text, -1);
    }


}
