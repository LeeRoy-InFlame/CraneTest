using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.VRModuleManagement;

// Этот скрипт цепляется на кнопку-объект в сцене
public class CraneButton : MonoBehaviour
{
    public enum ButtonType { Up, Down, Left, Right, Forward, Back }
    [SerializeField] private ButtonType _buttonType;

    // Вешаем этот метод на событие "OnPressDown" кнопки (в инспекторе VIU Button)
    public void OnPressDown()
    {
        switch (_buttonType)
        {
            case ButtonType.Up: CraneSignals.MoveUpDown(1f); break;
            case ButtonType.Down: CraneSignals.MoveUpDown(-1f); break;
            case ButtonType.Left: CraneSignals.MoveEastWest(-1f); break;
            case ButtonType.Right: CraneSignals.MoveEastWest(1f); break;
            case ButtonType.Forward: CraneSignals.MoveNorthSouth(1f); break;
            case ButtonType.Back: CraneSignals.MoveNorthSouth(-1f); break;
        }
    }

    // Вешаем этот метод на событие "OnPressUp"
    public void OnPressUp()
    {
        switch (_buttonType)
        {
            case ButtonType.Up:
            case ButtonType.Down: CraneSignals.MoveUpDown(0f); break;
            case ButtonType.Left:
            case ButtonType.Right: CraneSignals.MoveEastWest(0f); break;
            case ButtonType.Forward:
            case ButtonType.Back: CraneSignals.MoveNorthSouth(0f); break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            OnPressDown();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            OnPressUp();
        }
    }
}

