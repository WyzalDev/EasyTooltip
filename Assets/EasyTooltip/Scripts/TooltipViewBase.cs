using UnityEngine;

namespace EasyTooltip
{
    /// <summary>
    /// Base class for tooltip views.
    /// Put it on prefab that handles tooltip of this type and then map it in manager for corresponding tooltipDataBase.
    /// </summary>
    public abstract class TooltipViewBase : MonoBehaviour
    {
        /// <summary>
        /// Setups each tooltip view that inheriting from that class.
        /// Inherited class from this type you use for your prefab
        /// </summary>
        /// <param name="data">Typecast this param to TooltipDataBase type that linked view needs</param>
        public abstract void SetUp(TooltipDataBase data);
    }
}