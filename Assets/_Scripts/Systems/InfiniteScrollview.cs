using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrollview : MonoBehaviour {

    [SerializeField]
    private List<Transform> _transforms = new List<Transform>();

    private List<float> _startPos = new List<float>();

    private int _screenHeight;

    // Start is called before the first frame update
    void Start() {
        Init();

    }

    void Init() {
        _screenHeight = Screen.height;

        for (int i = 0; i < _transforms.Count; i++) {
            _startPos.Add(_transforms[i].position.y);
        }
    }

    // Update is called once per frame
    void Update() {
        //CheckResolution();

        if (_transforms[0].position.y < _startPos[1] || _transforms[2].position.y > _startPos[1]) {
            ResetTransforms();
        }


    }

    /*
    private void CheckResolution() {

        if (_screenHeight == Screen.height) return;

        var height = Screen.height;

        Debug.Log("ratio: " + ((float)height / (float)_screenHeight).ToString());

        for (int i = 0; i < _startPos.Count; i++) {
            _startPos[i] *= ((float)_screenHeight / (float)height);
        }

        _screenHeight = height;
    }
    */

    private void ResetTransforms() {
        for (int i = 0; i < _transforms.Count; i++) {
            _transforms[i].position = new Vector3(_transforms[i].position.x, _startPos[i]);
        }
    }
}
