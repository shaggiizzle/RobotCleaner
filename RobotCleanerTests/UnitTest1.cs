using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobotCleaner;

namespace RobotCleanerTests
{
  [TestClass]
  public class ParallelOverlap
  {
    [TestMethod]
    public void TestHorizontalOverlapRight()
    {
      Line lineA = new Line(5, 10, 10, 10);
      Line lineB = new Line(7, 10, 17, 10);

      Assert.AreEqual(3, Program.ParallelOverlap(lineA, lineB));
    }

    [TestMethod]
    public void TestHorizontalOverlapLeft()
    {
      Line lineA = new Line(5, 10, 10, 10);
      Line lineB = new Line(2, 10, 6, 10);

      Assert.AreEqual(1, Program.ParallelOverlap(lineA, lineB));
    }

    [TestMethod]
    public void TestHorizontalOverlapComplete()
    {
      Line lineA = new Line(5, 10, 10, 10);
      Line lineB = new Line(2, 10, 26, 10);

      Assert.AreEqual(5, Program.ParallelOverlap(lineA, lineB));
    }

    [TestMethod]
    public void TestHorizontalNoOverlap()
    {
      Line lineA = new Line(5, 10, 10, 10);
      Line lineB = new Line(12, 10, 26, 10);

      Assert.AreEqual(0, Program.ParallelOverlap(lineA, lineB));
    }

    [TestMethod]
    public void TestVerticalOverlapBottom()
    {
      Line lineA = new Line(10, 5, 10, 10);
      Line lineB = new Line(10, 7, 10, 17);

      Assert.AreEqual(3, Program.ParallelOverlap(lineA, lineB));
    }

    [TestMethod]
    public void TestVerticalOverlapTop()
    {
      Line lineA = new Line(10, 5, 10, 10);
      Line lineB = new Line(10, 2, 10, 6);

      Assert.AreEqual(1, Program.ParallelOverlap(lineA, lineB));
    }

    [TestMethod]
    public void TestVerticalOverlapComplete()
    {
      Line lineA = new Line(10, 5, 10, 10);
      Line lineB = new Line(10, 2, 10, 26);

      Assert.AreEqual(5, Program.ParallelOverlap(lineA, lineB));
    }

    [TestMethod]
    public void TestVerticalNoOverlap()
    {
      Line lineA = new Line(10, 5, 10, 10);
      Line lineB = new Line(10, 12, 10, 26);

      Assert.AreEqual(0, Program.ParallelOverlap(lineA, lineB));
    }
  }

  [TestClass]
  public class InterSectionOverlap
  {
    [TestMethod]
    public void TestMiddleIntersection()
    {
      Line lineA = new Line(0, 0, 0, 10);
      Line lineB = new Line(-5, 5, 5, 5);

      Assert.AreEqual(1, Program.IntersectionOverlap(lineA, lineB));
    }

    [TestMethod]
    public void TestMiddleIntersectionInverted()
    {
      Line lineA = new Line(0, 0, 0, 10);
      Line lineB = new Line(-5, 5, 5, 5);

      Assert.AreEqual(1, Program.IntersectionOverlap(lineB, lineA));
    }

    [TestMethod]
    public void IgnoreJustTheTip()
    {
      Line lineA = new Line(0, 0, 0, 10);
      Line lineB = new Line(0, 10, 15, 10);

      Assert.AreEqual(0, Program.IntersectionOverlap(lineB, lineA));
    }

    [TestMethod]
    public void IgnoreJustTheTipInverted()
    {
      Line lineA = new Line(0, 0, 0, 10);
      Line lineB = new Line(0, 10, 15, 10);

      Assert.AreEqual(0, Program.IntersectionOverlap(lineA, lineB));
    }

    [TestMethod]
    public void NoIntersection()
    {
      Line lineA = new Line(0, 0, 0, 5);
      Line lineB = new Line(5, 10, 15, 10);

      Assert.AreEqual(0, Program.IntersectionOverlap(lineA, lineB));
    }

  }

  [TestClass]
  public class EvaluateParallelAndSamePlane
  {
    [TestMethod]
    public void SameHorizontalPlane()
    {
      Line lineA = new Line(0, 0, 10, 0);
      Line lineB = new Line(-15, 0, -5, 0);

      Assert.IsTrue(Program.EvaluateParallelAndSamePlane(lineA, lineB));
    }

