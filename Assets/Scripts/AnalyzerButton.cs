using UnityEngine;

public class AnalyzerButton : MonoBehaviour
{
    [SerializeField] private GasAnalyzer _analyzer;

    private bool _isPressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger") || other.CompareTag("Controller"))
        {
            _isPressed = true;
            _analyzer.OnButtonPress(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finger") || other.CompareTag("Controller"))
        {
            _isPressed = false;
            _analyzer.OnButtonPress(false);
        }
    }
}
