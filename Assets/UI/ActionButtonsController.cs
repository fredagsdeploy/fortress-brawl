using System;
using System.Collections;
using System.Collections.Generic;
using Races;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonsController : MonoBehaviour
{
    
    public Button iconButton;
    public Sprite cancelSprite;
    public Sprite buildSprite;
    private ConstructionManager _constructionManager;
    private List<Button> _buttons = new List<Button>();
    private Race _constructionRace;
    private bool _buildActionActive = false;

    void Start()
    { 
        _constructionManager = ConstructionManager.Instance;
    }

    void Update()
    {
        if (_buildActionActive && Input.GetKeyDown(KeyCode.Escape))
        {
            BuildActionCancelled();
        }
    }
    
    public void BuildingSelected()
    {
        CleanupButtons();
    }

    public void BuildActionSelected()
    {
        _buildActionActive = true;
        CleanupButtons();
        _constructionRace.GetBuildingInfos().ForEach(info =>
        {
            var button = Instantiate(iconButton, transform);
            button.image.sprite = info.sprite;
            button.onClick.AddListener((() => _constructionManager.StartPlacing(info)));
            _buttons.Add(button);
        });
        var cancelButton = Instantiate(iconButton, transform);
        cancelButton.image.sprite = cancelSprite;
        cancelButton.onClick.AddListener(BuildActionCancelled);
        _buttons.Add(cancelButton);
    }

    public void BuildActionCancelled()
    {
        _buildActionActive = false;
        CleanupButtons();
        SetupBuildAction();
    }
    
    public void ConstructionUnitSelected(Race race)
    {
        _constructionRace = race;
        SetupBuildAction();
    }

    private void SetupBuildAction()
    {
        var buildButton = Instantiate(iconButton, transform);
        buildButton.image.sprite = buildSprite;
        buildButton.onClick.AddListener(BuildActionSelected);
        _buttons.Add(buildButton);
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

    public void ClearSelection()
    {
        _constructionRace = null;
        CleanupButtons();
    }
}
