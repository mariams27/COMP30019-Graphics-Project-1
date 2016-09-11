Generating the terrain:
Unity's terrain object is used to generate and render the actual terrain mesh. The diamond square algorithm is used to generate a heightmap which is then passed to the terrain object. 

Diamond Square Algorithm:
The map is a 2D array of size 2^n+1. The four corners are initialised to have random values. The diamond and square steps are then performed until all the values of the array have been set.
The diamond step sets the midpoint of that diamond to be the average of the four corner points plus a random value, for each square.
The square step does the same but for each diamond. 
Every iteration reduces the magnitude of the random value that is being added. Finally, the height map is normalised to be in the range [0,1] instead of [-1,1].

Camera:
The camera had  flight simulator styled controls and utilises unity’s  pitch, yaw and roll axes to set the movement speed appropriately.

Unity’s physics engine is used to handle collisions, both the terrain and the water plane are colliders, and the camera itself has a sphere collider around it.

Surface Properties:
A vertex shader is used to determine the colour of any given vertex. To determine how high a vertex is, the minimum and maximum heights of the terrain are passed to the shader. Then given how high or low the vertex’s y value is with respect to the terrain’s height, it is assigned a colour. The colours are as follows:
	1. Below sea level = blue
	2. lower altitudes = green
	3. mountains' snowy peaks = white
	4. Between level 2 and 3 = brown

A simple sphere is used to represent the sun and it is set to slowly orbit around the terrain. The shader then uses the sun's position in the world to determine how to appropriately light up the terrain.

A fragment shader is then responsible for lighting up the terrain using the Phong's illumination model.

A water plane is added, its y value set based on the height of the terrain and a coefficient.

The project also uses an fps display script from http://wiki.unity3d.com/index.php?title=FramesPerSecond