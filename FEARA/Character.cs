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
	class Character : EnvironmentObject
	{
		//the speed of the character
		const float speed = 150.0f;

		//keeps track of the character's state
		State thisState = new State();
		public State state { get { return thisState; } set { thisState.setState(value); } }

		//holds the characters ai
		AI ai;
		public AI AI { get { return ai; } set { ai = value; } }

		//what the character is carrying
		EnvironmentObject carrying = null;
		public EnvironmentObject Carrying { get { return carrying; } set { carrying = value; } }

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);
			if (carrying != null)
				carrying.Draw(sb, Position + new Vector2(texture.Width / 2, texture.Height / 2));

			state.Draw(sb);
		}

		public void Update(GameTime time)
		{
			//update the current action
			Action currAction = ai.getAction(this);
			//move toward where you need to go
			Position += Vector2.Normalize(Vector2.Subtract(currAction.MoveTo.Position, Position)) * (speed * time.ElapsedGameTime.Milliseconds * 0.001f);

			//update the state variables
			state.Update(time);
		}
	}
}
