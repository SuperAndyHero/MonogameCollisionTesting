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
    public struct Polygon
    {
        public Vector2[] vertices;
        public Vector2 center;

        public Polygon(Vector2[] Points)
        {
            vertices = Points;
            center = new Vector2();
            SetCenter();
        }
        public void SetCenter()
        {
            Vector2 average = new Vector2();
            foreach(Vector2 point in vertices)
            {
                average += point;
            }
            average /= vertices.Length;
            center = average;
        }
    }

    public struct PolygonFillDrawer
    {
        public VertexPositionColor[] vertexPos;
        public short[] indices;

        public PolygonFillDrawer(ref Polygon polygon, Color color)
        {
            vertexPos = Helper.MakeVertexPosList(ref polygon.vertices, color).ToArray();
            indices = new short[(polygon.vertices.Length - 2) * 3];
            SetIndices(ref polygon.vertices);
        }

        public PolygonFillDrawer(ref Polygon polygon)
        {
            vertexPos = Helper.MakeVertexPosListRainbow(ref polygon.vertices).ToArray();
            indices = new short[(polygon.vertices.Length - 2) * 3];
            SetIndices(ref polygon.vertices);
        }

        public void DrawPolygon(BasicEffect effect, EffectPass pass, GraphicsDevice graphicsDevice, Vector2 position, Vector2 center, float Size = 1f, float Rotation = 0)
        {
            graphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };//TODO remove

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertexPos.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertexPos);

            IndexBuffer indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);

            Matrix rotateAroundCenter = Matrix.CreateTranslation(-center.X, -center.Y, 0) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(center.X, center.Y, 0);
            Matrix screenOffset = Matrix.CreateTranslation(Game1.screenPosition.X - position.X, -Game1.screenPosition.Y + position.Y, 0);
            effect.World = rotateAroundCenter * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(Size) * screenOffset;
            effect.View = Matrix.CreateScale(Game1.zoom) * Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateRotationX(MathHelper.Pi);

            pass.Apply();
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;
            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexBuffer.IndexCount - 2);
        }

        public void SetIndices(ref Vector2[] vertices)//magic fill (only works up to 32~ vertices)
        {
            int pointCount = vertices.Length;
            int indexCount = indices.Length;

            int indicesIndex = 1;
            int wrapCount = 0;
            int wrapStartOffset = 0;
            int oddNumberOffset = 0; //2*countvarA
            int wrapNumA1 = 0;
            int ABoffset = 0;

            for (int numberIndex = indicesIndex; indicesIndex <= indexCount; numberIndex = (int)(indicesIndex - Math.Floor(((double)indicesIndex - 1f) / 3f)))
            {

                if (numberIndex > pointCount && wrapStartOffset == 0)
                {
                    wrapStartOffset = numberIndex - 1;
                }

                int numOut = getX();
                if (numOut > pointCount)
                {
                    wrapStartOffset = numberIndex - 1;
                    int XYoffset = (pointCount % 2 == 1 ? 2 : 0);
                    oddNumberOffset = XYoffset + ((wrapNumA1 + (wrapCount * 4)) - ABoffset);
                    ABoffset = (pointCount % 4 < 2) ? 4 : 0;
                    wrapNumA1 = (wrapCount * 4);
                    wrapCount++;
                    numOut = getX();
                }

                indices[indicesIndex - 1] = (short)(numOut - 1);
                indicesIndex++;

                int getX()
                {
                    int wrapNumberSkip = (2 * Math.Max((wrapCount * 2) - 1, 0));
                    return numberIndex > pointCount ? (((numberIndex - wrapStartOffset) * (2 + wrapNumberSkip)) - (1 + wrapNumberSkip)) + oddNumberOffset : numberIndex;
                }
            }
        }
    }

    public struct PolygonOutlineDrawer
    {
        public VertexPositionColor[] vertexPos;

        public PolygonOutlineDrawer(ref Polygon polygon, Color color)
        {
            List<VertexPositionColor> vertexList = Helper.MakeVertexPosList(ref polygon.vertices, color);
            vertexList.Add(new VertexPositionColor(new Vector3(polygon.vertices[0], 0), color));
            vertexPos = vertexList.ToArray();
        }

        public PolygonOutlineDrawer(ref Polygon polygon)
        {
            List<VertexPositionColor> vertexList = Helper.MakeVertexPosListRainbow(ref polygon.vertices);
            vertexList.Add(new VertexPositionColor(new Vector3(polygon.vertices[0], 0), vertexList[0].Color));
            vertexPos = vertexList.ToArray();
        }

        public void DrawPolygon(BasicEffect effect, EffectPass pass, GraphicsDevice graphicsDevice, Vector2 position, Vector2 center, float Size = 1f, float Rotation = 0f)
        {
            graphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };//TODO remove

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertexPos.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertexPos);

            Matrix rotateAroundCenter = Matrix.CreateTranslation(-center.X, -center.Y, 0) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(center.X, center.Y, 0);
            Matrix screenOffset = Matrix.CreateTranslation(Game1.screenPosition.X - position.X, -Game1.screenPosition.Y + position.Y, 0);
            effect.World = rotateAroundCenter * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(Size) * screenOffset;
            effect.View = Matrix.CreateScale(Game1.zoom) * Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateRotationX(MathHelper.Pi);

            pass.Apply();
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, vertexBuffer.VertexCount - 1);
        }
    }

    public struct PolygonOutwardDrawer
    {
        public VertexPositionColor[] vertexPos;
        public short[] indices;

        public PolygonOutwardDrawer(ref Polygon polygon, Color color)
        {
            List<VertexPositionColor> vertexList = Helper.MakeVertexPosList(ref polygon.vertices, color);
            vertexList.Add(new VertexPositionColor(new Vector3(polygon.center, 0), color));
            vertexPos = vertexList.ToArray();
            indices = new short[polygon.vertices.Length * 2];
            SetIndices(ref polygon.vertices);
        }

        public PolygonOutwardDrawer(ref Polygon polygon)
        {
            List<VertexPositionColor> vertexList = Helper.MakeVertexPosListRainbow(ref polygon.vertices);
            vertexList.Add(new VertexPositionColor(new Vector3(polygon.center, 0), Game1.random.NextColor()));
            vertexPos = vertexList.ToArray();
            indices = new short[polygon.vertices.Length * 2];
            SetIndices(ref polygon.vertices);
        }

        public void DrawPolygon(BasicEffect effect, EffectPass pass, GraphicsDevice graphicsDevice, Vector2 position, Vector2 center, float Size = 1f, float Rotation = 0f)
        {
            graphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };//TODO remove

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertexPos.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertexPos);

            IndexBuffer indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);


            Matrix rotateAroundCenter = Matrix.CreateTranslation(-center.X, -center.Y, 0) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(center.X, center.Y, 0);
            Matrix screenOffset = Matrix.CreateTranslation(Game1.screenPosition.X - position.X, -Game1.screenPosition.Y + position.Y, 0);
            effect.World = rotateAroundCenter * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(Size) * screenOffset;
            effect.View = Matrix.CreateScale(Game1.zoom) * Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateRotationX(MathHelper.Pi);

            pass.Apply();
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;
            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, indexBuffer.IndexCount - 2);
        }

        public void SetIndices(ref Vector2[] vertices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                if (i % 2 == 0)
                {
                    indices[i] = (short)(i / 2);
                }
                else indices[i] = (short)vertices.Length;
            }
        }
    }
}