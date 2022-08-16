using Tools;
using UnityEngine;

public class InputView : MonoBehaviour
{
    private SubscriptionProperty<Vector2> _moveDiff;
    private SubscriptionProperty<Vector2> _rotateDiff;
    private SubscriptionProperty<bool> _isFire;

    public void Init(SubscriptionProperty<Vector2> moveDiff, SubscriptionProperty<Vector2> rotateDiff, SubscriptionProperty<bool>  isFire)
    {
        _moveDiff = moveDiff;
        _rotateDiff = rotateDiff;
        _isFire = isFire;

        Cursor.visible = false;
    }

    public void CheckInput()
    {
        CheckMoving();
        CheckRotate();
        CheckShoot();
    }

    private void CheckShoot()
    {
        var isMouseButtonDown = Input.GetMouseButton(0);
        if (isMouseButtonDown != _isFire.Value)
        {
            _isFire.Value = isMouseButtonDown;
            Debug.Log($"IsFire {isMouseButtonDown}");
        }
    }

    private void CheckMoving()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        var direction = new Vector2( vertical, horizontal);

        if (vertical != 0f || horizontal != 0f)
            _moveDiff.Value = direction;
    }

    private void CheckRotate()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        _rotateDiff.Value = new Vector2(mouseX,mouseY);
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}