    [TestMethod]
    public void SameVerticalPlane()
    {
      Line lineA = new Line(0, 0, 0, 10);
      Line lineB = new Line(0, -15, 0, -5);

      Assert.IsTrue(Program.EvaluateParallelAndSamePlane(lineA, lineB));
    }

    [TestMethod]
    public void DifferentVerticalPlane()
    {
      Line lineA = new Line(5, 0, 5, 10);
      Line lineB = new Line(0, -15, 0, -5);

      Assert.IsFalse(Program.EvaluateParallelAndSamePlane(lineA, lineB));
    }

    [TestMethod]
    public void DifferentOrientations()
    {
      Line lineA = new Line(-10, 0, 5, 0);
      Line lineB = new Line(0, -15, 0, -5);

      Assert.IsFalse(Program.EvaluateParallelAndSamePlane(lineA, lineB));
    }
  }

  [TestClass]
  public class CheckForOverlap
  {
    [TestMethod]
    public void TestSimpleNoOverlappingLines()
    {
      Line[] readLines = new Line[1];
      readLines[0] = new Line(10, 22, 12, 22);

      Assert.AreEqual(0, Program.CheckForOverlap(new Line(12, 22, 12, 23), readLines, readLines.Length));
    }

    [TestMethod]
    public void TestSimpleOverlappingLines()
    {
      Line[] readLines = new Line[1];
      readLines[0] = new Line(0, 0, 12, 0);

      Assert.AreEqual(8, Program.CheckForOverlap(new Line(2, 0, 10, 0), readLines, readLines.Length));
    }

    [TestMethod]
    public void TestSimpleNoOverlappingDifferentPlanes()
    {
      Line[] readLines = new Line[1];
      readLines[0] = new Line(0, 0, 12, 0);

      Assert.AreEqual(0, Program.CheckForOverlap(new Line(5, 5, 12, 5), readLines, readLines.Length));
    }
  }

  [TestClass]
  public  class ParseNewLine
  {
    [TestMethod]
    public void ParseLineE()
    {
      int endLineX = 0, endLineY = 0, currentSteps = 0;
      Line parsedLine = Program.ParseNewLine("E 5", 0, 0, ref endLineX, ref endLineY, ref currentSteps);

      Assert.AreEqual(5, currentSteps);
      Assert.AreEqual(0, endLineY);
      Assert.AreEqual(5, endLineX);
    }

    [TestMethod]
    public void ParseLineW()
    {
      int endLineX = 0, endLineY = 0, currentSteps = 0;
      Line parsedLine = Program.ParseNewLine("W 5", 0, 0, ref endLineX, ref endLineY, ref currentSteps);

      Assert.AreEqual(5, currentSteps);
      Assert.AreEqual(0, endLineY);
      Assert.AreEqual(-5, endLineX);
    }

    [TestMethod]
    public void ParseLineN()
    {
      int endLineX = 0, endLineY = 0, currentSteps = 0;
      Line parsedLine = Program.ParseNewLine("N 5", 0, 0, ref endLineX, ref endLineY, ref currentSteps);

      Assert.AreEqual(5, currentSteps);
      Assert.AreEqual(5, endLineY);
      Assert.AreEqual(0, endLineX);
    }

    [TestMethod]
    public void ParseLineS()
    {
      int endLineX = 0, endLineY = 0, currentSteps = 0;
      Line parsedLine = Program.ParseNewLine("S 5", 0, 0, ref endLineX, ref endLineY, ref currentSteps);

      Assert.AreEqual(5, currentSteps);
      Assert.AreEqual(-5, endLineY);
      Assert.AreEqual(0, endLineX);
    }

    [TestMethod]
    public void ParseLineZ()
    {
      int endLineX = 0, endLineY = 0, currentSteps = 0;
      Line parsedLine = Program.ParseNewLine("Z 5", 0, 0, ref endLineX, ref endLineY, ref currentSteps);

      Assert.AreEqual(5, currentSteps);
      Assert.AreEqual(0, endLineY);
      Assert.AreEqual(0, endLineX);
    }
  }
}
