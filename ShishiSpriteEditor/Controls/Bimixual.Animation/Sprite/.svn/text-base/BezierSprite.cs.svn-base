//Copyright 2009 Derek Duban
//This file is part of the Bimixual Animation Library.
//
//Bimixual Animation is free software: you can redistribute it and/or modify
//it under the terms of the GNU Lesser General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//Bimixual Animation is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public License
//along with Bimixual Animation Library.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Drawing;
using System.Collections.Generic;

namespace Bimixual.Animation
{
    /// <summary>
    /// Creates a path sprite made of Bezier curves.
    /// Path format is "guidepoint,guidpoint,endpoint". Therefore you must give a start point to the
    /// constructor. If you set the 'p_closed' parameter to true, then automatic guide points will
    /// be setup for joining the last point to the first point. Those guidepoints will be symmetrical
    /// to the first and last guidepoints of the start poin and end point respectively.
    /// 
    /// How to use:
    /// ctor(starting point, path)
    /// time to traverse
    /// set speed
    /// 
    /// </summary>
	public class BezierSprite : Sprite, GeometricsInterface
	{
		private const double defaultRatio = 0.01d;
		private struct LengthRatio 
		{ 
			public int length; 
			public double ratio; 
			public double startTangent;
			public double endTangent; 
		};
		
		List<LengthRatio> lengthRatios; // length and ratio of each curve in the path
		
        Point startPoint;	// the first point from where to start moving
		PointF currentPoint;	// the result of the most recent GetNewPosition - it is a float so we can use it in normal and tangent calcs
		List<Point> points;	// list of points define the entire path
        double ratio;	// the ratio to move per step
        double ratioStep; // the amount of ratio to move at this step (this is cummulative)
        int segmentIndex; // more than one curve can be in the path. This tracks which curve we are drawing.
                          // The number is the index of the point in List<>points where the segment starts
		int segmentCount;	// How many curves make up the path
		double tangentAngle; // the angle of the tangent at the currentPoint
	
        int remainingLoops;	// is the total curve closed and looping? -1 or 0 means loop forever
		int pathLength; // total length of all curves in the whole thing
		int loopCount; // running count of how many loops have been performed
		double traversalTime; // how long it takes to do one loop
		int speed; // pixels per second

        /// <summary>
        /// Create a sprite that follows a Bezier path.
        /// 
        /// Path format is (similar to svg) (g=guidepoint):
        /// g1p1x,g1p1y[space]g2p1x,g2p1y[space]p1x,p1y
        /// g1p2x,g1p2y[space]g2p2x,g2p2y[space]p2x,p2y
        /// etc...
        /// So it's 2 guidepoints followed by end point.
        /// This will follow simple Bezier paths created using GIMP and exported as SVG.
        /// </summary>
        /// <param name="p_startPoint">Point where sprite begins</param>
        /// <param name="p_path">String of points sprite should follow</param>
        /// <param name="p_ratio">double value indicating stepping size as a fraction of distance along path to take with each step</param>
        /// <param name="p_closed">bool that is true if sprite should loop and automatically re-connects curve to start point</param>
		public BezierSprite(Point p_startPoint, string p_path, double p_ratio, int p_remainingLoops)
			: this(p_startPoint, p_path, p_ratio)
		{
			remainingLoops = p_remainingLoops;
		}

        /// <summary>
        /// Create a sprite that follows a Bezier path.
        /// 
        /// Path format is (similar to svg) (g=guidepoint):
        /// g1p1x,g1p1y[space]g2p1x,g2p1y[space]p1x,p1y
        /// g1p2x,g1p2y[space]g2p2x,g2p2y[space]p2x,p2y
        /// etc...
        /// So it's 2 guidepoints followed by end point.
        /// This will follow simple Bezier paths created using GIMP and exported as SVG.
        /// </summary>
        /// <param name="p_startPoint">Point where sprite begins</param>
        /// <param name="p_path">String of points sprite should follow</param>
        /// <param name="p_ratio">double value indicating stepping size as a fraction of distance along path to take with each step</param>
        public BezierSprite(Point p_startPoint, string p_path, double p_ratio)
		{
			startPoint = p_startPoint;
			
			points = new List<Point>();
            points.Add(p_startPoint);
            points.AddRange(BezierSprite.PointsStringToList(p_path));
			
			if ((points.Count - 1) % 3 != 0) throw new ArgumentException("The number of points in the path must be divisible by 3");

			segmentIndex = 0;
			segmentCount = (points.Count - 1) / 3;

			lengthRatios = new List<LengthRatio>(segmentCount);
			for (int i = 0; i < segmentCount; i++) lengthRatios.Add(new LengthRatio());
			FillInSegmentLengths();
			FillInEndTangents();
			loopCount = 0;
			remainingLoops = 1;
			traversalTime = 0.0d;
			speed = 0;
			completed = false;
			ratioStep = lengthRatios[0].ratio;
		}

