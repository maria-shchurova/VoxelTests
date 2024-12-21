using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Assets.Scripts.Jobs
{
    [BurstCompile]
    public struct ClearFloat3ArrayJob : IJobParallelFor
    {
        public NativeArray<float3> Array;

        public void Execute(int index)
        {
            Array[index] = float3.zero;
        }
    }
    
    [BurstCompile]
    public struct ClearIntArrayJob : IJobParallelFor
    {
        public NativeArray<int> Array;

        public void Execute(int index)
        {
            Array[index] = 0;
        }
    }
    
}