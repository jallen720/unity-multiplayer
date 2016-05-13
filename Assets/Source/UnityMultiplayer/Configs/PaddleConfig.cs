using UnityEngine;
using UnityUtils.ConfigUtils;

public class PaddleConfig : Config<PaddleConfig> {

    [SerializeField]
    private Color[] paddleColors;

    // Static members

    public static Color[] PaddleColors {
        get {
            return Instance.paddleColors;
        }
    }
}