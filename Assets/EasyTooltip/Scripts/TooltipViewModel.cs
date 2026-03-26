// Copyright (c) 2026 WyzalDev. All Rights Reserved.
using R3;

namespace EasyTooltip
{
    /// <summary>
    /// ViewModel for tooltip. Storages TooltipData and Shows/Hides
    /// tooltips based on its type inherited from TooltipDataBase
    /// <example>
    /// For using it, just call Show method with correct type or Hide.
    /// In example NameTooltipData is class that inherits TooltipDataBase, and contains name data for tooltips this type
    /// <code>
    /// TooltipViewModel viewModel;
    /// viewModel.Show(new NameTooltipData("Tom"));
    /// </code>
    /// </example>
    /// </summary>
    public class TooltipViewModel
    {
        /// <summary>
        /// If TooltipData.Value is null then tooltip is hided.
        /// If it contains value, then tooltip of this type is showing.
        /// </summary>
        public ReactiveProperty<TooltipDataBase> TooltipData { get; } = new(null);

        /// <summary>
        /// Shows tooltip, based on data param type
        /// </summary>
        /// <param name="data">Pass data param, to display view that linked with this Type</param>
        public void Show(TooltipDataBase data)
        {
            TooltipData.Value = data;
        }

        /// <summary>
        /// Hides tooltip (even if it's already hided)
        /// </summary>
        public void Hide()
        {
            TooltipData.Value = null;
        }
    }
}