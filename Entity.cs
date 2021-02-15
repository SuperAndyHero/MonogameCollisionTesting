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
    public class Entity
    {
        public Vector2 position = Vector2.Zero;
        public float size;
        public float rotation;

        //public Vector2 Center => position + (hitbox.center * size);//center in world coords
        public Vector2 Center {
            get => position + (hitbox.center * size);
            set => position = value - (hitbox.center * size); }

        //public Vector2 velocity = new Vector2();
        //public Vector2 oldPosition = new Vector2();
        //public Vector2 oldVelocity = new Vector2();

        public Polygon hitbox;

        public PolygonFillDrawer polygonFill;
        public PolygonOutlineDrawer polygonOutline;
        public PolygonOutwardDrawer polygonOutward;


        public Entity(Vector2 Positon, float Size = 32f, float Rotation = 0f)  {
            position = Positon;
            size = Size;
            rotation = Rotation;
            SetHitbox(Helper.DefaultPolygon); }

        public Entity(Vector2 Positon, Polygon Hitbox, float Size = 32f, float Rotation = 0f) { 
            position = Positon; 
            size = Size;
            rotation = Rotation;
            SetHitbox(Hitbox); }


        public void SetHitbox(Polygon polygon)
        {
            hitbox = polygon;
            polygonFill = new PolygonFillDrawer(ref hitbox);
            polygonOutward = new PolygonOutwardDrawer(ref hitbox, Color.IndianRed);
            polygonOutline = new PolygonOutlineDrawer(ref hitbox, Color.Black);//make new instance when shape changes
        }

        private const int dotSize = 2;
        public void Draw(SpriteBatch spriteBatch)
        {
            int index = 0;
            foreach (Vector2 point in hitbox.vertices)
            {
                spriteBatch.Draw(Game1.debugTexture, new Rectangle(((((point * size) + position) - (new Vector2(dotSize) / 2f)) - Game1.screenPosition).ToPoint(), new Point(dotSize)), null, Color.Black, default, default, default, default);
                spriteBatch.DrawString(Game1.font_Arial, index.ToString(), ((point * size) + position) - Game1.screenPosition, Color.Black);
                index++;
            }
            spriteBatch.Draw(Game1.debugTexture, new Rectangle(((((hitbox.center * size) + position) - (new Vector2(dotSize) / 2f)) - Game1.screenPosition).ToPoint(), new Point(dotSize)), null, Color.Black, default, default, default, default);
            spriteBatch.Draw(Game1.debugTexture, new Rectangle((position - (new Vector2(dotSize * 2) / 2f) - Game1.screenPosition).ToPoint(), new Point(dotSize * 2)), null, Color.LightSeaGreen, default, default, default, default);

        }

        public void Update()
        {
            Collision();
        }

        public void Collision()
        {
            foreach (Entity npc in Game1.entities)
            {
                if (npc != this)
                {
                    if (Helper.CollidePolygons(hitbox, position, size, rotation, npc.hitbox, npc.position, npc.size, npc.rotation, out Vector2 resolveOffset))
                    {
                        position -= resolveOffset / 2;
                        npc.position += resolveOffset / 2;
                        //Vector2 a = resolveOffset;
                    }
                }
            }
        }

        public void DrawHitboxFill(BasicEffect effect, EffectPass pass, GraphicsDevice graphicsDevice)
        {
            polygonFill.DrawPolygon(effect, pass, graphicsDevice, position, hitbox.center, size, rotation);
        }

        public void DrawHitboxOutline(BasicEffect effect, EffectPass pass, GraphicsDevice graphicsDevice)
        {
            polygonOutline.DrawPolygon(effect, pass, graphicsDevice, position, hitbox.center, size, rotation);
            polygonOutward.DrawPolygon(effect, pass, graphicsDevice, position, hitbox.center, size, rotation);
        }
    }
}