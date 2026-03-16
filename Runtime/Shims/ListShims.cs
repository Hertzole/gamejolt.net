#if !DISABLE_GAMEJOLT && !NET6_0_OR_GREATER // Only needed for older .NET versions, as List<T>.EnsureCapacity was added in .NET 6.0

namespace System.Collections.Generic
{
    internal static class ListShims
    {
        public static void EnsureCapacity<T>(this List<T> list, int capacity)
        {
            if (list.Capacity < capacity)
            {
                list.Capacity = capacity;
            }
        }
    }
}
#endif