using UnityEngine;

internal class EntryPoint : MonoBehaviour
{
    [SerializeField]
    private TargetController targetController;
    [SerializeField]
    private ControlsView controlsView;
    [SerializeField]
    private CharController character;
    [SerializeField]
    private ControlsProvider controlsProvider;
    [SerializeField]
    private CameraController cameraController;

    private void Start()
    {
        controlsProvider.Init(controlsView);
        targetController.Init(controlsProvider, character);
        character.Init(targetController, controlsProvider);
        cameraController.Init(character);
    }
}