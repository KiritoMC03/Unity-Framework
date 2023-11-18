using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Idlers.Zones.Test
{
    public class ResourcesZoneTest
    {
        [Test]
        public void CheckGridZonePatternPositions()
        {
            var tempGO = new GameObject();
            tempGO.transform.position = Vector3.zero;
            GridZonePattern.Preferences prefs = new GridZonePattern.Preferences
            { // Require this values
                cellSize = new Vector3(1, 1, 1),
                center = tempGO.transform,
                xRowSize = 5,
                zRowSize = 5
            };
            GridZonePattern pattern = new GridZonePattern(prefs);

            AreEqualsPositionsLayer(pattern, 0);
            AreEqualsPositionsLayer(pattern, 1);
            AreEqualsPositionsLayer(pattern, 2);
            AreEqualsPositionsLayer(pattern, 3);
            AreEqualsPositionsLayer(pattern, 4);
            AreEqualsPositionsLayer(pattern, 5);
            Object.DestroyImmediate(tempGO);
        }

        private void AreVector3Equals(Vector3 a, Vector3 b)
        {
            Assert.AreEqual((double)a.x, (double)b.x);
            Assert.AreEqual((double)a.y, (double)b.y);
            Assert.AreEqual((double)a.z, (double)b.z);
        }
        
        private void AreEqualsPositionsLayer(GridZonePattern pattern, int layer)
        {
            Vector3 pose0Test = pattern.GetPosition(0 + layer * 25);
            Vector3 pose4Test = pattern.GetPosition(4 + layer * 25);
            Vector3 pose12Test = pattern.GetPosition(12 + layer * 25);
            Vector3 pose20Test = pattern.GetPosition(20 + layer * 25);
            Vector3 pose24Test = pattern.GetPosition(24 + layer * 25);
            
            Vector3 pose0Target = new Vector3(-2, layer, -2);
            Vector3 pose4Target = new Vector3(2, layer, -2);
            Vector3 pose12Target = new Vector3(0, layer, 0);
            Vector3 pose20Target = new Vector3(-2, layer, 2);
            Vector3 pose24Target = new Vector3(2, layer, 2);

            AreVector3Equals(pose0Test, pose0Target);
            AreVector3Equals(pose4Test, pose4Target);
            AreVector3Equals(pose12Test, pose12Target);
            AreVector3Equals(pose20Test, pose20Target);
            AreVector3Equals(pose24Test, pose24Target);
        }
    }
}