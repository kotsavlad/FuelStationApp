using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NdimArray<T>
{
    private T[] data;
    public int[] Shape { get; }

    public NdimArray(params int[] shape)
    {
        var p = 1;
        foreach (var item in shape)
        {
            p *= item;
        }
        data = new T[p];
        Shape = shape;
    }
}
