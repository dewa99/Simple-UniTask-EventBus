using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = this.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(OnClick);
    }

    async void OnClick()
    {
        button.interactable = false;
        await new PlaySquareAnimation().Publish();
        button.interactable = true;
    }
}
