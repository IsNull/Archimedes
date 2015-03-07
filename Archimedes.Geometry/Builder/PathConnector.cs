using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using Vertex = Archimedes.Geometry.Vector2;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Builder
{
    public class PathConnector
    {
        private double? _bestDist;
        private Action _combineAction;
        private readonly IEnumerable<Vertex> _path;
        private readonly IEnumerable<Vertex> _path2;
        private List<Vertex> _connectedPath;

        public PathConnector(IEnumerable<Vertex> upath, IEnumerable<Vertex> upath2) {
            _path = upath;
            _path2 = upath2;
        }

        /// <summary>
        /// Prepares for a new Search
        /// </summary>
        private void Reset(){
            _bestDist = null;
            _connectedPath = new List<Vertex>(); 
        }

        public IEnumerable<Vertex> ConnectPaths() {
            float dist;
            Reset();

            // avoid error if one of the collections is empty
            if (_path.Count() == 0 || _path2.Count() == 0) {
                _connectedPath.AddRange(_path);
                _connectedPath.AddRange(_path2);
                return _connectedPath;
            }

            // Combine
            _bestDist = (float)Line2.CalcLenght(_path.Last(), _path2.First());
            _combineAction = Combine;

            // Combine2Reversed
            dist = (float)Line2.CalcLenght(_path.Last(), _path2.Last());
            if (dist < _bestDist) {
                _bestDist = dist;
                _combineAction = Combine2Reversed;
            }

            // Combine1Reversed
            dist = (float)Line2.CalcLenght(_path.First(), _path2.First());
            if (dist < _bestDist) {
                _bestDist = dist;
                _combineAction = Combine1Reversed;
            }

            // CombineInvert
            dist = (float)Line2.CalcLenght(_path.First(), _path2.Last());
            if (dist < _bestDist) {
                _bestDist = dist;
                _combineAction = CombineInvert;
            }

            _combineAction.Invoke();
            return _connectedPath;
        }


        private void Combine() {
            _connectedPath.AddRange(_path);
            _connectedPath.AddRange(_path2);
        }
        private void Combine2Reversed() {
            _connectedPath.AddRange(_path);
            _connectedPath.AddRange(_path2.Reverse());
        }
        private void Combine1Reversed() {
            _connectedPath.AddRange(_path.Reverse());
            _connectedPath.AddRange(_path2);
        }
        private void CombineInvert() {
            _connectedPath.AddRange(_path2);
            _connectedPath.AddRange(_path);
        }


    }
}
