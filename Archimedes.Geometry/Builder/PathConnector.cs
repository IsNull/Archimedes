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
        float? bestDist;
        Action combineAction;
        IEnumerable<Vertex> path;
        IEnumerable<Vertex> path2;
        List<Vertex> connectedPath;

        public PathConnector(IEnumerable<Vertex> upath, IEnumerable<Vertex> upath2) {
            path = upath;
            path2 = upath2;
        }

        /// <summary>
        /// Prepares for a new Search
        /// </summary>
        private void Reset(){
            bestDist = null;
            connectedPath = new List<Vertex>(); 
        }

        public IEnumerable<Vertex> ConnectPaths() {
            float dist;
            Reset();

            // avoid error if one of the collections is empty
            if (path.Count() == 0 || path2.Count() == 0) {
                connectedPath.AddRange(path);
                connectedPath.AddRange(path2);
                return connectedPath;
            }

            // Combine
            bestDist = (float)Line2.CalcLenght(path.Last(), path2.First());
            combineAction = Combine;

            // Combine2Reversed
            dist = (float)Line2.CalcLenght(path.Last(), path2.Last());
            if (dist < bestDist) {
                bestDist = dist;
                combineAction = Combine2Reversed;
            }

            // Combine1Reversed
            dist = (float)Line2.CalcLenght(path.First(), path2.First());
            if (dist < bestDist) {
                bestDist = dist;
                combineAction = Combine1Reversed;
            }

            // CombineInvert
            dist = (float)Line2.CalcLenght(path.First(), path2.Last());
            if (dist < bestDist) {
                bestDist = dist;
                combineAction = CombineInvert;
            }

            combineAction.Invoke();
            return connectedPath;
        }


        private void Combine() {
            connectedPath.AddRange(path);
            connectedPath.AddRange(path2);
        }
        private void Combine2Reversed() {
            connectedPath.AddRange(path);
            connectedPath.AddRange(path2.Reverse());
        }
        private void Combine1Reversed() {
            connectedPath.AddRange(path.Reverse());
            connectedPath.AddRange(path2);
        }
        private void CombineInvert() {
            connectedPath.AddRange(path2);
            connectedPath.AddRange(path);
        }


    }
}
