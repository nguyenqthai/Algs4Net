/******************************************************************************
 *  File name :    CollisionSystem.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Creates N random particles and simulates their motion according
 *  to the laws of elastic collisions.
 *
 ******************************************************************************/

using System;
using System.Windows;

namespace Algs4Net
{
  /// <summary>
  /// The <c>CollisionSystem</c> class represents a collection of particles
  /// moving in the unit box, according to the laws of elastic collision.
  /// This event-based simulation relies on a priority queue.</summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/61event">Section 6.1</a> of 
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/CollisionSystem.java.html">CollisionSystem</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class CollisionSystem : DrawingWindow
  {
    const int CanvasWidth = 500;
    const int CanvasHeight = 500;

    private MinPQ<Event> pq;        // the priority queue
    private double t = 0.0;        // simulation clock time
    private double hz = 0.5;        // number of redraw events per clock tick
    private Particle[] particles;   // the array of particles
    private double limit;

    /// <summary>
    /// Initializes a system with the specified collection of particles.
    /// The individual particles will be mutated during the simulation.</summary>
    /// <param name="limit">The simulation time in milliseconds</param>
    /// <param name="N">The number of particle</param>
    /// 
    public CollisionSystem(int N, double limit)
    {
      // Set up the drawing surface (canvas) and use the unit scale
      SetCanvasSize(CanvasWidth, CanvasHeight);
      SetPercentScale(true);

      // Initialize the domain objects with the unit coordinates
      particles = new Particle[N];
      for (int i = 0; i < N; i++)
        particles[i] = new Particle(this);

      // initialize PQ with collision events and redraw event
      pq = new MinPQ<Event>();
      for (int i = 0; i < particles.Length; i++)
      {
        predict(particles[i], limit);
      }
      pq.Insert(new Event(0, null, null));        // redraw event

      this.limit = limit;

      FrameUpdateHandler = Update; // atache a frame update handler
    }

    /// <summary>
    /// The event handler to support frame-based animation
    /// </summary>
    /// <param name="sender">the window host</param>
    /// <param name="ev">event argument, usually ignored</param>
    public void Update(object sender, EventArgs ev)
    {
      // the main event-driven simulation loop
      if (!pq.IsEmpty)
      {

        // get impending event, discard if invalidated
        Event e = pq.DelMin();
        if (!e.IsValid()) return;
        Particle a = e.A;
        Particle b = e.B;

        // physical collision, so update positions, and then simulation clock
        for (int i = 0; i < particles.Length; i++)
          particles[i].Move(e.Time - t);
        t = e.Time;

        // process event
        if (a != null && b != null) a.BounceOff(b);              // particle-particle collision
        else if (a != null && b == null) a.BounceOffVerticalWall();   // particle-wall collision
        else if (a == null && b != null) b.BounceOffHorizontalWall(); // particle-wall collision
        else if (a == null && b == null) redraw(limit);               // redraw event

        // update the priority queue with new collisions involving a or b
        predict(a, limit);
        predict(b, limit);
      }
    }
  
    // updates priority queue with all new events for particle a
    private void predict(Particle a, double limit)
    {
      if (a == null) return;

      // particle-particle collisions
      for (int i = 0; i < particles.Length; i++)
      {
        double dt = a.TimeToHit(particles[i]);
        if (t + dt <= limit)
          pq.Insert(new Event(t + dt, a, particles[i]));
      }

      // particle-wall collisions
      double dtX = a.TimeToHitVerticalWall();
      double dtY = a.TimeToHitHorizontalWall();
      if (t + dtX <= limit) pq.Insert(new Event(t + dtX, a, null));
      if (t + dtY <= limit) pq.Insert(new Event(t + dtY, null, a));
    }

    // redraw all particles
    private void redraw(double limit)
    {
      //StdDraw.clear();
      for (int i = 0; i < particles.Length; i++)
      {
        particles[i].Draw();
      }
      ShowFrame(20);
      //StdDraw.show(20);
      if (t < limit)
      {
        pq.Insert(new Event(t + 1.0 / hz, null, null));
      }
    }


    /***************************************************************************
     *  An event during a particle collision simulation. Each event contains
     *  the time at which it will occur (assuming no supervening actions)
     *  and the particles a and b involved.
     *
     *    -  a and b both null:      redraw event
     *    -  a null, b not null:     collision with vertical wall
     *    -  a not null, b null:     collision with horizontal wall
     *    -  a and b both not null:  binary collision between a and b
     *
     ***************************************************************************/
    private class Event : IComparable<Event>
    {
      private readonly double time;         // time that event is scheduled to occur
      private readonly Particle a, b;       // particles involved in event, possibly null
      private readonly int countA, countB;  // collision counts at event creation

      public Particle A
      {
        get { return a; }
      }

      public Particle B
      {
        get { return b; }
      }

      public double Time
      {
        get { return time; }
      }

      // create a new event to occur at time t involving a and b
      public Event(double t, Particle a, Particle b)
      {
        this.time = t;
        this.a = a;
        this.b = b;
        if (a != null) countA = a.CollisionCount;
        else countA = -1;
        if (b != null) countB = b.CollisionCount;
        else countB = -1;
      }

      // compare times when two events will occur
      public int CompareTo(Event that)
      {
        if (this.time < that.time) return -1;
        else if (this.time > that.time) return +1;
        else return 0;
      }

      // has any collision occurred between when event was created and now?
      public bool IsValid()
      {
        if (a != null && a.CollisionCount != countA) return false;
        if (b != null && b.CollisionCount != countB) return false;
        return true;
      }

      public override string ToString()
      {
        return string.Format("{0} {1} t={2:F5}", A, B, Time);
      }
    }

    /// <summary>
    /// Demo test the <c>CollisionSystem</c> data type.
    /// Reads in the particle collision system from a standard input
    /// (or generates <c>N</c> random particles if a command-line integer
    /// is specified); simulates the system.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd CollisionSystem N", "N-number of partices")]
    public static void MainTest(string[] args)
    {
      int N = 50;
      // create N random particles
      if (args.Length == 1)
      {
        N = int.Parse(args[0]);
      }
      CollisionSystem simulator = new CollisionSystem(N, 2000);
      Application app = new Application();
      app.Run(simulator);

      // TODO: Support simulation data from a file
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
