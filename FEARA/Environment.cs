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
	class Environment
	{
		//the objects and characters on the screen
		List<EnvironmentObject> objects;
		List<Character> chars;

		public Environment(List<EnvironmentObject> objs, List<Character> chs)
		{
			objects = objs;
			chars = chs;
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (EnvironmentObject obj in objects)
			{
				obj.Draw(sb);
			}
			foreach (Character ch in chars)
			{
				ch.Draw(sb);
			}
		}

		public void Update(GameTime time)
		{
			foreach (Character ch in chars)
			{
				ch.Update(time);
			}
		}
	}
}
