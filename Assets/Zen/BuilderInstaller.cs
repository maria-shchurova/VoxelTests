using UnityEngine;
using Zenject;

public class BuilderInstaller : MonoInstaller
{
    public GameObject hexCellPrefab;
    public World terrainGeneator;

    public override void InstallBindings()
    {
        Container.Bind<World>().FromInstance(terrainGeneator).AsSingle().NonLazy();
        Container.Bind<HexCell>().FromComponentInNewPrefab(hexCellPrefab).AsTransient();
    }
}