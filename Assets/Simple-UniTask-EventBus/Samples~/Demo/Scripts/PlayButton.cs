using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private int loop = 1;
    void Start()
    {
        button = this.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(OnClick);
    }

    async void OnClick()
    {
        button.interactable = false;
        await new PlaySquareAnimation()
        {
            Loop = loop
        }.Publish();
        button.interactable = true;
    }
}
