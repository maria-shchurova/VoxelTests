using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Jobs
{
    public static class Extensions
    {
        public static Vector3[] ToVector3Array(this NativeArray<float3> float3Array)
        {
            if (float3Array == null || float3Array.Length == 0)
                return new Vector3[0];

            Vector3[] vector3Array = new Vector3[float3Array.Length];
            for (int i = 0; i < float3Array.Length; i++)
            {
                vector3Array[i] = new Vector3(float3Array[i].x, float3Array[i].y, float3Array[i].z);
            }

            return vector3Array;
        }

        public static int[] ToIntArray(this NativeArray<int> nativeIntArray)
        {
            if (nativeIntArray == null || nativeIntArray.Length == 0)
                return new int[0];

            int[] intArray = new int[nativeIntArray.Length];
            for (int i = 0; i < nativeIntArray.Length; i++)
            {
                intArray[i] = nativeIntArray[i];
            }

            return intArray;
        }

        public static NativeArray<float3> ToNativeArrayFloat3(this Vector3[] vector3Array, Allocator allocator)
        {
            NativeArray<float3> nativeArray = new NativeArray<float3>(vector3Array.Length, allocator, NativeArrayOptions.UninitializedMemory);
            for (int i = 0; i < vector3Array.Length; i++)
            {
                nativeArray[i] = new float3(vector3Array[i].x, vector3Array[i].y, vector3Array[i].z);
            }
            return nativeArray;
        }
    }
}