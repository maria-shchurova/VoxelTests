using UnityEngine;
using Zenject;

public class BuilderInstaller : MonoInstaller
{
    public GameObject hexCellPrefab;
    public HexGrid hexGrid;
    public HexMesh hexMesh;

    public override void InstallBindings()
    {
        Container.Bind<HexGrid>().FromInstance(hexGrid).AsSingle().NonLazy();
        Container.Bind<HexMesh>().FromInstance(hexMesh).AsSingle().NonLazy();
        Container.Bind<HexCell>().FromComponentInNewPrefab(hexCellPrefab).AsTransient();
    }
}