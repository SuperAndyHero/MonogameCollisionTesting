using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace MonogameCollisionTesting
{
    public static class Helper
    {
        public static Polygon DefaultPolygon => new Polygon(new Vector2[4] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) });

        public static Polygon CreateCircle(int vertCount, float radius = 1f)
        {
            Vector2[] Points = new Vector2[vertCount];
            for (int i = 0; i < vertCount; i++)
            {
                Vector2 pointPosition = new Vector2(0, radius).RotatedBy((MathHelper.TwoPi / vertCount) * i);
                Points[i] = pointPosition;
            }
            return new Polygon(Points);
        }

        public static List<VertexPositionColor> MakeVertexPosList(ref Vector2[] vertices, Color color)
        {
            List<VertexPositionColor> vertexPosList = new List<VertexPositionColor>();
            foreach (Vector2 vec in vertices)
                vertexPosList.Add(new VertexPositionColor(new Vector3(vec, 0), color));
            return vertexPosList;
        }

        public static List<VertexPositionColor> MakeVertexPosListRainbow(ref Vector2[] vertices)
        {
            List<VertexPositionColor> vertexPosList = new List<VertexPositionColor>();
            Random rand = new Random(vertices.Length);
            foreach (Vector2 vec in vertices)
                vertexPosList.Add(new VertexPositionColor(new Vector3(vec, 0), rand.NextColor()));
            return vertexPosList;
        }

        //public static void DrawFilledPolygon(BasicEffect effect, EffectPass pass, GraphicsDevice graphicsDevice, Polygon polygon, Color color, Vector2 position, float Size = 1f)
        //{
        //    graphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };

        //    List<VertexPositionColor> vertices = new List<VertexPositionColor>();
        //    Random rand = new Random(polygon.vertices.Length);
        //    foreach (Vector2 vec in polygon.vertices) 
        //        vertices.Add(new VertexPositionColor(new Vector3(vec, 0), new Color(rand.Next(256), rand.Next(256), rand.Next(256))));

        //    VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.WriteOnly);
        //    vertexBuffer.SetData(vertices.ToArray());

        //    IndexBuffer indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), polygon.indices.Length, BufferUsage.WriteOnly);
        //    indexBuffer.SetData(polygon.indices);


        //    effect.World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(Size) * Matrix.CreateTranslation(Game1.screenPosition.X - position.X, -Game1.screenPosition.Y + position.Y, 0);
        //    //effect.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(Game1.screenWidth / 2, Game1.screenHeight / -2, 0) * Matrix.CreateScale(polygon.size);
        //    effect.View = Matrix.CreateScale(Game1.zoom) * Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateRotationX(MathHelper.Pi);

        //    pass.Apply();
        //    graphicsDevice.SetVertexBuffer(vertexBuffer);
        //    graphicsDevice.Indices = indexBuffer;
        //    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexBuffer.IndexCount - 2);
        //}

        //public static void DrawPolygonCenterToCorners(BasicEffect effect, EffectPass pass, GraphicsDevice graphicsDevice, Polygon polygon, Color color, Vector2 position, float Size = 1f)
        //{
        //    List<VertexPositionColor> vertices = new List<VertexPositionColor>();
        //    foreach (Vector2 vec in polygon.vertices) 
        //        vertices.Add(new VertexPositionColor(new Vector3(vec, 0), color));

        //    vertices.Add(new VertexPositionColor(new Vector3(polygon.center, 0), color));

        //    VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.WriteOnly);
        //    vertexBuffer.SetData(vertices.ToArray());


        //    short[] indices2 = new short[polygon.vertices.Length * 2];
        //    for (int i = 0; i < indices2.Length; i++)
        //    {
        //        if (i % 2 == 0)
        //        {
        //            indices2[i] = (short)(i / 2);
        //        }
        //        else indices2[i] = (short)polygon.vertices.Length;
        //    }

        //    IndexBuffer indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), indices2.Length, BufferUsage.WriteOnly);
        //    indexBuffer.SetData(indices2);


        //    effect.World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(Size) * Matrix.CreateTranslation(Game1.screenPosition.X - position.X, -Game1.screenPosition.Y + position.Y, 0);
        //    effect.View = Matrix.CreateScale(Game1.zoom) * Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateRotationX(MathHelper.Pi);

        //    pass.Apply();
        //    graphicsDevice.SetVertexBuffer(vertexBuffer);
        //    graphicsDevice.Indices = indexBuffer;
        //    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, indexBuffer.IndexCount - 2);
        //}

        //public static void DrawPolygonOutline(BasicEffect effect, EffectPass pass, GraphicsDevice graphicsDevice, Polygon polygon, Color color, Vector2 position, float Size = 1f)
        //{
        //    List<VertexPositionColor> vertices = new List<VertexPositionColor>();
        //    foreach (Vector2 vec in polygon.vertices)
        //        vertices.Add(new VertexPositionColor(new Vector3(vec, 0), color));

        //    vertices.Add(new VertexPositionColor(new Vector3(polygon.vertices[0], 0), color));

        //    VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.WriteOnly);
        //    vertexBuffer.SetData(vertices.ToArray());

        //    effect.World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(Size) * Matrix.CreateTranslation(Game1.screenPosition.X - position.X, -Game1.screenPosition.Y + position.Y, 0);
        //    effect.View = Matrix.CreateScale(Game1.zoom) * Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateRotationX(MathHelper.Pi);

        //    pass.Apply();
        //    graphicsDevice.SetVertexBuffer(vertexBuffer);
        //    graphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, vertices.Count - 1);
        //}

        //public static bool Ccw(Vector2 A, Vector2 B, Vector2 C) => 
        //    (C.Y - A.Y) * (B.X - A.X) > (B.Y - A.Y) * (C.X - A.X);
        //public static bool DoLinesIntersect(Vector2 LineStartA, Vector2 LineEndA, Vector2 LineStartB, Vector2 LineEndB) => 
        //    Ccw(LineStartA, LineStartB, LineEndB) != Ccw(LineEndA, LineStartB, LineEndB) && Ccw(LineStartA, LineEndA, LineStartB) != Ccw(LineStartA, LineEndA, LineEndB);

        public static Color NextColor(this Random rand) => new Color(rand.Next(256), rand.Next(256), rand.Next(256));

        public static void FindIntersection(
            Vector2 LineStartA, Vector2 LineEndA, Vector2 LineStartB, Vector2 LineEndB, out bool segments_intersect, out Vector2 intersection)
        {
            // Get the segments' parameters.
            float dx12 = LineEndA.X - LineStartA.X;
            float dy12 = LineEndA.Y - LineStartA.Y;
            float dx34 = LineEndB.X - LineStartB.X;
            float dy34 = LineEndB.Y - LineStartB.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 =
                ((LineStartA.X - LineStartB.X) * dy34 + (LineStartB.Y - LineStartA.Y) * dx34)
                    / denominator;
            if (float.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                //lines_intersect = false;
                segments_intersect = false;
                intersection = new Vector2(float.NaN, float.NaN);
                //close_p1 = new Vector2(float.NaN, float.NaN);
                //close_p2 = new Vector2(float.NaN, float.NaN);
                return;
            }
            //lines_intersect = true;

            float t2 =
                ((LineStartB.X - LineStartA.X) * dy12 + (LineStartA.Y - LineStartB.Y) * dx12)
                    / -denominator;

            // Find the point of intersection.
            intersection = new Vector2(LineStartA.X + dx12 * t1, LineStartA.Y + dy12 * t1);

            //The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
               ((t1 >= 0) && (t1 <= 1) &&
                (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            //if (t1 < 0)
            //    t1 = 0;
            //else if (t1 > 1)
            //    t1 = 1;

            //if (t2 < 0)
            //    t2 = 0;
            //else if (t2 > 1)
            //    t2 = 1;

            //close_p1 = new Vector2(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            //close_p2 = new Vector2(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }

        public static bool CollidePolygons(Polygon MainPolygon, Vector2 MainPosition, float MainSize, float MainRotation, Polygon SecondaryPolygon, Vector2 SecondaryPosition, float SecondarySize, float SecondaryRotation, out Vector2 resolve)
        {
            Vector2 lineStartA = (MainPolygon.center * MainSize) + MainPosition;

            for (int i = 0; i < MainPolygon.vertices.Length; i++)
            {
                Vector2 lineEndA = (MainPolygon.vertices[i].RotatedBy(MainRotation, MainPolygon.center) * MainSize) + MainPosition;

                for (int j = 0; j < SecondaryPolygon.vertices.Length; j++)
                {
                    Vector2 lineStartB = (SecondaryPolygon.vertices[j].RotatedBy(SecondaryRotation, SecondaryPolygon.center) * SecondarySize) + SecondaryPosition;
                    Vector2 lineEndB = (SecondaryPolygon.vertices[(j == SecondaryPolygon.vertices.Length - 1) ? 0 : j + 1].RotatedBy(SecondaryRotation, SecondaryPolygon.center) * SecondarySize) + SecondaryPosition;
                    FindIntersection(lineStartA, lineEndA, lineStartB, lineEndB, out bool otherIntersect, out Vector2 intersectionPoint);

                    if (otherIntersect)
                    {
                        resolve = lineEndA - intersectionPoint;
                        return true;
                    }
                }
            }
            resolve = Vector2.Zero;
            return false;
        }

        public static float ToRotation(this Vector2 v) { return (float)Math.Atan2((double)v.Y, (double)v.X); }

        public static Vector2 ToVector2Rotation(this float f) { return new Vector2((float)Math.Cos((double)f), (float)Math.Sin((double)f)); }

        public static Vector2 RotatedBy(this Vector2 spinningpoint, double radians, Vector2 center = default)
        {
            float dirX = (float)Math.Cos(radians);
            float dirY = (float)Math.Sin(radians);
            Vector2 vector = spinningpoint - center;
            Vector2 result = center;
            result.X += vector.X * dirX - vector.Y * dirY;
            result.Y += vector.X * dirY + vector.Y * dirX;
            return result;
        }
    }
}