		/// <summary>
		/// Use this constructor if you will explicitly set the speed of the sprite using
		/// one of the speed methods, SetSpeed() or SetTraversalTime()
		/// </summary>
		/// <param name="p_startPoint">
		/// A <see cref="Point"/> where the sprite begins
		/// </param>
		/// <param name="p_path">
		/// A <see cref="System.String"/> of points along a bezier curve to follow
		/// </param>
		public BezierSprite(Point p_startPoint, string p_path) : this(p_startPoint, p_path, 0.0d)
		{
		}
		
		/// <summary>
		/// If the ratio is 0.0d we calculate tangents based on the end points to their
		/// nearest guide point. This does that.
		/// </summary>
		private void FillInEndTangents()
		{
			if (lengthRatios == null) throw new Exception("lengthRatios must be defined assigned before calling FillInEndTangents");
			
			for (int i = 0; i < segmentCount; i++)
			{
				LengthRatio lr = lengthRatios[i];
				
				Point pt;

				// we can't determine angles if guide points are exactly the same as node points.
				// Therefore, if that is the case, find another guide point to use.
				// if first guide point is not same as first node
				if (points[i * 3] != points[i * 3 + 1]) pt = points[i * 3 + 1];
				else // else if 2nd guide point is not same as first node
				if (points[i * 3] != points[i * 3 + 2]) pt = points[i * 3 + 2];
				else // else if end point is not same as first node
				if (points[i * 3] != points[i * 3 + 3]) pt = points[i * 3 + 3];
				else // else all points in curve are the same - just accept it and move on
					pt = points[i * 3];
				
				if (pt == points[i * 3]) // all points in curve are same, set tangent to 0 degrees
				{
					lr.startTangent = 0.0d;
					lr.endTangent = 0.0d;
					continue;
				}
				else
				{
					lr.startTangent = CalculateAngleBetweenPoints(points[i * 3], pt);
				}
				// if 2nd guide point is not same as 2nd node
				if (points[i * 3 + 3] != points[i * 3 + 2]) pt = points[i * 3 + 2];
				else // else if 1st guide point is not same as 2nd node
				if (points[i * 3 + 3] != points[i * 3 + 1]) pt = points[i * 3 + 1];
				else // else if 1st node is not same as 2nd node
				if (points[i * 3 + 3] != points[i * 3 + 0]) pt = points[i * 3 + 0];
				
				lr.endTangent = CalculateAngleBetweenPoints(pt, points[i * 3 + 3]);
				
				lengthRatios[i] = lr;
			}
		}

		/// <summary>
		/// Determine and store the length of each curve segment along the whole path
		/// 
		/// Assumption:
		/// 	points has 3*n+1 points in it.
		/// 	
		/// </summary>
		private void FillInSegmentLengths()
		{
			if (lengthRatios == null) throw new Exception("lengthRatios must be defined assigned before calling FillInEndTangents");

			pathLength = 0;
			
			for (int i = 0; i < segmentCount; i++)
			{
				LengthRatio lr = new LengthRatio();
				
				lr.length = 
					BezierSprite.CalculateCurveLength(
						points.GetRange(i * 3, 4).ToArray() // there are 4 points to each segment. * 3 because we start each next segment with the end point of the previous segment
					);
				
				lr.ratio = defaultRatio; // default value since we haven't calc'd ratios yet
				lengthRatios[i] = lr;
				
				pathLength += lr.length; // calc length of whole path
			}
		}

