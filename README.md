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

6. 7/4/2015 Lots of Progress: IO is now more or less finished. Any additional logic that needs to be added can simply be done in the Dex_reader (has a CSV parser), and Dex_Writer classes. Additional change was adding the battle mechanics. The basic turn-based mechanics is now implemented, with the exception of status moves but that will be very easy to modify. Stat moves now have working effects! Most of the prediction engine is functioning now. For stat moves, it is only looking at a one stage boost at a t...(line truncated)...

7. 7/27/2015 Battle Logic and decision engine (mostly) implemented. There could still be a few bugs that need to be squashed.
 
8. 8/7/2015 The end is complete. Although there are still many moves left to implement, the foundational code is all there. Most battle mechanisms have been implemented. All that is left to do are creating a beautiful .docx output file, some UI tweaks. and of course, documentation.
