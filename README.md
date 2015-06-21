# PokemonBattlePredictor
A Pokemon Battle Prediction Program written in VB.NET for predicting Pokemon battles on TwitchPlaysPokemon.

The ultimate goal of the program is to predict the outcome of a battle and allow the user to gain the maximum amount
of Pokedollars from the battle.

CURRENT PROGRESS:
1. 6/18/15 Successfully queried the PokeAPI, NEXT STEP: parsing the data into a database (possibly LINQ)
2. 6/20/15 Successfully parsed Pokedex file and arranged into a Dictionary of URI. At the moment, not planning to use LINQ to store data; rather, it will be a series of Dictionaries for Pokemon, Types, Moves, and Abilities. NEXT STEP: query PokeAPI for individual Pokemon and their respective data (Types, Moves, etc.), parse that data and put into file and into Pokemon Dictionary and any other Dictionaries
