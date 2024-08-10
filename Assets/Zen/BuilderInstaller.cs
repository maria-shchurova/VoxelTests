using UnityEngine;
using Zenject;

public class BuilderInstaller : MonoInstaller
{
    public World terrainGeneator;

    public override void InstallBindings()
    {
        Container.Bind<World>().FromInstance(terrainGeneator).AsSingle().NonLazy();
    }
}