		/// <summary>
		/// Setup the ratios for each curve of the path. Ratios are based on
		/// the speed that that portion of the path must follow. It is complicated
		/// by the fact that we must work on the curve via ratios rather than discreet
		/// points along it.
		/// </summary>
		private void InitRatios()
		{
			int nFrames;

			// for each curve along the path
			for (int i = 0; i < lengthRatios.Count; i++)
			{
				try
				{
					// this curve amounts to what % of the total path length
					double lengthPortion = (double)(lengthRatios[i].length / (double)pathLength);
					
					// based on total path time, how much time should be spent traversing this curve
					double timePortion = (lengthPortion * traversalTime);
					
					// how fast must sprite travel on this curve based on time spent on it.
					// Remember, this gets re-considered as a ratio later so the speed really
					// won't be different for this curve than others.
					double thisSpeed = lengthRatios[i].length/timePortion;
					
					// determine how far to travel per frame
					double frameSpeed = thisSpeed / SpriteManager.FPS;
				
					// determine how many frames will show while travelling this curve
					nFrames = (int)Math.Round(lengthRatios[i].length/ frameSpeed);
					
					// at least show one frame in this curve, no matter what
					nFrames = nFrames < 1? 1: nFrames;
					
					// determine the ratio for the curve based on the number of frames to show
					double ratio = (double)(1.0d / (double)nFrames);
					
					// store what we calculated
					LengthRatio lrRef = lengthRatios[i];
					lrRef.length = lengthRatios[i].length;
					lrRef.ratio = ratio;
					lengthRatios[i] = lrRef;
				}
				catch(Exception e)
				{
					throw e;
				}
			}
			ratioStep = lengthRatios[0].ratio;
		}
		
		/// <summary>
		/// Set how long it takes to travel length of whole path
		/// </summary>
		/// <param name="p_time">
		/// A <see cref="System.Double"/> indicating how long it takes to travel whole path
		/// </param>
		public void SetTraversalTime(double p_time)
		{
			// Complete path in p_time seconds.
			
			traversalTime = p_time;
			speed = (int)Math.Round(pathLength / traversalTime);
			InitRatios();
		}
		
		/// <summary>
		/// Set, roughtly, how fast sprite should travel along path
		/// </summary>
		/// <param name="p_time">
		/// A <see cref="System.Int32"/> speed in pixels per second
		/// </param>
		public void SetSpeed(int p_pixelsPerSecond)
		{
			speed = p_pixelsPerSecond;
			traversalTime = (double)Math.Round((double)(pathLength / (double)p_pixelsPerSecond));
			InitRatios();
		}
		
		/// <summary>
		/// Trace the path quickly through to determine its length
		/// </summary>
		/// <param name="p_points">
		/// A <see cref="Point"/> array of 4 points specifying Bezier curve
		/// </param>
		/// <returns>
		/// A <see cref="System.Int32"/> as the length of the given curve
		/// </returns>
		static public int CalculateCurveLength(Point [] p_points)
		{
			if (p_points.Length != 4) throw new Exception("p_points array passed to CalculateCurveLength must have exactly 4 items, this one has " + p_points.Length);

			PointF pA = p_points[0];
			double rtn = 0.0d;
			
			// determine what ratio gives up about 1 pixel of distance
			double accuracyRatio = BezierSprite.CalculateRatioSizeToPixelAccuracy(p_points, 1);
			
			// set an incrementer
			double currentRatio = accuracyRatio;
			
			// set a limit for when we reach the end of the path - allow some overshoot
			double topLimit = 1.0 + accuracyRatio;
			
			// traverse path one fraction bit at a time calculating distances between points
			while(currentRatio < topLimit)
			{
				PointF pB = CalculatePoint(p_points, currentRatio);
				rtn += Math.Sqrt(Math.Pow(pA.X - pB.X, 2) + Math.Pow(pA.Y - pB.Y, 2));
				pA = pB;
				currentRatio += accuracyRatio;
			}
			
			return (int)Math.Round(rtn);
		}
		
