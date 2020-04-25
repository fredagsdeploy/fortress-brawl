using System.Collections;
using System.Collections.Generic;
using Races;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonsController : MonoBehaviour
{
    
    public Button iconButton;
    public ConstructionManager constructionManager;
    public List<ConstructionInfo> constructionInfos;
    private List<Button> _buttons = new List<Button>();
    
    public void ConstructionUnitSelected()
    {
        CleanupButtons();
        constructionInfos.ForEach(info =>
        {
            var button = Instantiate(iconButton, transform);
            button.image.sprite = info.sprite;
            button.onClick.AddListener((() => constructionManager.StartPlacing(info)));
            _buttons.Add(button);
        });
    }

    private void CleanupButtons()
    {
        foreach (Button b in _buttons)
        {
            Debug.Log(b.name);
            Destroy(b.gameObject);
        }
        _buttons.Clear();
    }

    public void ConstructionUnitDeselected()
    {
        CleanupButtons();
    }
}
