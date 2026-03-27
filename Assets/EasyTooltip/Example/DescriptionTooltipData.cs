// Copyright (c) 2026 WyzalDev. All Rights Reserved.
using UnityEngine;

namespace EasyTooltip.Example
{
    [CreateAssetMenu(menuName = "EasyTooltip/DescriptionTooltipData")]
    public class DescriptionTooltipData : TooltipDataBase
    {
        [TextArea]
        [SerializeField] public string Description;
    }
}