// Copyright (c) 2026 WyzalDev. All Rights Reserved.
using UnityEngine;
using TMPro;

namespace EasyTooltip.Example
{
    public class DescriptionTooltipView : TooltipViewBase
    {
        [SerializeField] private TMP_Text _descriptionText;

        public override void SetUp(TooltipDataBase data)
        {
            if (data is not DescriptionTooltipData descriptionData)
                return;

            _descriptionText.text = descriptionData.Description;
        }
    }
}