		/// <summary>
		/// Determine what ratio amounts to a given pixel distance
		/// </summary>
		/// <param name="p_points">
		/// A <see cref="Point"/> array of 4 points specifying the Bezier curve to analyze
		/// </param>
		/// <param name="p_pixels">
		/// A <see cref="System.Int32"/> indicating how many pixels the returned ratio should indicate
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/> representing the ratio on the curve that amounts to p_pixels distance
		/// </returns>
		static public double CalculateRatioSizeToPixelAccuracy(Point [] p_points, int p_pixels)
		{
			if (p_points.Length != 4) throw new Exception("p_points array passed to GetRatioSizeToPixelAccuracy must have exactly 4 items, this one has " + p_points.Length);
			
			double testRatio = 1.0d;
			PointF prevPoint = BezierSprite.CalculatePoint(p_points, 0.0d);

			// first cut test ratio in half repeatedly until we fall below the requested
			// pixel threshold, then increment the ratio back up by .01 until we hit the
			// threshold again.
			while(true)
			{
				testRatio /= 2.0d;
				PointF nextPoint = BezierSprite.CalculatePoint(p_points, testRatio);

				if (BezierSprite.Hypot(prevPoint, nextPoint) < p_pixels)
				{
					while(true)
					{
						testRatio += 0.01d;
						PointF nextPoint2 = BezierSprite.CalculatePoint(p_points, testRatio);
						if (BezierSprite.Hypot(prevPoint, nextPoint2) > p_pixels)
						{
							break;
						}
					}
					break;
				}
			}

			return testRatio;
		}
		
		/// <summary>
		/// Get the hypotenuse (distance) of the triangle formed by the two points
		/// </summary>
		/// <param name="p_1">
		/// A <see cref="Point"/> 
		/// </param>
		/// <param name="p_2">
		/// A <see cref="Point"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/> representing the distance between the two given points
		/// </returns>
		static public double Hypot(PointF p_1, PointF p_2)
		{
			return Math.Sqrt(Math.Pow(p_1.X - p_2.X, 2) + Math.Pow(p_1.Y - p_2.Y, 2));
		}
		
		/// <summary>
		/// Determines of a given number is between 2 others
		/// </summary>
		/// <param name="p_">
		/// A <see cref="System.Double"/> to be tested
		/// </param>
		/// <param name="p_lower">
		/// A <see cref="System.Double"/> that is the non-inclusive lower bound of the test
		/// </param>
		/// <param name="p_upper">
		/// A <see cref="System.Double"/> that is the non-inclusive upper bound of the test
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> true if the first parameter is between the 2 given bounds
		/// </returns>
		static public bool IsBetween(double p_, double p_lower, double p_upper)
		{
			return (p_ > p_lower && p_ < p_upper);
		}
		
		/// <summary>
		/// Calculate the angle between 2 points by first shifting the Y axis
		/// to Cartesian cooors
		/// </summary>
		/// <param name="p_1">
		/// A <see cref="PointF"/>
		/// </param>
		/// <param name="p_2">
		/// A <see cref="PointF"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/>
		/// </returns>
		static public double CalculateAngleBetweenPoints(PointF p_1, PointF p_2)
		{
			double rtn = 0.0d;
			
			double dy = (p_2.Y - p_1.Y);
			double dx = (p_2.X - p_1.X);
			
			rtn = (float)Math.Atan2(dy, dx) * (float)(180 / Math.PI);
			
			if (dx < 0 && dy < 0) rtn *= -1;
			if (dx > 0 && dy < 0) rtn *= -1;
			if (dx < 0 && dy > 0) rtn = 360.0d - rtn;
			if (dx > 0 && dy > 0) rtn = 360.0d - rtn;			

//Console.WriteLine("dy:{0}, dx:{1}, rtn:{2}",dy, dx, rtn);
			return rtn;
		}
		
