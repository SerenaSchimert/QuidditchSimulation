# 565-A2
Quidditch Simulation via Unity

Functionality:

Forces:
- Very high cohesion of players towards snitch (by far strongest force, otherwise game takes to long to finish)
- Snitch has moderate collision avoidance
- Players have very low collision avoidance
- Players have a mild repulsion/seperation from players on the same team as them

Added Player Attributes:
- Recoveryrate (float) -> how fast player exhaustion lowers while resting (sampled from distribution like other attributes)
	- (More likely to be higher in Gryffindors)
- Steadfast (bool) -> if steadfast, player will not rest when their team is losing, i.e. the team score is lower than the opponent's (sampled from a distribution, then cut of as above or below 0.5)
	- (More likely in Gryffindors)

Code Sources:
The Boids-Simulation found here:https://omaddam.github.io/Boids-Simulation/
- was used as a code base/starting point for the simulation implementation
Implementation of a method sampling player attributes from a normal distribution given mean and standard deviation derived from responses to this query: https://stats.stackexchange.com/questions/16334/how-to-sample-from-a-normal-distribution-with-known-mean-and-variance-using-a-co
Some snippets of code referenced from 'GameManager' and 'TankManager' code in unity tanks Tutorial: https://learn.unity.com/project/tanks-tutorial

Third-party Assets:
Terraine assets (topoplogy, water, related materials) were from the 'SimpleLowPolyNature' pack, which is described here: https://assetstore.unity.com/packages/3d/environments/landscapes/simple-low-poly-nature-pack-157552#content
- The license is the standard unity store license discribed here: https://unity3d.com/legal/as_terms

