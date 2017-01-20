using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Tiled.Renderers;
using Demo.Scenes;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Collisions;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Demo.Scenes
{
    class Area_1 : SceneManager
    {
        private TiledMap map;
        public static ViewportAdapter viewPortAdapter;
        private IMapRenderer mapRenderer;
        private Queue<string> maps;

        public static Entity player;
        public static Player playerData;

        Camera2D camera;

        CollisionWorld collision;

        public static KeyboardState oldState;
        public static KeyboardState newState;

        public Area_1(Game game, GameWindow window) : base(game)
        {

        }
    }
}