        /// <summary>
        /// Returns the sprites next position on the path using the following formula:
        /// 
        /// Bx(t) = (1-t)^3  x P1x + 3 x (1-t)^2 x t x P2x + 3 x (1-t) x t^2 x P3x + t^3 x P4x
        /// By(t) = (1-t)^3  x P1y + 3 x (1-t)^2 x t x P2y + 3 x (1-t) x t^2 x P3y + t^3 x P4y
		///
		/// http://upload.wikimedia.org/math/2/d/5/2d5e5d58562d8ec2c35f16df98d2b974.png
		/// B(t)=(1-t)^2)*p0 + 2(1-t)*t*p1 + t^2(p2)
		/// quadratic: http://upload.wikimedia.org/wikipedia/commons/b/bf/Bezier_2_big.png
		/// cubic: http://upload.wikimedia.org/wikipedia/commons/c/c1/Bezier_3_big.png
        /// </summary>
        /// <returns>Point indicating next position of sprite</returns>
		protected override Point GetNewPosition()
        {
			currentPoint = 
				BezierSprite.CalculatePoint(
					points.GetRange(segmentIndex * 3, 4).ToArray(), 
				    ratio
				);
			
			// if approaching end of current curve, then get pre-calculated angle
			if ((1.0d - ratio) < ratioStep)
			{
				tangentAngle = lengthRatios[segmentIndex].endTangent;
				
			}
			else
			if (ratio < ratioStep) // if just past the start of next curve, then use pre-calculated angle
			{
				tangentAngle = lengthRatios[segmentIndex].startTangent;
			}
			else // else get angle along the curve based on ratio
			{
				tangentAngle = CalculateTangentAngle(points.GetRange(segmentIndex * 3, 4).ToArray(), ratio, currentPoint);
			}
//Console.WriteLine("tangentAngle={0}, seg={1}, ratio={2}", tangentAngle, segmentIndex, ratio);
			ratio = (double)(ratio + ratioStep);

			if (ratio > 1.0d)
            {
				ratio -= 1.0d;
                segmentIndex++;
                if (segmentIndex >= segmentCount)
				{
					if (remainingLoops != -1)
					{
						if (--remainingLoops > 0)
							segmentIndex = 0;
						else
	                    	completed = true;
					}
					else
						segmentIndex = 0;
					
					loopCount++;
				}
				else
				{
		            ratioStep = lengthRatios[segmentIndex].ratio;
				}
            }

            return new Point((int)Math.Round(currentPoint.X), (int)Math.Round(currentPoint.Y));

        }

