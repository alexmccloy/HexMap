using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Amccloy.HexMap3D.Test {
    [TestClass]
    public class CoordinateTests {

        /// <summary>
        /// Test the relationship that Q + R + S = 0
        /// </summary>
        [TestMethod]
        public void CubicCoordinateConstructor() {
            var rand = new Random();
            for (int i = 0; i < 25; i++) {
                var coord = new CubicCoordinate(rand.Next(1000), rand.Next(1000));

                Assert.AreEqual(0, (coord.Q + coord.R + coord.S));
            }
        }

        /// <summary>
        /// Tests that cubic coordinates can be converted correctly to cartesian coordinates
        /// </summary>
        [TestMethod]
        public void CubicToCartesianFlatTop() {
            var valuesToTest = new List<(CubicCoordinate cubic, Point cartesian)>() {
                (new CubicCoordinate(0, 0), new Point(0, 0)), //Center
                
                (new CubicCoordinate(1, 0), new Point(1500, 866)), //Right down
                (new CubicCoordinate(1, -1), new Point(1500, -866)), //Right up
                
                (new CubicCoordinate(0, -1), new Point(0, -1732)), // Up
                (new CubicCoordinate(0, 1), new Point(0, 1732)), // Down
                
                (new CubicCoordinate(-1, 0), new Point(-1500, -866)), //Left up
                (new CubicCoordinate(-1, 1), new Point(-1500, 866)), //Left down
            };

            foreach (var set in valuesToTest) {
                set.cubic.ToCartesian(Orientation.FlatTop, 1000).ShouldBe(set.cartesian);
            }
        }
        
        /// <summary>
        /// Tests that cubic coordinates can be converted correctly to cartesian coordinates
        /// </summary>
        [TestMethod]
        public void CubicToCartesianPointyTop() {

            var valuesToTest = new List<(CubicCoordinate cubic, Point cartesian)>() {
                (new CubicCoordinate(0, 0), new Point(0, 0)),
            };

            foreach (var set in valuesToTest) {
                set.cubic.ToCartesian(Orientation.PointyTop, 1000).ShouldBe(set.cartesian);
            }        
        }

    }
}