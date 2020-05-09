using cakeslice;
using Photon.Pun;
using UnityEngine;

namespace Code
{
    public class Selectable : MonoBehaviourPunCallbacks
    {
        private Outline _outline;
        private bool _isSelected = false;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    if (_outline)
                    {
                        _outline.enabled = value;
                    }
                    SendMessage(nameof(ISelectionListener.SelectionChanged), value, SendMessageOptions.DontRequireReceiver);
                }

            }
        }

        public bool IsMine {
            get
            {
                if (photonView != null)
                {
                    return photonView.IsMine;
                }

                return true;
            }
        }

        private void Awake()
        {
            SetupOutline();
        }
    
        private void SetupOutline()
        {
            _outline = GetComponentInChildren<Outline>();
            if (_outline)
            {
                _outline.color = 1; // Green
                _outline.enabled = false;
            
            }
        }
    
        void OnEnable()
        {
            UnitManager.selectables.Add(this);
        }

        void OnDisable()
        {
            UnitManager.selectables.Remove(this);
        }
    }
}
