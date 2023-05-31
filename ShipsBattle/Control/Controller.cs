﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShipsBattle
{
    public class Controller
    {
        public void Update(GameTime gameTime)
        {
            foreach (var sprite in Global.Sprites)
            {
                switch (sprite.GetType().Name)
                {
                    case "Player":
                    {
                        Update((Player)sprite, gameTime);
                        break;
                    }
                    case "Bullet":
                    {
                        Update((Bullet)sprite, gameTime);
                        break;
                    }
                }

                ViewDataBuilder.Build(sprite);
            }
        }

        public void PostUpdate(GameTime gameTime)
        {
            Global.AddToEntities();
            Global.RemoveFromEntities();
        }


        private void Update(Player player, GameTime gameTime)
        {
            var pressedKey = Keyboard.GetState();
            var input = player.Input;

            if (pressedKey.IsKeyDown(input.RotateLeft))
                player.Rotate(-MathHelper.ToRadians(player.RotationVelocity));

            if (pressedKey.IsKeyDown(input.RotateRight))
                player.Rotate(MathHelper.ToRadians(player.RotationVelocity));

            player.Direction = Directions.CalculateDirection(player.Rotation);

            if (pressedKey.IsKeyDown(input.Up))
                player.Move(player.Direction * player.Speed);

            if (pressedKey.IsKeyDown(input.Down))
                player.Move(-player.Direction * player.Speed);

            if (pressedKey.IsKeyDown(input.Left))
                player.Move(new Vector2(player.Direction.Y, -player.Direction.X) * player.Speed);

            if (pressedKey.IsKeyDown(input.Right))
                player.Move(-new Vector2(player.Direction.Y, -player.Direction.X) * player.Speed);


            if (pressedKey.IsKeyDown(input.Shoot))
                player.Shoot();

            foreach (var sprite in Global.Sprites)
            {
                if(sprite == player) continue;
                if (sprite.Rectangle.Intersects(player.Rectangle) && sprite.GetType().Name == "Bullet")
                {
                    var bullet = sprite as Bullet;
                    if (bullet?.Parent != player)
                        player.IsDied = true;
                }
            }

            if (player.IsDied)
                Global.RemoveEntity(player);
        }

        private void Update(Bullet bullet, GameTime gameTime)
        {
            bullet.Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (bullet.Timer > bullet.LifeSpan)
            {
                bullet.IsRemoved = true;
            }
            
            bullet.Move(bullet.Direction * bullet.LinerVelocity);
        }

    }
}
