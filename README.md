# 565-A2
Quidditch Simulation via Unity (Version: 2020.2.6f1)

Functionality:
This is a aimplified quidditch simulation, where the goal of the players is to catch the snitch (gold object). 2 points are earned for consequetive catches, and 100 points is winning the game. The red (Gryffindor) and green (Slytherin) teams are assigned attributes based on a given mean and standard deviation, such that all players have a value sampled from the team distrubution (differs by red vs green) as per the Box Muller Transform. The four main player attributes are maximum velocity, weight, aggresiveness and maximum exhaustion (additional attributes described below). Exhaustion increases with player movement, and the player will fall to the ground if the maximum limit is reached and respawn in front of the red and green boxes respectively. This sequence of falling and respawn can also occur during a 'tackle' with either another player or the ground (collision). If the player is on the opposing team, the player with more energy (lower percentge of max exhaustion) and higher aggresiveness weighted together will tackle the weaker player, otherwise there is only a 5% chance of the weaker player being tackled. Weight affects acceleration (slower to gain speed), and maximum velocity is as the name implies. 

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

