using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEARA
{
    class Goal
    {
        string name;
        public string Name { get { return name; } set { name = value; } }

        State goalState;
        public State GoalState { get { return goalState; } set { goalState = value; } }

        string weightFeature;
        public string WeightFeature { get { return weightFeature; } set { weightFeature = value; } }

        int weightLimit;
        public int WeightLimit { get { return weightLimit; } set { weightLimit = value; } }

        float weight;
        public float Weight { get { return weight; } set { weight = value; } }

        //have the conditions for the goal been met?
        public bool isActive(State state)
        {
            return int.Parse(state.getValue(WeightFeature)) > WeightLimit;
        }

        //the amount of attention to pay to this goal
        public float goalWeight(State state)
        {
            return (int.Parse(state.getValue(WeightFeature)) - WeightLimit)*weight;
        }
    }
}