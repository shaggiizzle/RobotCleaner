using System;

namespace RobotCleaner
{
  public enum Orientation
  { 
    Horizontal,
    Vertical
  }

  public struct Line
  {
    public int startingX;
    public int startingY;
    public int endingX;
    public int endingY;
    public Orientation orientation;

    public Line(int startingX, int startingY, int endingX, int endingY)
    {
      this.startingX = Math.Min(startingX, endingX);
      this.startingY = Math.Min(startingY, endingY);
      this.endingX = Math.Max(startingX, endingX);
      this.endingY = Math.Max(startingY, endingY);
      orientation = startingX == endingX ? Orientation.Vertical : Orientation.Horizontal;
    }
  }
  public class Program
  {
   

    static void Main(string[] args)
    {

      int numberOfMoves = int.Parse(Console.ReadLine());
      string[] startingCoordinates = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      Line[] readLines = new Line[numberOfMoves];
      int cleanCount = 1;
      int lineBeginX = int.Parse(startingCoordinates[0]);
      int lineBeginY = int.Parse(startingCoordinates[1]);
      int lineEndX = lineBeginX;
      int lineEndY = lineBeginY;


      for (int i = 0; i < numberOfMoves; i++)
      {
        int currentSteps = 0;
        Line currentLine = ParseNewLine(Console.ReadLine(), lineBeginX, lineBeginY, ref lineEndX, ref lineEndY, ref currentSteps);
        int overlap = CheckForOverlap(currentLine, readLines, i);
        cleanCount += currentSteps - overlap;
        readLines[i] = currentLine;
        lineBeginX = lineEndX;
        lineBeginY = lineEndY;

      }

      Console.WriteLine("=> Cleaned: {0}", cleanCount);
      Console.ReadLine();
    }


    public static Line ParseNewLine(String input, int lineBeginX, int lineBeginY, ref int lineEndX, ref int lineEndY, ref int currentSteps)
     {
      string[] nextMove = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      string currentDirection = nextMove[0];
      currentSteps = int.Parse(nextMove[1]);
      int moveX = 0;
      int moveY = 0;
      switch (currentDirection)
      {
        case "E":
          moveX = currentSteps;
          break;
        case "W":
          moveX = -currentSteps;
          break;
        case "S":
          moveY = -currentSteps;
          break;
        case "N":
          moveY = currentSteps;
          break;
        default:
          break;
      }

      lineEndX = lineBeginX + moveX;
      lineEndY = lineBeginY + moveY;

      Line currentLine = new Line(lineBeginX, lineBeginY, lineEndX, lineEndY);

      return currentLine;
    }

    public static int CheckForOverlap(Line currentLine, Line[] readLines, int readLineCount)
    {
      int overlapCount = 0;
      for (int i = 0; i < readLineCount; i++)
      {
        Line compareLine = readLines[i];
        if (EvaluateParallelAndSamePlane(currentLine,readLines[i]))
        {
          overlapCount += ParallelOverlap(currentLine, readLines[i]);
        }
        else if (compareLine.orientation != currentLine.orientation)
        {
          overlapCount += IntersectionOverlap(currentLine, readLines[i]);
        }
      }

      return overlapCount;
    }

    public static bool EvaluateParallelAndSamePlane(Line lineA, Line lineB)
    {
      if (lineA.orientation == lineB.orientation)
      {
        if(lineA.orientation == Orientation.Horizontal)
        {
          return lineA.startingY == lineB.startingY;
        }
        else
        {
          return lineA.startingX == lineB.startingX;
        }
      }
      return false;
    }

    public static int IntersectionOverlap(Line lineA, Line lineB)
    {
      if (lineA.orientation == Orientation.Horizontal)
      {
        if (lineA.startingY > lineB.startingY && lineA.startingY < lineB.endingY && 
          lineB.startingX >= lineA.startingX && lineB.startingX <= lineA.endingX)
        {
          return 1;
        }
      }
      else
      {
        if (lineA.startingX > lineB.startingX && lineA.startingX < lineB.endingX &&
          lineB.startingY >= lineA.startingY && lineB.startingY <= lineA.endingY)
        {
          return 1;
        }
      }
      return 0;
    }

    public static int ParallelOverlap(Line lineA, Line lineB)
    {
      int startA, endA, startB, endB;
      if (lineA.orientation == Orientation.Horizontal)
      {
        startA = lineA.startingX;
        endA = lineA.endingX;
        startB = lineB.startingX;
        endB = lineB.endingX;
      }
      else
      {
        startA = lineA.startingY;
        endA = lineA.endingY;
        startB = lineB.startingY;
        endB = lineB.endingY;
      }

        int minIntersection = Math.Max(startA, startB);
        int maxIntersection = Math.Min(endA, endB);

        if (minIntersection < maxIntersection)
        {
          return maxIntersection - minIntersection;
        }
      
      return 0;
    }
  }
}
