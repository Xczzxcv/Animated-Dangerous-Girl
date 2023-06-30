using UnityEngine;

internal class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Vector3 shift;

    private CharController _character;

    public void Init(CharController character)
    {
        _character = character;
    }

    private void LateUpdate()
    {
        var characterPos = _character.Transform.position;
        var newCameraPos = new Vector3(
            characterPos.x + shift.x,
            shift.y,
            characterPos.z + shift.z
        );
        cam.transform.position = newCameraPos;
    }
}