using UnityEngine;

namespace EasyTooltip.Example
{
    [CreateAssetMenu(menuName = "EasyTooltip/DescriptionTooltipData")]
    public class DescriptionTooltipData : TooltipDataBase
    {
        [TextArea]
        public string Description;
    }
}