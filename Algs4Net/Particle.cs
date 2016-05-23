/******************************************************************************
 *  File name :    Particle.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A particle moving in the unit box with a given position, velocity,
 *  Radius, and Mass.
 *
 ******************************************************************************/
using System;
using System.Windows.Media;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Particle</c> class represents a particle moving in the unit box,
  /// with a given position, velocity, Radius, and Mass. Methods are provided
  /// for moving the particle and for predicting and resolvling elastic
  /// collisions with vertical walls, horizontal walls, and other particles.
  /// This data type is mutable because the position and velocity change.</summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/61event">Section 6.1</a> of 
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Particle.java.html">Particle</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Particle : BasicVisual
  {
    private const double INFINITY = double.PositiveInfinity;
    private Color color;      // color - later
    private int count;        // number of collisions so far

    /// <summary>
    /// The <c>X</c> coordinate from the left
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// The <c>Y</c> coordinate from the top
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Velocity along x-coordinate  
    /// </summary>
    public double Vx { get; set; }

    /// <summary>
    /// Velocity along y-coordinate
    /// </summary>
    public double Vy { get; set; }

    /// <summary>
    /// The particle's radius
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    /// The particle's mass
    /// </summary>
    public double Mass { get; set; }

    /// <summary>
    /// Default constructor to be used with member initializers to set member
    /// values
    /// </summary>
    public Particle() {}

    /// <summary>
    /// Initializes a particle with a random position and velocity.
    /// The position is uniform in the unit box; the velocity in
    /// either direciton is chosen uniformly at random. Member initializers
    /// will replace these random values.</summary>
    /// <param name="target">the drawing target</param>
    public Particle(DrawingWindow target) : base(target)
    {
      X = StdRandom.Uniform(0.0, 1.0);
      Y = StdRandom.Uniform(0.0, 1.0);
      Vx = StdRandom.Uniform(-.005, 0.005);
      Vy = StdRandom.Uniform(-.005, 0.005);
      Radius = 0.01;
      Mass = 0.5;
      color = Colors.Black;
    }

    /// <summary>
    /// Moves this particle in a straight line (based on its velocity)
    /// for the specified amount of time.</summary>
    /// <param name="dt">the amount of time to move</param>
    ///
    public void Move(double dt)
    {
      X += Vx * dt;
      Y += Vy * dt;
    }

    /// <summary>
    /// Returns the number of collisions involving this particle with
    /// vertical walls, horizontal walls, or other particles.
    /// This is equal to the number of calls to <see cref="BounceOff"/>,
    /// <see cref="BounceOffVerticalWall"/>, and <see cref="BounceOffHorizontalWall"/>
    /// </summary>
    /// <returns>the number of collisions involving this particle with
    /// vertical walls, horizontal walls, or other particles</returns>
    ///
    public int CollisionCount
    {
      get { return count; }
    }

    /// <summary>
    /// Returns the amount of time for this particle to collide with the specified
    /// particle, assuming no interening collisions.</summary>
    /// <param name="that">the other particle</param>
    /// <returns>the amount of time for this particle to collide with the specified
    /// particle, assuming no interening collisions; 
    /// <c>double.PositiveInfinity</c> if the particles will not collide</returns>
    ///
    public double TimeToHit(Particle that)
    {
      if (this == that) return INFINITY;
      double dx = that.X - this.X;
      double dy = that.Y - this.Y;
      double dvx = that.Vx - this.Vx;
      double dvy = that.Vy - this.Vy;
      double dvdr = dx * dvx + dy * dvy;
      if (dvdr > 0) return INFINITY;
      double dvdv = dvx * dvx + dvy * dvy;
      double drdr = dx * dx + dy * dy;
      double sigma = this.Radius + that.Radius;
      double d = (dvdr * dvdr) - dvdv * (drdr - sigma * sigma);
      // if (drdr < sigma*sigma) Console.WriteLine("overlapping particles");
      if (d < 0) return INFINITY;
      return -(dvdr + Math.Sqrt(d)) / dvdv;
    }

    /// <summary>
    /// Returns the amount of time for this particle to collide with a vertical
    /// wall, assuming no interening collisions.</summary>
    /// <returns>the amount of time for this particle to collide with a vertical wall,
    /// assuming no interening collisions; <c>double.PositiveInfinity</c> if 
    /// the particle will not collide with a vertical wall</returns>
    ///
    public double TimeToHitVerticalWall()
    {
      if (Vx > 0) return (1.0 - X - Radius) / Vx;
      else if (Vx < 0) return (X - Radius) / Vx;
      else return INFINITY;
    }

    /// <summary>
    /// Returns the amount of time for this particle to collide with a horizontal
    /// wall, assuming no interening collisions.</summary>
    /// <returns>the amount of time for this particle to collide with a 
    /// horizontal wall, assuming no interening collisions; <c>Double.PositiveInfinity</c>
    /// if the particle will not collide with a horizontal wall</returns>
    ///
    public double TimeToHitHorizontalWall()
    {
      if (Vy > 0) return (1.0 - Y - Radius) / Vy;
      else if (Vy < 0) return (Y - Radius) / Vy;
      else return INFINITY;
    }

    /// <summary>
    /// Updates the velocities of this particle and the specified particle according
    /// to the laws of elastic collision. Assumes that the particles are colliding
    /// at this instant.</summary>
    /// <param name="that">the other particle</param>
    ///
    public void BounceOff(Particle that)
    {
      double dx = that.X - this.X;
      double dy = that.Y - this.Y;
      double dvx = that.Vx - this.Vx;
      double dvy = that.Vy - this.Vy;
      double dvdr = dx * dvx + dy * dvy;             // dv dot dr
      double dist = this.Radius + that.Radius;   // distance between particle centers at collison

      // normal force F, and in x and y directions
      double F = 2 * this.Mass * that.Mass * dvdr / ((this.Mass + that.Mass) * dist);
      double fx = F * dx / dist;
      double fy = F * dy / dist;

      // update velocities according to normal force
      this.Vx += fx / this.Mass;
      this.Vy += fy / this.Mass;
      that.Vx -= fx / that.Mass;
      that.Vy -= fy / that.Mass;

      // update collision counts
      this.count++;
      that.count++;
    }

    /// <summary>
    /// Updates the velocity of this particle upon collision with a vertical
    /// wall (by reflecting the velocity in the <c>X</c>-direction).
    /// Assumes that the particle is colliding with a vertical wall at this instant.</summary>
    ///
    public void BounceOffVerticalWall()
    {
      Vx = -Vx;
      count++;
    }

    /// <summary>
    /// Updates the velocity of this particle upon collision with a horizontal
    /// wall (by reflecting the velocity in the <c>Y</c>-direction).
    /// Assumes that the particle is colliding with a horizontal wall at this instant.</summary>
    ///
    public void BounceOffHorizontalWall()
    {
      Vy = -Vy;
      count++;
    }

    /// <summary>
    /// Returns the kinetic energy of this particle.
    /// The kinetic energy is given by the formula 1/2 <c>M</c> <c>V</c><sup>2</sup>,
    /// where <c>M</c> is the Mass of this particle and <c>V</c> is its velocity.</summary>
    /// <returns>the kinetic energy of this particle</returns>
    ///
    public double KineticEnergy()
    {
      return 0.5 * Mass * (Vx * Vx + Vy * Vy);
    }

    /// <summary>
    /// Draws the particle as a filled circle
    /// </summary>
    public override void Draw()
    {
      Display.SetPenColor(color);
      if (Visual == null)
      {
        Visual = Display.DrawFilledCircle(X, Y, Radius);
      }
      else
        Display.DrawFilledCircle(Visual, X, Y, Radius);
      Display.SetPenColor();   
    }

    /// <summary>
    /// Returns useful particle info for debugging
    /// </summary>
    /// <returns>position, velocity and collision count</returns>
    public override string ToString()
    {
      return string.Format("[XY={0:F5},{1:F5}, V={2:F5},{3:F5}, C={4}]", X, Y, Vx, Vy, CollisionCount);
    }
  }
}

/******************************************************************************
 *  Copyright 2016, Thai Nguyen.
 *  Copyright 2002-2015, Robert Sedgewick and Kevin Wayne.
 *
 *  This file is part of Algs4Net.dll, a .NET library that ports algs4.jar,
 *  which accompanies the textbook
 *
 *      Algorithms, 4th edition by Robert Sedgewick and Kevin Wayne,
 *      Addison-Wesley Professional, 2011, ISBN 0-321-57351-X.
 *      http://algs4.cs.princeton.edu
 *
 *
 *  Algs4Net.dll is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Algs4Net.dll is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Algs4Net.dll.  If not, see http://www.gnu.org/licenses.
 ******************************************************************************/
