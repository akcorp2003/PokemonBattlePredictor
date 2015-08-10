# PokemonBattlePredictor

## What is this program? ##
PokemonBattlePredictor (PBP) is pretty simple: it predicts Pokemon battles! PBP is written in VB.NET and it's primary target is to predict battles on TwitchPlaysPokemon (TPP). However, the program is also written so that it can predict entire Pokemon battles as well! 

What I hope PBP can become is that it can help you maximize your earnings on TPP and also help you predict any Pokemon battles that come your way!

## How Can I Get it? ##
You can clone this entire project
 
    git clone https://github.com/akcorp2003/PokemonBattlePredictor.git

or download it as a ZIP or other ways that GitHub can let you get my repository. 

You can open the .sln Visual Studio file to see the entire project in Visual Studio or you can directly go into the Release folder and open the .exe file to see the current build. Make sure you copy all the contents in that Release folder if you are going to be moving it someplace else!!

## How Do I Use It? ##
The UI is designed as simple as possible because I believe that you don't want to see too much clutter around the interface. When you start up the program, you see this window:

![](http://i.imgur.com/jCgjDRx.png)

This is the main screen and you can click "Enter the Battling Pokemon!" which leads you here:

![](http://i.imgur.com/sai84Il.png)

You fill in the name of your Pokemon by "Search for your Pokemon:" 

![](http://i.imgur.com/sEDwIEp.png)

Ah! You can see a list of Pokemon! This program holds all Pokemon up to Generation VI (megas are in the database but I haven't carried them into the auto-complete). So once you type in the entire Pokemon name, this window pops up:

![](http://i.imgur.com/t9vEbFN.png)

This windows shows the Pokemon that you are adding and room to add up to 4 moves! The stats are pre-filled but you can type your own values as well. Likewise, there is auto-complete but my program doesn't understand all the moves yet (there's too many of them...). After inserting all the moves, you can click on "Add to Team Blue" or "Add to Team Red" to add the Pokemon to one of the teams. You can have an unlimited number of Pokemon on each team but TPP only has 3 Pokemon/team.

Once you added everything, this window should pop up now:

![](http://i.imgur.com/S4LuHmY.png)

You can see all the information of the Pokemon you added displayed in a list. At the moment, however, **you can't edit the Pokemon that you have added**. Sorry! That will be in a future release when I migrate this to WPF.

When you feel that you have added all the appropriate Pokemon, click on "Load My Pokemon!" and you should be led back to the Main Window with some new figures:

![](http://i.imgur.com/LgepQmD.png)  

Go ahead and click on "Predict!" and a small window will pop up indicating which team, red or blue, wins. It will also ask if you want to see a record of the moves the program chose for you!

The program outputs a .docx file and the general layout looks like this:

![](http://i.imgur.com/ukD7gPG.jpg)
(The first page)

![](http://i.imgur.com/0FecOGN.jpg)
(Some move information)

![](http://i.imgur.com/9gUVUhd.jpg)
(More information)

There are still some things I need to work out there so you may see some weird capital letter words printed...

## The Algorithm ##
I chose to use a greedy algorithm. When a Pokemon is about to make a move, the program chooses the best move to use in that situation. Further, moves are primarily chosen based on how fast that move can destroy the opposing Pokemon.

I am also planning to write up a documentation detailing the functions used in the program. Most of the functions are written so that if you were interested in using some of my functions to write your own .NET Pokemon game, predictor, etc. you can use what I have! 

## So What Can It Do Now? ##
The program has the turn-based mechanics of Pokemon games. It can apply regular damage using the formula from Bulbapedia and also critical damage as well. (Side note: Since it's predicting battles, I have all moves doing max damage but the program can do normal damage (i.e. no critical or other shenanigans) without fault.) 

The program can also understand how to use stat-raising moves like Swords Dance and Calm Mind, and also status moves such as, Poison Powder, and Thunder Wave. It can also make confused Pokemon hit itself.

Though I haven't field-tested it yet, it can predict some Pokemon battles choosing moves you would generally consider using, especially attacking moves. 

## What Are the Current Limitations? ##
Quite a few. Pokemon abilities are not yet implemented and many moves don't have their side effects registered yet. Also, there's is no guarantee that the program will work every time nor accurately predict a battle.
 
## What's Next? ##
As you know, there are still many things I have to tweak:


1. Support for many more moves.
2. Pokemon abilities
2. Improved prediction algorithm
3. Making PBP into a Pokemon prediction API
3. Bug fixes 

## Are You Interested in Helping? ##
I would be thrilled! This is a personal project of mine so I may be doing this on and off. Outlined in the *What's Next* heading are the things that I am looking to have completed so we can start there. You can reach me at akcorporation2003@gmail.com if you want to talk! 

Even if you wanted to take what I have written and change it all up, I wouldn't mind that either! 

## Credits ##
I *love* Paul Hallet who designed [http://www.pokeapi.co/](http://www.pokeapi.co/). I used your valuable database in building my own Pokemon dictionaries, move information, and much more. I also used a lot of Veekun's, a GitHub user, Pokedex project, which has an *incredible* amount of information: [https://github.com/veekun/pokedex](https://github.com/veekun/pokedex). Also, I wish to give many thanks to [PokemonDB](http://pokemondb.net/) for the  colourful sprites. I also would love to thank the fabulous [DocX](http://docx.codeplex.com/) library created by Cathel Coffey, which made creating .docx files so easy. In addition, I cannot forget Bulbapedia for all its valuable information, [PsyPokes](http://www.psypokes.com/index.php) for the awesome stats info, [Smogen University](http://www.smogon.com/) for their damage formula information and countless other resources, and finally the wonderful [University of Miami Department of Statistics](https://www.math.miami.edu/~jam/azure/whatsnew.htm "University of Miami") for their invaluable Pokemon statistics. 
