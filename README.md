This is a 1.12.2 minecraft server written fully in C# from scratch. It is somewhat lightweight expect the world loading, but still, not bad. 
So far not much is done but yet this is still very good for a dummy server or for a hub/lobby that can handle ten of thousands of clients on one single server without eating much memory and cpu.
Planning to rewrite parts of code from 1.12.2 to 1.20.1 with support of higher versions.

The goal with this project is to learn something new, do something while im bored instead of brainrotting, and just cuz i can.

### Currently working functions
- [x] Basic chunk loading
- [x] Block Changing/Placing
- [x] Basic chat (no commands)
- [x] Player movement
- [x] Inventory
- [ ] Commands, and colored chat
- [ ] Crafting
- [ ] Full survival mode
- [ ] Mobs
- [ ] Physics
- [ ] Redstone
- [ ] World Generation (no)
- [ ] Custom plugin framework
- [ ] Paper plugins implementation

### Need to improve
- [ ] Memory leaks while loading world
- [ ] Player not loading correctly when loading world
- [ ] Memory leaks/increase when player loads on a huge world
- [ ] Load further than regions 0.0, 0.-1, -1.0, -1.-1

I am the single person to develop the project so please be mindful of that lol.
