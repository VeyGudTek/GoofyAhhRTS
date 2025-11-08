using Source.GamePlay.Services;
using Source.Shared.Utilities;
using UnityEngine;

public class PauseMenuService : MonoBehaviour
{
    [InitializationRequired]
    [SerializeField]
    private GameObject QuitConfirmationObject;
    [InitializationRequired]
    private GamePlayService GamePlayService { get; set; }
    [InitializationRequired]
    private GameObject SettingsMenuObject { get; set; }

    public void InjectDependencies(GamePlayService gamePlayService, GameObject settingsMenuObject)
    {
        GamePlayService = gamePlayService;
        SettingsMenuObject = settingsMenuObject;
    }

    void Start()
    {
        this.CheckInitializeRequired();
    }

    public void OnResume()
    {
        if (SettingsMenuObject == null) return;

        gameObject.SetActive(false);
    }

    public void OnSettings()
    {
        if (SettingsMenuObject == null) return;

        SettingsMenuObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnQuit()
    {
        QuitConfirmationObject.SetActive(true);
    }

    public void OnQuitCancel()
    {
        QuitConfirmationObject.SetActive(false);
    }

    public void OnQuitConfirm()
    {

    }
}
