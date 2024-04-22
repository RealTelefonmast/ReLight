using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TeleCore;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace ReLight.ComputeGrid
{
    public class ComputeGrid<T> : IExposable, IDisposable where T : unmanaged
    {
        private Map _map;

        private ComputeBuffer _buffer;
        private NativeArray<T> _arr;
        
        public ComputeGrid(Map map)
        {
            _map = map;
            _buffer = new ComputeBuffer(map.cellIndices.NumGridCells, Marshal.SizeOf<T>());
            _arr = new NativeArray<T>(map.cellIndices.NumGridCells, Allocator.Persistent);
        }
        
        public void SetData(IntVec3 c, T data)
        {
            var index = c.Index(_map);
            _arr[index] = data;
            _buffer.SetData(_arr, index, index, 1);
        }
        
        public void SetData(int index, T data)
        {
            _arr[index] = data;
            _buffer.SetData(_arr, index, index, 1);
        }

        public void SetBufferOn(ComputeShader shader, int kernel, string tag)
        {
            shader.SetBuffer(kernel, tag, _buffer);
        }
        
        public void ExposeData()
        {
            
        }
        
        #region Disposal

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _map?.Dispose();
                _buffer?.Dispose();
                _arr.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ComputeGrid()
        {
            Dispose(false);
        }

        #endregion
    }
}
