using UnityEngine;

public class RTSCamera : MonoBehaviour {
    [SerializeField]
    private float _screenEdgeBorder;

    [SerializeField]
    private float _screenEdgeMovementSpeed;

    [SerializeField]
    private float _keyboardMovementSpeed;

    [SerializeField]
    private Vector2 _minBoundaries;

    [SerializeField]
    private Vector2 _maxBoundaries;

    private bool _keyboardUsed = false;

    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start() {
        Vector2 startOffset = transform.position;
        _minBoundaries = _minBoundaries * 0.5f + startOffset;
        _maxBoundaries = _maxBoundaries * 0.5f + startOffset;

        transform.position += new Vector3(135, 135, 0);
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update() {

        CheckKeyboardInput();
        if (!_keyboardUsed)
            CheckMousePosition();
        else
            _keyboardUsed = false;

        if (lastPos != transform.position) {
            CheckScreenBoundaries();
        }


        lastPos = transform.position;
    }

    private void CheckMousePosition() {

        if (Utils.IsPointerOverUI()) return;

        Vector3 desiredMove = new Vector3();

        Vector2 mouseInput = Input.mousePosition;

        Rect leftRect = new Rect(0, 0, _screenEdgeBorder, Screen.height);
        Rect rightRect = new Rect(Screen.width - _screenEdgeBorder, 0, _screenEdgeBorder, Screen.height);
        Rect upRect = new Rect(0, Screen.height - _screenEdgeBorder, Screen.width, _screenEdgeBorder);
        Rect downRect = new Rect(0, 0, Screen.width, _screenEdgeBorder);

        desiredMove.x = leftRect.Contains(mouseInput) ? -1 : rightRect.Contains(mouseInput) ? 1 : 0;
        desiredMove.y = upRect.Contains(mouseInput) ? 1 : downRect.Contains(mouseInput) ? -1 : 0;

        if (desiredMove.Equals(Vector2.zero)) return;

        MoveCamera(desiredMove, _screenEdgeMovementSpeed);
    }

    private void CheckKeyboardInput() {
        Vector2 desiredMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (desiredMove.Equals(Vector2.zero)) return;

        desiredMove = desiredMove.normalized;

        _keyboardUsed = true;

        MoveCamera(desiredMove, _keyboardMovementSpeed);

    }

    private void MoveCamera(Vector2 desiredMove, float speed) {
        desiredMove *= speed;
        desiredMove *= Time.deltaTime;
        desiredMove = transform.InverseTransformDirection(desiredMove);

        transform.Translate(desiredMove, Space.Self);

    }

    private void CheckScreenBoundaries() {
        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, _minBoundaries.x, _maxBoundaries.x),
        Mathf.Clamp(transform.position.y, _minBoundaries.y, _maxBoundaries.y),
        transform.position.z);
    }


}
