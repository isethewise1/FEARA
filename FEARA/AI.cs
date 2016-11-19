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
	class AI
	{
		//stuff for keeping track of actions
		Stack<Action> _actionStack = new Stack<Action>();

		//get an action to perform for a character
		public Action getAction(Character self)
		{
			//get a goal
			Goal currGoal = getGoal(self.state);

			//if we do not have any actions left
			if (_actionStack.Count < 1)
			{
				//if there is no goal at the moment, perform the default action
				//if there is a goal, run the A* algorithm to find the best action path
				if (currGoal == null || AStar(0.0f, self.state, currGoal, out _actionStack, new List<Action>()) == -1)
					return defaultAction;
			}

			//if our current action is complete
			if (_actionStack.Peek().isComplete(self))
			{
				//remove it 
				_actionStack.Pop().Complete(self);
				//and get a new action
				Action retVal = getAction(self);
				Console.WriteLine("current action is " + retVal.Name);
				return retVal;
			}

			//return the current action
			return _actionStack.Peek();
		}


		//A* and stuff
		public float AStar(float currCost, State currState, Goal goalState, out Stack<Action> retActionStack, List<Action> newVisited)
		{
			//keep track of what you have visited
			List<Action> visited = new List<Action>(newVisited);
			//if you have reached the goal
			if (currState.Equals(goalState.GoalState))
			{
				//return the current cost and an empty stack
				retActionStack = new Stack<Action>();
				return currCost;
			}

			//get the best action with its stack and cost
			Action bestAction = null;
			Stack<Action> bestActionStack = null;
			float bestCost = -1;

			List<Action> avail = availableActions(currState);
			foreach (Action act in avail)
			{
				//if an action has already been visited, skip it
				if (visited.Contains(act))
					continue;
				//add the action we are looking at to the visited list
				visited.Add(act);
				//get the cost and the action stack
				Stack<Action> actionStack = null;
				float actCost = AStar(currCost + act.Cost, currState.addState(act.Result), goalState, out actionStack, visited);
				if (actCost == -1)
					continue;

				//if we havent found any actions yet, it is the best one
				if (bestAction == null)
				{
					bestAction = act;
					bestCost = actCost;
					bestActionStack = actionStack;
					continue;
				}

				//if our action is better than what we thought was best, it is the best one
				if (actCost < bestCost)
				{
					bestAction = act;
					bestCost = actCost;
					bestActionStack = actionStack;
				}
			}

			//return the action stack
			if (bestActionStack == null)
				retActionStack = new Stack<Action>();
			else
				retActionStack = bestActionStack;

			//if we got a new action, add it
			if (bestAction != null)
			{
				retActionStack.Push(bestAction);
			}

			//return -1 if we could not find a goal states
			return (bestAction == null ? -1 : bestCost);
		}

		//stuff governing actions
		List<Action> actions;
		public List<Action> Actions
		{
			get { return actions; }
			set
			{
				foreach (Action act in value)
				{
					//set the default action
					if (act.Name == "default")
					{
						DefaultAction = act;
						value.Remove(act);
						break;
					}
				}
				actions = value;
			}
		}

		Action defaultAction;
		public Action DefaultAction { get { return defaultAction; } set { defaultAction = value; } }

		//get a list of the available actions
		public List<Action> availableActions(State state)
		{
			List<Action> retVal = new List<Action>();

			//if we meet the precondition for the action, it is available
			foreach (Action act in actions)
			{
				if (state.Equals(act.Precondition))
					retVal.Add(act);
			}

			return retVal;
		}

		//stuff involving goals
		List<Goal> goals;
		public List<Goal> Goals { get { return goals; } set { goals = value; } }

		//returns the goals which have their conditions met
		public List<Goal> activeGoals(State state)
		{
			List<Goal> retVal = new List<Goal>();
			foreach (Goal goal in goals)
			{
				//if the goal is active, add it
				if (goal.isActive(state))
					retVal.Add(goal);
			}
			return retVal;
		}

		//get a goal for the current state
		public Goal getGoal(State state)
		{
			//find the goal that is weighted the highest
			Goal mostWeight = null;
			foreach (Goal goal in activeGoals(state))
			{
				if (mostWeight == null)
				{
					mostWeight = goal;
					continue;
				}
				if (goal.goalWeight(state) > mostWeight.goalWeight(state))
					mostWeight = goal;
			}
			return mostWeight;
		}
	}
}
