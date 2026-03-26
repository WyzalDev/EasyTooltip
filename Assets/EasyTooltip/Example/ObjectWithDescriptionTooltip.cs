using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace EasyTooltip.Example
{
    public class ObjectWithDescriptionTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private DescriptionTooltipData _descriptionTooltipData;

        private TooltipViewModel _tooltipViewModel;

        [Inject]
        private void Construct(TooltipViewModel tooltipViewModel)
        {
            _tooltipViewModel = tooltipViewModel;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltipViewModel.Show(_descriptionTooltipData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltipViewModel.Hide();
        }
    }
}