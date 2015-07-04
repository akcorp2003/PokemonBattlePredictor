# PokemonBattlePredictor
A Pokemon Battle Prediction Program written in VB.NET for predicting Pokemon battles on TwitchPlaysPokemon.

The ultimate goal of the program is to predict the outcome of a battle and allow the user to gain the maximum amount
of Pokedollars from the battle.

CURRENT PROGRESS:
1. 6/18/15 Successfully queried the PokeAPI, NEXT STEP: parsing the data into a database (possibly LINQ)

2. 6/20/15 Successfully parsed Pokedex file and arranged into a Dictionary of URI. At the moment, not planning to use LINQ to store data; rather, it will be a series of Dictionaries for Pokemon, Types, Moves, and Abilities. NEXT STEP: query PokeAPI for individual Pokemon and their respective data (Types, Moves, etc.), parse that data and put into file and into Pokemon Dictionary and any other Dictionaries.
 
3. 6/21/15 Program can now accept a Pokemon name, query the database or dictionary, and add it to the Pokemon Dictionary (if applicable). The lack of a progress bar makes the user feel like the program is hung so in future updates, it will be good to add a progress bar. Now, beginning the phases of establishing the Pokemon teams. NEXT STEP: finish implementing Pokemon teams, begin battle logic.
 
4. 6/24/15 Small Progress: Pokemon is now displayed in the ListView. Battle logic implementation begins here.
 
5. 6/26/15 Small Progress: IO is now implemented. This reduces the need to constantly go to the server, which is quite slow. Also, this serves as the ability to cache all the data.

CURRENT ACTIVE BRANCH: BattleLogic
