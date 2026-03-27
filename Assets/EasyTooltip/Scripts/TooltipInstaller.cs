// Copyright (c) 2026 WyzalDev. All Rights Reserved.
using Zenject;

namespace EasyTooltip
{
    /// <summary>
    /// You can add custom installers, don't forget about TooltipViewModel in that case.
    /// </summary>
    public class TooltipInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TooltipViewModel>().AsSingle();
        }
    }
}