		/// <summary>
        /// Determines the angle of the tangent along the curve given by the points and ratio
        /// </summary>
        /// <param name="p_points">array of the first 4 points of a cubic bezier curve</param>
        /// <param name="p_ratio">double indicating what position along the curve we are. ex: .5 is half way along curve</param>
        /// <param name="quadPoint">the point on a quadratic bezier curve with which determine the tangent</param>
        /// <returns>double value of angle of tangent at given point on curve</returns>
		static public double CalculateTangentAngle(Point [] p_points, double p_ratio, PointF quadPoint)
		{
			if (p_points.Length != 4) throw new Exception("CalculatePoint() must be passed array of exactly 4 Point objects");
			
            float cx = (float)
	            (
	             (1-p_ratio)*(1-p_ratio)*p_points[0].X +
	             2*(1-p_ratio)*p_ratio*p_points[1].X +
	             p_ratio*p_ratio*p_points[2].X
	            );

			float cy = (float)
		        (
		         (1-p_ratio)*(1-p_ratio)*p_points[0].Y +
		         2*(1-p_ratio)*p_ratio*p_points[1].Y +
		         p_ratio*p_ratio*p_points[2].Y
		        );

			return CalculateAngleBetweenPoints(new PointF(cx, cy), quadPoint);
	
		}
		/// <summary>
        /// Determines the position along the curve given by the segment and ratio
        /// </summary>
        /// <param name="p_whiSegment">int indicating which segment of the curve we are along</param>
        /// <param name="p_ratio">double indicating what position along the curve we are. ex: .5 is half way along curve</param>
        /// <returns>PointF value of point of curve at given ratio</returns>
		static public PointF CalculatePoint(Point [] p_points, double p_ratio)
		{
			if (p_points.Length != 4) throw new Exception("CalculatePoint() must be passed array of exactly 4 Point objects");
			
			PointF rtn = new Point();

			try
			{
            rtn.X = (float)(
                Math.Pow(1 - p_ratio, 3) * p_points[0].X +
                    3 * Math.Pow(1 - p_ratio, 2) * p_ratio * p_points[1].X +
                    3 * Math.Pow(1 - p_ratio, 1) * Math.Pow(p_ratio, 2) * p_points[2].X +
                    Math.Pow(p_ratio, 3) * p_points[3].X);

            rtn.Y = (float)(
                Math.Pow(1 - p_ratio, 3) * p_points[0].Y +
                    3 * Math.Pow(1 - p_ratio, 2) * p_ratio * p_points[1].Y +
                    3 * Math.Pow(1 - p_ratio, 1) * Math.Pow(p_ratio, 2) * p_points[2].Y +
                    Math.Pow(p_ratio, 3) * p_points[3].Y);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
			
			return rtn;
		}

		/// <summary>
		/// [deprecated]
        /// Determines the position along the curve given by the segment and ratio
        /// </summary>
        /// <param name="p_whiSegment">int indicating which segment of the curve we are along</param>
        /// <param name="p_ratio">double indicating what position along the curve we are. ex: .5 is half way along curve</param>
        /// <returns></returns>
		private Point CalculatePoint(int p_whiSegment, double p_ratio)
		{
			Point rtn = new Point();

			try
			{
            rtn.X = (int)Math.Round(
                Math.Pow(1 - p_ratio, 3) * points[p_whiSegment].X +
                    3 * Math.Pow(1 - p_ratio, 2) * p_ratio * points[p_whiSegment + 1].X +
                    3 * Math.Pow(1 - p_ratio, 1) * Math.Pow(p_ratio, 2) * points[p_whiSegment + 2].X +
                    Math.Pow(p_ratio, 3) * points[p_whiSegment + 3].X);
			

            rtn.Y = (int)Math.Round(
                Math.Pow(1 - p_ratio, 3) * points[p_whiSegment].Y +
                    3 * Math.Pow(1 - p_ratio, 2) * p_ratio * points[p_whiSegment + 1].Y +
                    3 * Math.Pow(1 - p_ratio, 1) * Math.Pow(p_ratio, 2) * points[p_whiSegment + 2].Y +
                    Math.Pow(p_ratio, 3) * points[p_whiSegment + 3].Y);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message + "\n\nwhiSeg="+p_whiSegment+",point count="+points.Count);
			}
			
			return rtn;
		}

		/// <summary>
		/// Converts a string list of points to a List<Point> of points
		/// </summary>
		/// <param name="p_path">
		/// A <see cref="System.String"/> of points separated by commas & spaces
		/// ex: "10,10 20,20"  defines 2 points.
		/// </param>
		/// <returns>
		/// A <see cref="List"/> of points.
		/// </returns>
		static public List<Point> PointsStringToList(string p_path)
		{
			List<Point> rtn = new List<Point>();

			try
			{
				foreach (string point in p_path.Split(new char [] { ' ' }))
				{
					string [] parts = point.Split(new char [] { ',' });
					rtn.Add(new Point(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1])));
					
				}
			}
			catch
			{
				rtn = null;
			}
			
			return rtn;
		}

        /// <summary>
        /// Reflects the give point over the given vertex
        /// </summary>
        /// <param name="p_point">Point to be refected</param>
        /// <param name="p_vertex">Point around which to reflect</param>
        /// <returns>Point mirrored around given vertex</returns>
		static public Point ReflectPoint(Point p_point, Point p_vertex)
		{
			Point rtn = new Point();

			rtn.X = p_point.X + 2 * (p_vertex.X - p_point.X);
			rtn.Y = p_point.Y + 2 * (p_vertex.Y - p_point.Y);

			return rtn;
		}

		//// <value>
		/// The tangle angle at the current point along curve
		/// </value>
		public double TangentAngle
		{
			get { return tangentAngle; }
		}

		//// <value>
		/// The normal of the tangent at current point along curve
		/// </value>
		public double NormalAngle
		{
			get
			{
				double normal = tangentAngle + 90.0d;
				
				if (normal > 360)
					normal -= 360.0d;
				
				return normal;
			}
		}

	}

}
