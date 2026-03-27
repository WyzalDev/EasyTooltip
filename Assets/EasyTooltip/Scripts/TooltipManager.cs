// Copyright (c) 2026 WyzalDev. All Rights Reserved.
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;
using R3;
using Zenject;

namespace EasyTooltip
{
    /// <summary>
    /// Configurable manager for tooltips.
    /// For use configure mappings and tooltipContainer
    /// Put this on GameObject on scene for tooltipHandling
    /// </summary>
    public class TooltipManager : MonoBehaviour
    {
        [Header("ManagerSettings")] [Tooltip("RectTransform that contains all tooltips view windows")]
        [SerializeField] private RectTransform _tooltipContainer;

        [Tooltip("Offset between tooltip window and cursor")]
        [SerializeField] private float _cursorPixelsOffset;

        [Header("Mappings")]
        [Tooltip(
            "Contains mappings between data and view class of each tooltipType.\n\r You need full class name for each data tooltip type")]
        [SerializeField] private List<TooltipPrefabMapping> _mappings;

        private TooltipViewModel _viewModel;
        private IInstantiator _instantiator;

        private TooltipViewBase _currentView;
        private Type _currentTooltipDataType;

        private Dictionary<Type, ObjectPool<TooltipViewBase>> _viewsPoolsDictionary = new();

        private InputAction _mousePositionAction;
        private Vector2 _mousePosition;

        private IDisposable _mouseTrackingSubscription;

        private void Start()
        {
            _mousePositionAction = InputSystem.actions.FindAction("MousePosition");
        }

        [Inject]
        private void Construct(TooltipViewModel viewModel, IInstantiator instantiator)
        {
            _viewModel = viewModel;
            _instantiator = instantiator;

            foreach (var mapping in _mappings)
            {
                var type = Type.GetType(mapping.FullDataClassName);
                if (type != null)
                    _viewsPoolsDictionary[type] = CreatePoolForPrefab(mapping.ViewBase);
            }

            var d = Disposable.CreateBuilder();

            _viewModel.TooltipData.Subscribe(OnTooltipChanged).AddTo(ref d);
            d.RegisterTo(destroyCancellationToken);
        }

        private ObjectPool<TooltipViewBase> CreatePoolForPrefab(TooltipViewBase prefab)
        {
            return new ObjectPool<TooltipViewBase>(
                () => _instantiator.InstantiatePrefabForComponent<TooltipViewBase>(prefab, _tooltipContainer),
                view => view.gameObject.SetActive(true),
                view => view.gameObject.SetActive(false),
                view => Destroy(view.gameObject),
                false,
                2,
                5
            );
        }

        private void OnTooltipChanged(TooltipDataBase data)
        {
            if (_currentView != null && _currentTooltipDataType != null)
            {
                HideTooltip();
                MouseTrackingOff();
            }

            if (data is null)
                return;

            var dataType = data.GetType();

            if (_viewsPoolsDictionary.TryGetValue(dataType, out var pool))
            {
                _currentView = pool.Get();
                _currentTooltipDataType = dataType;

                _currentView.SetUp(data);

                var rect = _currentView.GetComponent<RectTransform>();
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                UpdateTooltipPosition();

                _mouseTrackingSubscription = Observable.EveryUpdate().Subscribe(_ => UpdateTooltipPosition());
            }
            else
                Debug.LogError($"[TooltipManager] Pool for type {dataType} not exists");
        }

        private void HideTooltip()
        {
            _viewsPoolsDictionary[_currentTooltipDataType].Release(_currentView);
            _currentView = null;
            _currentTooltipDataType = null;
        }

        private void MouseTrackingOff()
        {
            _mouseTrackingSubscription?.Dispose();
        }

        private void UpdateTooltipPosition()
        {
            if (_currentView is null)
                return;

            _mousePosition = _mousePositionAction.ReadValue<Vector2>();
            var rect = _currentView.GetComponent<RectTransform>();

            var normalizedX = _mousePosition.x / Screen.width;
            var normalizedY = _mousePosition.y / Screen.height;

            var pivotX = normalizedX > 0.5f ? 1f : 0f;
            var pivotY = normalizedY > 0.5f ? 1f : 0f;

            rect.pivot = new Vector2(pivotX, pivotY);

            var offsetX = pivotX == 0f ? _cursorPixelsOffset : -_cursorPixelsOffset;
            var offsetY = Mathf.Approximately(pivotY, 1f) ? -_cursorPixelsOffset : _cursorPixelsOffset;

            rect.position = _mousePosition + new Vector2(offsetX, offsetY);
        }
    }

    [Serializable]
    public class TooltipPrefabMapping
    {
        public string FullDataClassName;
        public TooltipViewBase ViewBase;
    }
}