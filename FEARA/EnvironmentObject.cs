using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace FEARA
{
	class EnvironmentObject
	{
		//holds a list of all the objects in the game
		static List<EnvironmentObject> objects = new List<EnvironmentObject>();
		//retrieves an object by name
		public static EnvironmentObject getObjectByName(string name)
		{
			if (name == "null") return null;

			foreach (EnvironmentObject obj in objects)
			{
				if (obj.name == name)
					return obj;
			}

			return null;
		}

		Game1 game;
		public Game1 Game { get { return game; } set { game = value; } }

		string name;
		public string Name { get { return name; } set { name = value; } }
		Vector2 position;
		public Vector2 Position { get { return position; } set { position = value; } }
		protected Texture2D texture;
		public string Texture { set { texture = game.Content.Load<Texture2D>(value); } }
		float scale;
		public float Scale { get { return scale; } set { scale = value; } }

		public virtual void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
		}

		public void Draw(SpriteBatch sb, Vector2 position)
		{
			sb.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
		}

		public EnvironmentObject()
		{
			objects.Add(this);
		}
	}
}
